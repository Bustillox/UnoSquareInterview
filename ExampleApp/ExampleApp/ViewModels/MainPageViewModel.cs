using ExampleApp.Models;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Acr.UserDialogs;
using Xamarin.Forms;

namespace ExampleApp.ViewModels
{
    class MainPageViewModel : ViewModelBase
    {
        #region Props

        private string _brand;
        public string Brand
        {
            get { return _brand; }
            set { SetProperty(ref _brand, value); }
        }

        private string _model;
        public string Model
        {
            get { return _model; }
            set { SetProperty(ref _model, value); }
        }

        private string _year;
        public string Year
        {
            get { return _year; }
            set { SetProperty(ref _year, value); }
        }

        private List<Vehicle> _vehicleList = new List<Vehicle>();
        public List<Vehicle> VehicleList
        {
            get => _vehicleList; 
            set => SetProperty(ref _vehicleList, value); 
        }
        #endregion

        #region Commands
        public DelegateCommand AddVehicleCommand { get; set; }
        #endregion

        public MainPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            Title = "My Vehicles";
            AddVehicleCommand = new DelegateCommand(AddVehicle);

            LoadVehicleList();
        }

        void AddVehicle()
        {
            if (string.IsNullOrEmpty(Brand) || string.IsNullOrEmpty(Model) || string.IsNullOrEmpty(Year))
            {
                UserDialogs.Instance.AlertAsync("Please fill all the values before continuing");
                return;
            }
            
            if (VehicleList.Any())
            {
                foreach (var item in VehicleList)
                {
                    if (item.Brand == Brand && item.Model == Model && item.Year == Year)
                    {
                        UserDialogs.Instance.AlertAsync("This element is already on the list");
                        return;
                    }
                    else
                        VehicleList.Add( new Vehicle { Brand = Brand, Model = Model, Year = Year });
                }
            }
            else
            {
                VehicleList.Add(new Vehicle { Brand = Brand, Model = Model, Year = Year });
                Application.Current.Properties["MyVehicles"] = VehicleList;
                Application.Current.SavePropertiesAsync();
            }
            
        }

        void LoadVehicleList()
        {
            //VehicleList = (List<Vehicle>)Application.Current?.Properties["MyVehicles"] ?? new List<Vehicle>();
        }
    }
}
