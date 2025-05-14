using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PetGuiProject
{
    /// <summary>
    /// Interaction logic for OwnerPetsWindow.xaml
    /// </summary>
    public partial class OwnerPetsWindow : Window
    {
        ViewModel vm;
        public OwnerPetsWindow(ViewModel wm)
        {
            InitializeComponent();
            vm = wm;
        }

        private void ShowThePetClick(object sender, MouseButtonEventArgs e)
        {

            if (sender is ListBox listBox && listBox.SelectedItem is Pet selectedPet)
            {
                // Set selected pet as current in the shared ViewModel
                vm.CurrentPet = selectedPet;
                vm.Owner.Pet.Remove(selectedPet);
                this.Close(); 
            }


        }
    }
}
