using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Threading.Tasks;

using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PetGuiProject
{
    public static class EnumHelper
    {
        public static Array GetValues(Type enumType) => Enum.GetValues(enumType);
    }
    public partial  class EditWindow: Window
    {
        private ViewModel vm;
        public EditWindow(ViewModel model)
        {
            InitializeComponent();

            this.vm = model;
            

        }
        

        private void NewSpeciesClick(object sender, MouseButtonEventArgs e)
        {
            this.speciesbox.GetBindingExpression(ComboBoxItem.ContentProperty)
                .UpdateSource(); vm.CurrentPet.UpdateImagePath();
            

            
        }

        private void NewFurClick(object sender, MouseButtonEventArgs e)
        {
            this.furbox.GetBindingExpression(ComboBoxItem.ContentProperty)
                .UpdateSource(); vm.CurrentPet.UpdateImagePath();
        }

        private void NewColorClick(object sender, MouseButtonEventArgs e)
        {
            this.colorbox.GetBindingExpression(ComboBoxItem.ContentProperty)
                 .UpdateSource(); vm.CurrentPet.UpdateImagePath();
        }
       


        private void UpDateClick(object sender, RoutedEventArgs e)
        {

            
            vm.update();

           
        }

        private void IncreaseAge_Click(object sender, RoutedEventArgs e)
        {
            if (vm?.CurrentPet != null)
                vm.CurrentPet.Age++;
            vm.CurrentPet.UpdateImagePath();

        }

        private void DecreaseAge_Click(object sender, RoutedEventArgs e)
        {
            if (vm?.CurrentPet != null && vm.CurrentPet.Age > 0)
                vm.CurrentPet.Age--;
            vm.CurrentPet.UpdateImagePath();
        }

        private void SavePetClick(object sender, RoutedEventArgs e)
        {
            this.Ptbox
             .GetBindingExpression(TextBox.TextProperty)
             .UpdateSource();
            vm.PetIsReady = true;
            vm.CurrentPet.UpdateImagePath();

            PassportWindow page = new PassportWindow(vm)
            {
                DataContext = vm
            };
            page.Show();
            // order !!!
        }

        private void NewPetClick(object sender, RoutedEventArgs e)
        {
            if (vm.PetIsReady)
            {
                MainWindow mw = new MainWindow();
                mw.Show();
                this.Close();


            }
            else
            {
               var result =  MessageBox.Show("Want to save the pet?","From closing",MessageBoxButton.YesNo);

                if (result == MessageBoxResult.Yes)
                {
                    vm.PetIsReady = true;
                    PassportWindow page = new PassportWindow(vm)
                    {
                        DataContext = vm
                    };
                    page.Show();


                }
                else
                {

                    MainWindow mw = new MainWindow();
                    mw.Show();
                    this.Close();
                }
                // ha igen --> SavePetClick
                // kulonben --> mainwindow 
            }
            
        }

        private void ViewAllPets_Click(object sender, RoutedEventArgs e)
        {
            OwnerPetsWindow view = new OwnerPetsWindow(vm);
            view.DataContext = vm;
            view.Show();
        }
    }
}
