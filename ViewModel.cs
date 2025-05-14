using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;


namespace PetGuiProject
{
    
    public class ViewModel : INotifyPropertyChanged
    {
        public IRelayCommand AddToOwnersPet { get; set; }
        public event PropertyChangedEventHandler? PropertyChanged;


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


        private Pet currentPet;
        public Pet CurrentPet
        {
            get => currentPet;
            set { currentPet = value; OnPropertyChanged(); }
        }
        public Owner Owner { get; set; }
        public bool PetIsReady { get; set;  }
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

        public ViewModel()
        {
            CurrentPet = new Pet();
            Owner = new Owner();
            CanViewAllPets = false;

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
                   if (Owner.Pet.Count >= 2)
                   {
                       CanViewAllPets = true;
                   }
                   MessageBox.Show($" {petCopy.Name} added to Owner's list");
                
               },
               () => CurrentPet != null
              
           );
            
            

        }
      /*  public void AddCurrentPetToOwner()
        {
            if (CurrentPet != null && !Owner.Pet.Contains(CurrentPet))
            {
                Owner.Pet.Add(CurrentPet);
                CanViewAllPets = Owner.Pet.Count >= 2;
            }
        }*/


        private void UpdatePetFlags()
        {
            PetHasFur = CurrentPet.Species != Species.goldfish;
            PetHasColor = CurrentPet.Species != Species.chick;
        }

        protected void OnPropertyChanged([CallerMemberName] string name = "")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
       
        public void update()
        {

            PetIsReady = true;
            MessageBox.Show("Pet updated!");
            OnPropertyChanged();

            PetIsReady = false;

        }
        
    }
    }
