using GalaSoft.MvvmLight.Command;
using System.ComponentModel;
using System.Windows.Input;
using Tickets.Models;
using Tickets.Services;
using Xamarin.Forms;

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
        private string statusTicket;
        private string ticketCode;
        private Color statusColor;
        #endregion

        #region Propiedades

        public Color StatusColor
        {
            set
            {
                if (statusColor != value)
                {
                    statusColor = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("StatusColor"));
                }
            }
            get
            {
                return statusColor;
            }
        }

        public string StatusTicket
        {
            set
            {
                if (statusTicket != value)
                {
                    statusTicket = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("StatusTicket"));
                }
            }
            get
            {
                return statusTicket;
            }
        }

        public string TicketCode
        {
            set
            {
                if (ticketCode != value)
                {
                    ticketCode = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("TicketCode"));
                }
            }
            get
            {
                return ticketCode;
            }
        }

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
                await dialogService.ShowMessage("Importante", "Ingresa Tu Codigo De Boleto!");
                StatusTicket = "";
                return;
            }

            // Realizamos el llamado al Servicio REST para Validar la Entrada
            IsRunning = true;
            IsEnabled = false;
            string controlador = string.Format("{0}{1}","/Tickets/",TicketCode);
            var response = await apiService.GetTicket<Ticket>("http://checkticketsback.azurewebsites.net", "/api", controlador);
            if (!response.IsSuccess)
            {
                // Realizamos el llamado al Servicio REST para Registrar La Entrada
                controlador = string.Format("{0}", "/Tickets");
                Ticket ticket = new Ticket();
                ticket.TicketCode = TicketCode;
                ticket.DateTime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                ticket.UserId = this.UserId;
                response = await apiService.AddTicket("http://checkticketsback.azurewebsites.net", "/api", controlador, ticket);
                // Si Lo Pudo Crear
                Ticket dataTicket = (Ticket)response.Result;
                if (response.IsSuccess == false)
                {
                    IsRunning = false;
                    IsEnabled = true;
                    StatusTicket = response.Message;
                    StatusTicket = StatusTicket.Replace("@@", TicketCode);
                    TicketCode = "";
                    StatusColor = Color.Red;
                    return;
                }
                else
                {
                    IsRunning = false;
                    IsEnabled = true;
                    StatusTicket = response.Message;
                    StatusTicket = StatusTicket.Replace("@@", TicketCode);
                    TicketCode = "";
                    StatusColor = Color.Green;
                }
            }
            else
            {
                IsRunning = false;
                IsEnabled = true;
                StatusTicket = response.Message;
                StatusTicket = StatusTicket.Replace("@@", TicketCode);
                TicketCode = "";
                StatusColor = Color.Red;
                return;
            }
        }
        #endregion
    }
}
