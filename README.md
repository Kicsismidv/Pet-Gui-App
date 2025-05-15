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

* User inputs their name .
* On click, navigates to `EditWindow.xaml`.

#### Event Handler:

```xaml
 <!--- First, I set the DataContext to an instance of the ViewModel so I can access its properties and commands in XAML. --- >
    <Window.DataContext>
        <local:ViewModel />
    </Window.DataContext>
 <!--- Then, I set the style for these items, so i don't have to do this everywhere and also that way its more uniform --- >
    <Window.Resources>
        <Style TargetType="TextBox">
            <Setter Property="FontSize" Value="20"/>
            <Setter Property="Padding" Value="8"/>
            <Setter Property="Margin" Value="10"/>
            <Setter Property="BorderThickness" Value="2"/>
            <Setter Property="BorderBrush" Value="#D895C5"/>
            <Setter Property="Background" Value="#FFFFFF"/>
        </Style>

        <Style TargetType="Label">
            <Setter Property="FontSize" Value="22"/>
            <Setter Property="Foreground" Value="#D36DA8"/>
            <Setter Property="Margin" Value="10"/>
        </Style>

        <Style TargetType="Button">
            <Setter Property="FontSize" Value="22"/>
            <Setter Property="Padding" Value="10,5"/>
            <Setter Property="Margin" Value="20"/>
            <Setter Property="Background" Value="#FAD0E9"/>
            <Setter Property="BorderBrush" Value="#D895C5"/>
            <Setter Property="BorderThickness" Value="2"/>
            <Setter Property="Template">
                <Setter.Value>
                    <ControlTemplate TargetType="Button">
                        <Border Background="{TemplateBinding Background}" 
                                BorderBrush="{TemplateBinding BorderBrush}" 
                                BorderThickness="2" 
                                CornerRadius="10">
                            <ContentPresenter HorizontalAlignment="Center" VerticalAlignment="Center"/>
                        </Border>
                    </ControlTemplate>
                </Setter.Value>
            </Setter>
        </Style>
    </Window.Resources>

    <Grid>
        <Border CornerRadius="20" Background="#FFE7F2" BorderBrush="#D895C5" BorderThickness="2" Padding="30" Margin="20">
            <StackPanel >
                
                <TextBlock Text="👩 Owner Info" FontSize="26" FontWeight="Bold" Foreground="#D36DA8" 
                   TextAlignment="Center" Margin="0,0,0,20"/>


                <StackPanel Orientation="Horizontal" Margin="0,0,0,10">
                    <Label Content="💌 Owner's Name:" VerticalAlignment="Center"/>
                    <TextBox x:Name="Otbox" Text="{Binding Owner.Name, UpdateSourceTrigger=PropertyChanged}" Height="50" Width="240"/>
                </StackPanel>
                <Button Content="💾 Save" Click="SaveButton_Click" Height="50" Width="120"
                HorizontalAlignment="Center" Background="#FAD0E9" BorderBrush="#D895C5" BorderThickness="2"
                FontSize="18" />
            </StackPanel>
        </Border>

    </Grid>
</Window>
```

The xaml itself is not enough, so we need a  MainWindow.xaml.cs 
```csharp
 public partial class MainWindow : Window
 {
   // It need its own ViewModel to have access to the properties.
     private ViewModel vm;
     public MainWindow()
     {
         InitializeComponent();
        // The datacontext is setted here
         if (this.DataContext is ViewModel vm)
         {
             this.vm = vm;
         }
         else
         {
             this.vm = new ViewModel();
             this.DataContext = this.vm;
         }
        
        
         
     }

     private void SaveButton_Click(object sender, RoutedEventArgs e)
     {
      // The `SaveClick` is explaind here. Its bindig the setted name then update its value.
         this.Otbox
             .GetBindingExpression(TextBox.TextProperty)
             .UpdateSource();
        
       //Here the ViewModel is given to the next window, than open it.
         
         EditWindow editWindow = new EditWindow(vm)
         { 
             DataContext = vm
         };
         editWindow.Show();
         this.Close();
     }
 }
```

---

### 4. EditWindow\.xaml

This is the customization screen:

* Dropdowns/sliders for species, fur, and color.
* Image updates based on selected traits.
* `Passport` button is lead us to the `PassportWindow`.
* `View All Pets` button is only enabled when pet count >= 2.
*  `New Owner` button  takes us back to the `MainWindow`.

