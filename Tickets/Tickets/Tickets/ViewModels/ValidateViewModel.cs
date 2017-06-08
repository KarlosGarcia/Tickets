using GalaSoft.MvvmLight.Command;
using System.ComponentModel;
using System.Windows.Input;
using Tickets.Models;
using Tickets.Services;

namespace Tickets.ViewModels
{
    public class ValidateViewModel : User, INotifyPropertyChanged
    {
        #region Atributos
        private DialogService dialogService;
        private ApiService apiService;
        private NavigationService navigationService;
        private bool isRunning;
        private bool isEnabled;
        #endregion

        #region Propiedades

        public string TicketCode { get; set; }

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

        #region Constructor
        public ValidateViewModel(User user)
        {
            UserId = user.UserId;
            FirstName = user.FirstName;
            LastName = user.LastName;
            Email = user.Email;
            FullName = string.Format("{0} {1}", FirstName, LastName);
            navigationService = new NavigationService();
            dialogService = new DialogService();
            apiService = new ApiService();
            IsEnabled = true;
        }
        #endregion

        #region Commands
        public ICommand ValidateCommand { get { return new RelayCommand(ValidateTicket); } }

        private async void ValidateTicket()
        {
            if (string.IsNullOrEmpty(TicketCode))
            {
                await dialogService.ShowMessage("Importante", "Ingresa Tu Codigo De Entrada!");
                return;
            }

            // Realizamos el llamado al Servicio REST para Validar la Entrada
            IsRunning = true;
            IsEnabled = false;
            string controlador = string.Format("{0}{1}","/Tickets/",TicketCode);
            var response = await apiService.GetTicket<Ticket>("http://checkticketsback.azurewebsites.net", "/api", controlador);
            Ticket dataTicket = (Ticket)response.Result;
            if (!response.IsSuccess)
            {
                var answer = await dialogService.ShowConfirm("Importante", response.Message);
                if (!answer)
                {
                    IsRunning = false;
                    IsEnabled = true;
                    return;
                }
                // Realizamos el llamado al Servicio REST para Registrar La Entrada
                controlador = string.Format("{0}", "/Tickets");
                Ticket ticket = new Ticket();
                ticket.TicketCode = TicketCode;
                ticket.DateTime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                ticket.UserId = this.UserId;
                response = await apiService.AddTicket("http://checkticketsback.azurewebsites.net", "/api", controlador, ticket);
                if (!response.IsSuccess)
                {
                    await dialogService.ShowMessage("Importante", "Lo Siento, Entrada No Registrada!");
                    IsRunning = false;
                    IsEnabled = true;
                    return;
                }
                await dialogService.ShowMessage("Importante", "Felicidades, Entrada Registrada Exitosamente!");
                IsRunning = false;
                IsEnabled = true;
                TicketCode = "";
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("TicketCode"));
                return;
            }
            if (!string.IsNullOrEmpty(dataTicket.TicketCode))
            {
                await dialogService.ShowMessage("Importante", "Tu Entrada Ya Se Encuentra Registrada!");
                IsRunning = false;
                IsEnabled = true;
                return;
            }
            IsRunning = false;
            IsEnabled = true;
        }
        #endregion
    }
}
