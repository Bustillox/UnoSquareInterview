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
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using ExampleApp.Interfaces;

namespace ExampleApp.ViewModels
{
    class MainPageViewModel : ViewModelBase
    {
        #region Props

        public string _version;
        public string Version
        {
            get { return _version; }
            set { SetProperty(ref _version, value); }
        }

        public string _brand;
        public string VehicleBrand
        {
            get { return _brand; }
            set { SetProperty(ref _brand, value); }
        }


        public string _model;
        public string VehicleModel
        {
            get { return _model; }
            set { SetProperty(ref _model, value); }
        }

        public string _year;
        public string VehicleYear
        {
            get { return _year; }
            set { SetProperty(ref _year, value); }
        }

        ObservableCollection<Vehicle> _vehicleList = new ObservableCollection<Vehicle>();
        public ObservableCollection<Vehicle> VehicleList
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
            AddVehicleCommand = new DelegateCommand(async () => await AddVehicle());
            LoadVehicleList();

            string v = DependencyService.Get<IAppVersion>().GetVersion();
            Version = v;
        }

        async Task AddVehicle()
        {
            if (string.IsNullOrEmpty(VehicleBrand) || string.IsNullOrEmpty(VehicleModel) || string.IsNullOrEmpty(VehicleYear))
            {
                await UserDialogs.Instance.AlertAsync("Please fill all the values before continuing");
                return;
            }
            
            if (VehicleList.Any())
            {
                foreach (var item in VehicleList)
                {
                    if (item.Brand == VehicleBrand && item.Model == VehicleModel && item.Year == VehicleYear)
                    {
                        await UserDialogs.Instance.AlertAsync("This element is already on the list");
                        return;
                    }
                }
                VehicleList.Add( new Vehicle { Brand = VehicleBrand, Model = VehicleModel, Year = VehicleYear });
            }
            else
            {
                VehicleList.Add(new Vehicle { Brand = VehicleBrand, Model = VehicleModel, Year = VehicleYear });
                Application.Current.Properties.Add("MyVehicles", VehicleList);
                await Application.Current.SavePropertiesAsync();
            }
        }

        void LoadVehicleList()
        {
            if (Application.Current.Properties.ContainsKey("MyVehicles"))
            {
                VehicleList = (ObservableCollection<Vehicle>)Application.Current?.Properties["MyVehicles"] ?? new ObservableCollection<Vehicle>();
            }
            else
            {
                VehicleList = new ObservableCollection<Vehicle>();
            }
        }
    }
}
