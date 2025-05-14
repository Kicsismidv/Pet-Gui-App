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
