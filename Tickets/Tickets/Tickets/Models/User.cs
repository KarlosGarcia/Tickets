namespace Tickets.Models
{
    public class User
    {
        #region Atributos
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string Password { get; set; }
        #endregion
    }
}
