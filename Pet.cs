using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace PetGuiProject
{
    public enum Species
    { 
        bunny,
        guineapig,
        goldfish,
        chick





    }
  public  enum fur
    { 
        basic,
        extrapuffy,
        spotted
        

    }
    public enum color
    {
        white,
        orange,
        black,
        brown


    }
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
/*
        public void UpdateImagePath()
        {
            if (Species == Species.bunny)
            {
                if (Fur == fur.basic)
                {

                    if (Color == color.black)
                    {
                        ImagePath = "Pets\\Bunny\\Basic\\Black.png";
                        
                    }
                    else if (Color == color.brown)
                    { ImagePath = "Pets\\Bunny\\Basic\\Brown.png"; }
                    else if (Color == color.orange)
                    { ImagePath = "Pets\\Bunny\\Basic\\Orange"; }
                    else if (Color == color.white)
                    { ImagePath = "Pets\\Bunny\\Basic\\White"; }


                    }
                    else if (Fur == fur.extrapuffy)
                    {
                        if (Color == color.black)
                        {
                            ImagePath = "Pets\\Bunny\\Fluffy\\Black";
                        }
                        else if (Color == color.brown)
                        { ImagePath = "Pets\\Bunny\\Fluffy\\Brown"; }
                        else if (Color == color.orange)
                        { ImagePath = "Pets\\Bunny\\Fluffy\\Orange"; }
                        else if (Color == color.white)
                        { ImagePath = "Pets\\Bunny\\Fluffy\\White"; }
                    }
                    else if (Fur == fur.spotted)
                    {
                        if (Color == color.black)
                        {
                            ImagePath = "Pets\\Bunny\\Spotted\\Black";
                        }
                        else if (Color == color.brown)
                        { ImagePath = "Pets\\Bunny\\Spotted\\Brown"; }
                        else if (Color == color.orange)
                        { ImagePath = "Pets\\Bunny\\Spotted\\Orange"; }
                        else if (Color == color.white)
                        { ImagePath = "Pets\\Bunny\\Spotted\\White"; }
                    }
                }
                else if (Species == Species.chick)
                {
                    if (Fur == fur.basic)
                    { ImagePath = "Pets\\Chicken\\Basic"; }
                    else if (Fur == fur.extrapuffy)
                    { ImagePath = "Pets\\Chicken\\Fluffy"; }
                    else if (Fur == fur.spotted)
                    { ImagePath = "Pets\\Chicken\\Spotted"; }

                }
                else if (Species == Species.guineapig)
                {
                    if (Fur == fur.basic)
                    {
                        if (Color == color.black)
                        { }
                        else if (Color == color.brown)
                        { }
                        else if (Color == color.orange)
                        { }
                        else if (Color == color.white)
                        { }
                    }
                    if (Fur == fur.extrapuffy)
                    {
                        if (Color == color.black)
                        { }
                        else if (Color == color.brown)
                        { }
                        else if (Color == color.orange)
                        { }
                        else if (Color == color.white)
                        { }
                    }
                    if (Fur == fur.spotted)
                    {
                        if (Color == color.black)
                        { }
                        else if (Color == color.brown)
                        { }
                        else if (Color == color.orange)
                        { }
                        else if (Color == color.white)
                        { }
                    }

                }
                else if (Species == Species.goldfish)
                {
                    if (Color == color.black)
                    { ImagePath = "Pets\\Goldfish\\Black"; }
                    else if (Color == color.brown)
                    { ImagePath = "Pets\\Goldfish\\Brown"; }
                    else if (Color == color.orange)
                    { ImagePath = "Pets\\Goldfish\\Orange"; }
                    else if (Color == color.white)
                    { ImagePath = "Pets\\Goldfish\\White"; }
                }

                OnPropertyChanged();
                OnPropertyChanged(nameof(ImagePath));

            }
        */

        private void OnPropertyChanged([CallerMemberName] string propetyName = "")
        {
            this.PropertyChanged?
                .Invoke(this, new PropertyChangedEventArgs(propetyName));
        }
        public event PropertyChangedEventHandler? PropertyChanged;
     
        
    }
}
