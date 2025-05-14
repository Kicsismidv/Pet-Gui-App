using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PetGuiProject
{
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
}
