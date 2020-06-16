using System.Collections;
using System.Collections.Generic;

namespace APDB_Project.Domain
{
    public class Client
    {
        public int IdClient { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public virtual ICollection<Campaign> Campaigns { get; set; }
    }
}