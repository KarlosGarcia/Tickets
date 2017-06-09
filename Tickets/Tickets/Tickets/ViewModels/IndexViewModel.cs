using GalaSoft.MvvmLight.Command;
using System.ComponentModel;
using System.Windows.Input;
using Tickets.Models;
using Tickets.Services;

namespace Tickets.ViewModels
{
    public class IndexViewModel : User, INotifyPropertyChanged
    {

        #region Atributos
        private NavigationService navigationService;
        private DialogService dialogService;
        private ApiService apiService;
        private bool isRunning;
        private bool isEnabled;
        #endregion

        #region Propiedades
        public bool IsRunning
        {
            set
            {
                if (isRunning != value)
                {
                    isRunning = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsRunning"));
                }
            }
            get
            {
                return isRunning;
            }
        }

        public bool IsEnabled
        {
            set
            {
                if (isEnabled != value)
                {
                    isEnabled = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsEnabled"));
                }
            }
            get
            {
                return isEnabled;
            }
        }
        #endregion

        #region Eventos
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion
        
        #region Contructor
        public IndexViewModel()
        {
            navigationService = new NavigationService();
            dialogService = new DialogService();
            apiService = new ApiService();
            IsEnabled = true;
            Email = "carlosandresgr@gmail.com";
            Password = "123456";
        }
        #endregion

        #region Commands
        public ICommand LoginCommand { get { return new RelayCommand(LoginUser); } }

        private async void LoginUser()
        {
            if (string.IsNullOrEmpty(Email))
            {
                await dialogService.ShowMessage("Importante", "Ingresa Tu Email!");
                return;
            }

            if (string.IsNullOrEmpty(Password))
            {
                await dialogService.ShowMessage("Importante", "Ingresa Tu Contraseña!");
                return;
            }

            // Realizamos el llamado al Servicio REST para Login
            IsRunning = true;
            IsEnabled = false;
            var response = await apiService.Login("http://checkticketsback.azurewebsites.net", "/api", "/Users/Login", this);
            IsRunning = false;
            IsEnabled = true;

            if (!response.IsSuccess)
            {
                await dialogService.ShowMessage("Importante", response.Message);
                return;
            }
            User dataUser = (User)response.Result;
            var mainViewModel = MainViewModel.GetInstance();
            mainViewModel.Validate = new ValidateViewModel(dataUser);
            await navigationService.Navigate("ValidationPage");
        }
        #endregion
    }
}
