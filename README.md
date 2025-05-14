# 🐾 Pet Customization App

A cute and customizable WPF desktop application where users can name, design, and manage their virtual pets! Built with C# and MVVM design pattern.

---

## 🎯 Features

- 🐶 Create and customize pets (name, age, species, color, fur).
- 📋 Maintain a list of pets owned by the user.
- 👁️ View all pets in a separate window. --> 🔁 Double-click a pet to reload it into the editor.
- 💾 Fully data-bound UI with layered architecture (MVVM).
-- Later maybe a database for the users

---

## 🧠 Technologies Used

- C# with .NET
- WPF (Windows Presentation Foundation)
- MVVM pattern
- CommunityToolkit.Mvvm (for modern MVVM support)

---

## 🧱 Project Structure

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


## 🐾 1. Model Classes
Pet.cs

Represents a customizable pet with properties like:
Name, Age: Basic info
Species: Enum (e.g., dog, cat, goldfish)
Fur, Color: Enums used to customize appearance
ImagePath: Computed image path based on customization
The Pet class implements INotifyPropertyChanged so that UI elements bound to it can update when properties change.

Owner.cs

Represents the pet owner:
Name: Owner's name
Pet: A BindingList<Pet> – the list of pets owned
Also implements INotifyPropertyChanged for data binding.


## 🧠 2. ViewModel
ViewModel.cs

Main logic layer connecting the UI and models. Key responsibilities:

Holds instances of CurrentPet and Owner

Manages commands like AddToOwnersPet using RelayCommand

Properties like CanViewAllPets enable/disable buttons conditionally

UpdatePetFlags() adjusts visibility (e.g., goldfish → no fur)

update() finalizes a pet, and resets flags

Logic ensures a new Pet is created after adding one to the owner list (deep copy) to avoid duplicate references.

## 🏠 3. MainWindow.xaml

This is the login/start screen where:
User enters their name and a pet name
A button navigates to EditWindow.xaml
Event Handling:
```plaintext
private void SaveButton_Click(object sender, RoutedEventArgs e)
{
    viewModel.Owner.Name = OwnerNameTextBox.Text;
    viewModel.CurrentPet.Name = PetNameTextBox.Text;
    var editWindow = new EditWindow(viewModel);
    editWindow.Show();
    this.Close();
}

```

## ✨ 4. EditWindow.xaml
The main customization interface. Features:

Sliders or dropdowns for species, color, fur

Image updates live with changes

Button to "Add to Owner's Pets"

Button to "View All Pets" (only enabled if 2+ pets exist)

```
<Button Content="View All Pets"
        IsEnabled="{Binding CanViewAllPets}"
        Click="ViewAllPets_Click"/>

```

```
private void ViewAllPets_Click(object sender, RoutedEventArgs e)
{
    var ownerWindow = new OwnerPetWindow(viewModel);
    ownerWindow.Show();
}

```
