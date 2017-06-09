namespace Tickets.Models
{
    public class Ticket : User
    {
        #region Propiedades
        public int TicketId { get; set; }
        public string TicketCode { get; set; }
        public string DateTime { get; set; }
        #endregion
    }
}