```xaml
<!--- It gets the same Datacontext as the previous window--->
    <Window.DataContext>
        <local:ViewModel />
    </Window.DataContext>

    <Window.Resources>
        <local:BrushColorConverter x:Key="BrushConverter" />
<!--- Since the Pet's properties change here, inclueding the fur,species, and color.
 Which are Enum types, so its needs some help with the casting.
 `EnumHelper` is a utility class (static class with one purpose)
with a methode that takes an Enum and returns its values as an Array. --->
        <ObjectDataProvider x:Key="SpeciesValues" MethodName="GetValues" ObjectType="{x:Type local:EnumHelper}">
            <ObjectDataProvider.MethodParameters>
                <x:Type TypeName="local:Species"/>
            </ObjectDataProvider.MethodParameters>
        </ObjectDataProvider>
        <ObjectDataProvider x:Key="FurValues" MethodName="GetValues" ObjectType="{x:Type local:EnumHelper}">
            <ObjectDataProvider.MethodParameters>
                <x:Type TypeName="local:fur"/>
            </ObjectDataProvider.MethodParameters>
        </ObjectDataProvider>
        <ObjectDataProvider x:Key="ColorValues" MethodName="GetValues" ObjectType="{x:Type local:EnumHelper}">
            <ObjectDataProvider.MethodParameters>
                <x:Type TypeName="local:color"/>
            </ObjectDataProvider.MethodParameters>
        </ObjectDataProvider>
<!---
The same style settings is need to be done the same way as in the `MainWindow`.
--->
        <Style TargetType="Button">
            <Setter Property="Background" Value="#D895C5"/>
            <Setter Property="FontSize" Value="16"/>
            <Setter Property="Margin" Value="5"/>
            <Setter Property="Padding" Value="5"/>
            <Setter Property="BorderBrush" Value="#D895C5"/>
            <Setter Property="BorderThickness" Value="2"/>
            <Setter Property="Template" >
                <Setter.Value>
                    <ControlTemplate TargetType="Button">
                        <Grid Width="{TemplateBinding Width}" Height="{TemplateBinding Height}" ClipToBounds="True">

                            <Rectangle x:Name="innerRectangle" HorizontalAlignment="Stretch" VerticalAlignment="Stretch" Stroke="Transparent" StrokeThickness="20" Fill="{TemplateBinding Background}" RadiusX="10" RadiusY="10" />

                            <ContentPresenter HorizontalAlignment="Center"
                                  VerticalAlignment="Center"
                                  RecognizesAccessKey="True"
                                  Content="{TemplateBinding Content}"
                                  ContentTemplate="{TemplateBinding ContentTemplate}"
                                  ContentStringFormat="{TemplateBinding ContentStringFormat}"
                                  Margin="{TemplateBinding Padding}"
                                  />


                        </Grid>
                    </ControlTemplate>
                </Setter.Value>
            </Setter>

        </Style>
    </Window.Resources>

    <Grid Background="#FFE7F2">
        <Grid.ColumnDefinitions>
            <ColumnDefinition Width="5*" />

            <ColumnDefinition Width="3*"/>

        </Grid.ColumnDefinitions>
        <Grid.RowDefinitions>
            <RowDefinition Height="6*" />
            <RowDefinition Height="2*" />
        </Grid.RowDefinitions>

        <!-- Left Column: Labels, Here the owner is able to change the values of the pet (name, species, fur, color,age).-->
        <Border Grid.Column="0" Grid.Row="0" Margin="30,30,300,5" Padding="10" BorderBrush="#D895C5" BorderThickness="2" Background="#FFE7F2" CornerRadius="10" Grid.ColumnSpan="2">

            <StackPanel Grid.Column="0" Margin="0,0,0,20" Grid.ColumnSpan="2">

                <TextBlock Text="✨ Customize your pet ✨" FontSize="21" FontWeight="Bold" Foreground="#D895C5" Margin="0,0,0,20"/>
                <StackPanel Orientation="Horizontal" Margin="0,5,0,0">
                    <Label Content="Pet's Name:" FontWeight="Bold"  Foreground="PaleVioletRed" Margin="20,0,10,5"  FontSize="20" VerticalAlignment="Center"/>
                    <TextBox x:Name="Ptbox" Text="{Binding CurrentPet.Name, UpdateSourceTrigger=PropertyChanged}" FontSize="20" Height="40" Width="240"/>
                </StackPanel>
                <StackPanel Orientation="Horizontal" Margin="0,5,0,0">
                    <TextBlock Text="Species:" FontSize="20" FontWeight="Bold"  Foreground="PaleVioletRed" Margin="20,0,10,5" HorizontalAlignment="Right"/>
                    <ComboBox Name="speciesbox" Width="150" FontSize="16"  Margin="40,0,0,0"
                     Background="#FFE7F2"
                      ItemsSource="{Binding Source={StaticResource SpeciesValues}}"
                      SelectedItem="{Binding CurrentPet.Species, Mode=TwoWay}" />
                </StackPanel>
                <StackPanel Orientation="Horizontal" Margin="0,5,0,0">
                    <TextBlock Text="Fur Type:" FontSize="20" FontWeight="Bold" Foreground="PaleVioletRed" Margin="20,0,10,5" HorizontalAlignment="Right"/>
                   <!--- The Combobox is unable if the `species` is setted to Goldfish,
                   because of the `PetHasFur` that we setted in the `ViewModel` class. --->
                    <ComboBox IsEnabled="{Binding PetHasFur}"  x:Name="furbox" Width="150" FontSize="16" Margin="40,0,0,0" Background="#FFE7F2"
                      ItemsSource="{Binding Source={StaticResource FurValues}}"
                      SelectedItem="{Binding CurrentPet.Fur, Mode=TwoWay}" />
                </StackPanel>

                <StackPanel Orientation="Horizontal" Margin="0,5,0,0">
                    <TextBlock Text="Color:" FontSize="20" FontWeight="Bold" Foreground="PaleVioletRed" Margin="20,0,10,5" HorizontalAlignment="Right"/>
                     <!--- Similar thing is happening here as with the previous Combobox.
                       This time with the `color` and the `PetHasColor`. --->
                    <ComboBox IsEnabled="{Binding PetHasColor}" x:Name="colorbox" Width="150" FontSize="16"  Background="#FFE7F2" Margin="40,0,0,0"
                      ItemsSource="{Binding Source={StaticResource ColorValues}}"
                      SelectedItem="{Binding CurrentPet.Color, Mode=TwoWay}" />
                </StackPanel>
                <StackPanel Orientation="Horizontal" Margin="0,5,0,0">
                    <TextBlock Text="Age:" FontSize="20" FontWeight="Bold" Foreground="PaleVioletRed" Margin="20,0,0,20" HorizontalAlignment="Right"/>
                    <Button Content="▲" Width="50" Height="50" Click="IncreaseAge_Click"/>
                    <TextBlock Text="{Binding CurrentPet.Age}" FontSize="18" Margin="0,20,10,10"/>
                    <Button Content="▼" Width="50"  Height="50" Click="DecreaseAge_Click"/>

                </StackPanel>
            </StackPanel>
        </Border>
        
        <!-- Right Column: Summary -->
        <Border Grid.Column="1" Grid.Row="0" Margin="30,30,23,5" Padding="10" BorderBrush="#D895C5" BorderThickness="2" Background="#FFE7F2" CornerRadius="10">

            <StackPanel>
                <TextBlock Text="✨ Pet Passport ✨" FontSize="24" FontWeight="Bold" Foreground="#D895C5" Margin="0,0,0,20" HorizontalAlignment="Center"/>

                <StackPanel Orientation="Horizontal" Margin="0,5">
                    <TextBlock Text="🐾 Name: " FontSize="18" Foreground="PaleVioletRed" FontWeight="SemiBold"/>
                    <TextBlock Text="{Binding CurrentPet.Name}" Foreground="PaleVioletRed" FontSize="18"/>
                </StackPanel>




                <StackPanel Orientation="Horizontal" Margin="0">
                    <TextBlock Text="👩 Owner: " FontSize="18" Foreground="PaleVioletRed" FontWeight="SemiBold"/>
                    <TextBlock Text="{Binding Owner.Name}" Foreground="PaleVioletRed" FontSize="18"/>
                </StackPanel>

                <StackPanel Orientation="Horizontal" Margin="0">
                    <TextBlock Text="🦄 Species: " FontSize="18" Foreground="PaleVioletRed" FontWeight="SemiBold"/>
                    <TextBlock Text="{Binding CurrentPet.Species}" Foreground="PaleVioletRed" FontSize="18"/>
                </StackPanel>

                <StackPanel Orientation="Horizontal" Margin="0">
                    <TextBlock Text="🌈 Color: " FontSize="18" Foreground="PaleVioletRed" FontWeight="SemiBold"/>
                    <TextBlock Text="{Binding CurrentPet.Color}" Foreground="PaleVioletRed" FontSize="18"/>
                </StackPanel>
                <StackPanel Orientation="Horizontal" Margin="0">
                    <TextBlock Text="🎂 Age: " FontSize="18" Foreground="PaleVioletRed" FontWeight="SemiBold"/>
                    <TextBlock Text="{Binding CurrentPet.Age}" Foreground="PaleVioletRed" FontSize="18"/>
                </StackPanel>
            </StackPanel>
        </Border>

        <StackPanel Grid.Row="1" Grid.Column="0"  Orientation="Horizontal" >

            <Button Content="New Owner"  FontSize ="16"  Height="60" Width="200" Click="NewPetClick" Margin="20,20,10,5" />


            <Button Content="Passport" Height="60" Width="200" Click="SavePetClick" Margin="70,20,10,5" />
        </StackPanel>
        <StackPanel Grid.Row="1" Grid.Column="1" Margin="0,30,0,5">
            <Button Content="View All Pets" Height="60" Width="200"
            IsEnabled="{Binding CanViewAllPets}" 
            Click="ViewAllPets_Click"/>
        </StackPanel>

    </Grid>
</Window>
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



---



