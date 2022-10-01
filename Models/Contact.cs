using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdressBook.Models
{
    internal class Contact // Varje kontakt blir en ny instans av denna klass
    {
        public Contact()  // I konstruktorn skapas automariskt ett unikt id för varje kontakt, detta ska inte skapas av användaren 
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; } // Id har ingen set. Det kan inte ändras
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
    }
}
