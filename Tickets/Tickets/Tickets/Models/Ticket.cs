namespace Tickets.Models
{
    public class Ticket
    {
        #region Propiedades
        public int TicketId { get; set; }
        public string TicketCode { get; set; }
        public string DateTime { get; set; }
        public int UserId { get; set; }
        #endregion
    }
}
