using GalaSoft.MvvmLight.Command;
using System.ComponentModel;
using System.Windows.Input;
using Tickets.Models;
using Tickets.Services;

namespace Tickets.ViewModels
{
    public class MainViewModel
    {

        #region Atributos
        public IndexViewModel Login { get; set; }
        public ValidateViewModel Validate { get; set; }
        #endregion

        #region Propiedades
        #endregion

        #region Contructor
        public MainViewModel()
        {
            instance = this;
            Login = new IndexViewModel();
        }
        #endregion

        #region Singleton
        private static MainViewModel instance;

        public static MainViewModel GetInstance()
        {
            if (instance == null)
            {
                instance = new MainViewModel();
            }

            return instance;
        }
        #endregion
    }
}
