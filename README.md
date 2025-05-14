#  Pet Customization App

A cute and customizable WPF desktop application where users can name, design, and manage their virtual pets! Built with C# and MVVM design pattern.

---

##  Features

-  Create and customize pets (name, age, species, color, fur).
-  Maintain a list of pets owned by the user.
-  View all pets in a separate window. --> Double-click a pet to reload it into the editor.
- Fully data-bound UI with layered architecture (MVVM).
-- Later maybe a database for the users

---

##  Technologies Used

- C# with .NET
- WPF (Windows Presentation Foundation)
- MVVM pattern
- CommunityToolkit.Mvvm (for modern MVVM support)

---

##  Project Structure

```plaintext
PetCustomizationApp/
├── PetGuiProject/           # WPF UI
│   ├── Views/
│   ├── ViewModels/
│   └── App.xaml, MainWindow.xaml, etc.
├── PetGuiProject.Models/    # Domain models (Pet, Owner, Enums)
├── PetGuiProject.Tests/     # (optional) for unit tests
├── PetGuiProject.sln  # Solution file
├── {PetGuiProject.Data/  # Later a database - not exitsing yet}
└── README.md
```

##  Pet Customization WPF App 

###  1. Model Classes

#### `Pet.cs`

Represents a customizable pet.

* **Properties**:

  * `Name`, `Age`: Basic identity details.
  * `Species`: Enum type (e.g., dog, cat, goldfish).
  * `Fur`, `Color`: Enum types to customize the appearance.
  * `ImagePath`: Computed string that reflects the pet’s visual representation.
* **Implements**: `INotifyPropertyChanged` to support UI updates on property changes.

#### `Owner.cs`

Represents a pet owner.

* **Properties**:

  * `Name`: Owner’s name.
  * `Pet`: `BindingList<Pet>` representing the pets owned.
* **Implements**: `INotifyPropertyChanged`.

---

###  2. ViewModel Layer

#### `ViewModel.cs`

Serves as the mediator between views and models.

* **Properties**:

  * `CurrentPet`: The currently edited pet.
  * `Owner`: The owner and their pet list.
  * `CanViewAllPets`: Enables "View All Pets" button when pet list has 2+ items.
  * `PetHasFur`, `PetHasColor`: Toggles visibility depending on species.
  * `PetIsReady`: Temporary flag for visual confirmation.

* **Commands**:

  * `AddToOwnersPet`: Adds a deep copy of `CurrentPet` to the owner's list and resets `CurrentPet`.

* **Methods**:

  * `update()`: Confirms pet settings.
  * `UpdatePetFlags()`: Adjusts flags based on species (e.g., goldfish has no fur).

---

###  3. MainWindow\.xaml

This is the entry screen:

* User inputs their name and their pet’s name.
* On click, navigates to `EditWindow.xaml`.

#### Event Handler:

```csharp
private void SaveButton_Click(object sender, RoutedEventArgs e)
{
    viewModel.Owner.Name = OwnerNameTextBox.Text;
    viewModel.CurrentPet.Name = PetNameTextBox.Text;
    var editWindow = new EditWindow(viewModel);
    editWindow.Show();
    this.Close();
}
```

---

### 4. EditWindow\.xaml

This is the customization screen:

* Dropdowns/sliders for species, fur, and color.
* Image updates based on selected traits.
* `Add to Owner's Pets` button adds a deep copy of the current pet.
* `View All Pets` button is only enabled when pet count >= 2.

#### View All Pets Button:

```xml
<Button Content="View All Pets"
        IsEnabled="{Binding CanViewAllPets}"
        Click="ViewAllPets_Click" />
```

#### Event Handler:

```csharp
private void ViewAllPets_Click(object sender, RoutedEventArgs e)
{
    var ownerWindow = new OwnerPetWindow(viewModel);
    ownerWindow.Show();
}
```

---

###  5. OwnerPetWindow\.xaml

Displays the list of owned pets:

* Shows `Name`, `Species`, and a small image.
* **Double-click** on an item sets it as the current pet.

#### Event Handler:

```csharp
private void PetListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
{
    if (PetListBox.SelectedItem is Pet selectedPet)
    {
        viewModel.CurrentPet = selectedPet;
        this.Close();
    }
}
```

---

###  6. PassportWindow\.xaml 

If included, this window can display full pet info as a "passport". Could be used for documentation, printing, or detailed view per pet.

---



