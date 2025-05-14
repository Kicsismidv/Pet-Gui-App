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
    /// Interaction logic for PassportWindow.xaml
    /// </summary>
    public partial class PassportWindow : Window
    {
        ViewModel vm;
        public PassportWindow(ViewModel vm)
        {
            InitializeComponent();
            this.vm = vm;
        }

        private void ExitClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
