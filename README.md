#  Pet Customization App

A cute and customizable WPF desktop application where users can name, design, and manage their virtual pets! Built with C# and MVVM design pattern.

![Welcome](https://github.com/user-attachments/assets/b1c0fde2-322a-4654-a387-c3345bf78d2a)
![EditThelma](https://github.com/user-attachments/assets/5a2deaa4-d18b-4be9-ad16-f178c2d50409)
![ThelmasPassport](https://github.com/user-attachments/assets/6e7911a1-1c90-456f-92f1-7023658e8671)
![Pets](https://github.com/user-attachments/assets/12de9c7b-0ce0-4520-b26a-a619d17e1c75)


---

##  Features

-  Create and customize pets (name, age, species, color, fur).
-  Maintain a list of pets owned by the user.
-  View all pets in a separate window. --> Double-click a pet to reload it into the editor.
- Fully data-bound UI with layered architecture (MVVM).
-- Later maybe a database for the owners.

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
├── {PetGuiProject.Data/  # Later a database - not existing yet}
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
```csharp
public class Pet : INotifyPropertyChanged
{
    public string ImagePath { get; set; }
    private string name { get; set; }
    public string Name { get => name;  set { name = value; OnPropertyChanged(); } }    
    private Owner owner;
    private Species specie;
    public Species Species { get => specie;  set { specie = value; OnPropertyChanged(); } }
    private color color;
    public color Color { get => color; set { color = value; OnPropertyChanged(); } }
    private fur fur;
    public fur Fur { get => fur;        set { fur = value; OnPropertyChanged(); } }
    private int age { get; set; }
    public int Age { get => age; set { age = value; OnPropertyChanged(); } }
    public Pet()
    { 
        Name = string.Empty;
        ImagePath = "Pets\\Bunny\\Basic\\Black.png";




    }
    private string GetImageUri(string relativePath)
    {
        var fullPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relativePath);
        return new Uri(fullPath).AbsoluteUri;
    }
/*
Since the imagepath depends on the current condition of the pets' properties
 it is necessary to be set in each possibility.*/

    public void UpdateImagePath()
    {
        switch (Species)
        {
            case Species.bunny:
                switch (Fur)
                {
                    case fur.basic:
                        ImagePath = GetImageUri($"Pets/Bunny/Basic/{Color}.png");
                        break;
                    case fur.extrapuffy:
                        ImagePath = GetImageUri($"Pets/Bunny/Fluffy/{Color}.png");
                        break;
                    case fur.spotted:
                        ImagePath = GetImageUri($"Pets/Bunny/Spotted/{Color}.png");
                        break;
                }
                break;

            case Species.chick:
                switch (Fur)
                {
                    case fur.basic:
                        ImagePath = GetImageUri("Pets/Chicken/Basic.png");
                        break;
                    case fur.extrapuffy:
                        ImagePath = GetImageUri("Pets/Chicken/Fluffy.png");
                        break;
                    case fur.spotted:
                        ImagePath = GetImageUri("Pets/Chicken/Spotted.png");
                        break;
                }
                break;

            case Species.goldfish:
                ImagePath = GetImageUri($"Pets/Goldfish/{Color}.png");
                break;

           
                
            case Species.guineapig:
                switch (Fur)
                {
                    case fur.basic:
                        ImagePath = GetImageUri($"Pets/Guineapig/Basic/{Color}.png");
                        break;
                    case fur.extrapuffy:
                        ImagePath = GetImageUri($"Pets/Guineapig/Fluffy/{Color}.png");
                        break;
                    case fur.spotted:
                        ImagePath = GetImageUri($"Pets/Guineapig/Spotted/{Color}.png");
                        break;
                }
                break;
          ;
        }

        OnPropertyChanged(nameof(ImagePath));
    }
    private void OnPropertyChanged([CallerMemberName] string propetyName = "")
    {
        this.PropertyChanged?
            .Invoke(this, new PropertyChangedEventArgs(propetyName));
    }
    public event PropertyChangedEventHandler? PropertyChanged;
}
```

#### `Owner.cs`

Represents a pet owner.

* **Properties**:

  * `Name`: Owner’s name.
  * `Pet`: `BindingList<Pet>` representing the pets owned.
* **Implements**: `INotifyPropertyChanged`.
```csharp
  public class Owner : INotifyPropertyChanged
  {
      private string name { get; set; }
      public string Name { get { return name; } set { name = value;  OnPropertyChanged(); } }    
      private BindingList<Pet> pet { get; set; }
      public BindingList<Pet> Pet{ get { return pet; } set { pet = value; OnPropertyChanged(); } }
      public Owner()
      { 
          Name = string.Empty;
          Pet = new BindingList<Pet>();
      }
      private void OnPropertyChanged([CallerMemberName] string propetyName = "")
      {
          this.PropertyChanged?
              .Invoke(this, new PropertyChangedEventArgs(propetyName));
      }

      public event PropertyChangedEventHandler? PropertyChanged;
  }
```
---

###  2. ViewModel Layer

#### `ViewModel.cs`

Serves as the mediator between views and models.

* **Properties**:

  * `CurrentPet`: The currently edited pet.
  * `Owner`: The owner and their pet list.
  * `CanViewAllPets`: Enables "View All Pets" button when pet list has 2+ items.
  * `PetHasFur`, `PetHasColor`: Toggles visibility depending on species. Goldfish specie inable to set fur type, and Chick specie inable to set color.
  * `PetIsReady`: Temporary flag for visual confirmation.

* **Commands**:

  * `AddToOwnersPet`: Adds a deep copy of `CurrentPet` to the owner's list and resets `CurrentPet`.

* **Methods**:

  * `update()`: Confirms pet settings.
  * `UpdatePetFlags()`: Adjusts flags based on species (e.g., goldfish has no fur).

```csharp

 public class ViewModel : INotifyPropertyChanged
 {
     public IRelayCommand AddToOwnersPet { get; set; }
     public event PropertyChangedEventHandler? PropertyChanged;

/* The following booleans has to be setted each time
when we change a pet's properties, so these need to updated
 with `OnPropertyChanged();` */.


     private bool canViewAllPets;
     public bool CanViewAllPets
     {
         get => canViewAllPets;
         set
         {
             if (canViewAllPets != value)
             {
                 canViewAllPets = value;
                 OnPropertyChanged();
             }
         }
     }
    private bool petHasFur = true;
     public bool PetHasFur
     {
         get => petHasFur;
         set
         {
             if (petHasFur != value)
             {
                 petHasFur = value;
                 OnPropertyChanged(nameof(PetHasFur));
              
             }
         }
     }
     private bool petHasColor = true;
     public bool PetHasColor
     {
         get => petHasColor;
         set
         {
             if (petHasColor != value)
             {
                 petHasColor = value;
                 OnPropertyChanged(nameof(PetHasColor));
             }
         }
     }


     private Pet currentPet;
     public Pet CurrentPet
     {
         get => currentPet;
         set { currentPet = value; OnPropertyChanged(); }
     }
     public Owner Owner { get; set; }

 



     public ViewModel()
     {
         CurrentPet = new Pet();
         Owner = new Owner();
         CanViewAllPets = false;


  /* This  event subscription listens to changes in the properties of the CurrentPet object
     specifically when its Species changes
and then reacts by calling a method called UpdatePetFlags().. Which is used to update `PetHasFur` and `PetHasColor`bool values.
*/
         CurrentPet.PropertyChanged += (s, e) =>
         {
             if (e.PropertyName == nameof(CurrentPet.Species))
             {
                 UpdatePetFlags();
             }
         };
        
         PetIsReady = false;
         PetHasFur = true;
         PetHasColor = true;
     AddToOwnersPet = new RelayCommand(
            () =>
            {
                var petCopy = new Pet
                {
                    Name = CurrentPet.Name,
                    Age = CurrentPet.Age,
                    Species = CurrentPet.Species,
                    Fur = CurrentPet.Fur,
                    Color = CurrentPet.Color
                };
                petCopy.UpdateImagePath();
               
                Owner.Pet.Add(petCopy);

/*Since  the owner's list updated here
the `CanViewAllPets` also setted here.*/

                if (Owner.Pet.Count >= 2)
                {
                    CanViewAllPets = true;
                }
                MessageBox.Show($" {petCopy.Name} added to Owner's list");
            },
            () => CurrentPet != null   
        );
         
     }
  

     private void UpdatePetFlags()
     {
         PetHasFur = CurrentPet.Species != Species.goldfish;
         PetHasColor = CurrentPet.Species != Species.chick;
     }

     protected void OnPropertyChanged([CallerMemberName] string name = "")
         => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

 }
```
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



