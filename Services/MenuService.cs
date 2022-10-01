using AdressBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AdressBook.Services
{
    internal interface IMenuService //  Alla klassens metoder finns inte med i interfacet och kan då inte kallas från en instans av klassen som görs med interfacet
    {
        public void MainMenu();
        public void CreateContact();
        public void ShowContacts();
        public void SearchContact();
        
    }

    internal class MenuService : IMenuService
    {
        private IFileService _fileService;
        private static List<Contact> _contacts = new List<Contact>(); // En tom lista av contacts instansieras

        public MenuService(string filePath) // I konstruktorn instansieras FileService och innehållet i filen läggs in i _contacts.
        {                                   // Om filen inte finns fortsätter _contacts vara tom
            _fileService = new FileService(filePath);
            try
            {
                _contacts = _fileService.Read();
            }
            catch
            {}
        }

        public void MainMenu() // Huvudmenyn körs i en loop i Program.cs. Den tar emot användarens val och kallar olika metoder
        {
            Console.Clear();

            Console.WriteLine("---  Contact List  ---");
            Console.WriteLine("1. Show all contacts");
            Console.WriteLine("2. Search for a contact");
            Console.WriteLine("3. Create new contact");
            Console.WriteLine("4. Quit");


            var option = Console.ReadLine(); 

            switch (option)
            {
                case "1":
                    ShowContacts();
                    break;
                case "2":
                    SearchContact();
                    break;
                case "3":
                    CreateContact();
                    break;
                case "4":
                    Console.Clear();
                    Console.WriteLine("BYE");
                    Console.ReadKey();
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Invalid option. Choose again.");
                    break;
            }
        }

        public void CreateContact() // En ny instans av Contact görs. Därefter matar användaren in kontaktuppgifterna.
        {                           // Kontakten läggs till listan och sparas till filen.
            Contact contact = new Contact();

            Console.Clear();
            Console.WriteLine("Enter contact information:");

            Console.Write("First name: ");
            contact.FirstName = Console.ReadLine().Trim();
            Console.Write("Last name: ");
            contact.LastName = Console.ReadLine().Trim();
            Console.Write("Phone number: ");
            contact.Phone = Console.ReadLine().Trim();
            Console.Write("E-mail: ");
            contact.Email = Console.ReadLine().Trim();
            _contacts.Add(contact);
            _fileService.Save(_contacts);
            Console.WriteLine("Contact added.");            
            Console.ReadKey();            
        }

        public  void ShowContacts() // Alla kontakter visas upp
        {
            Console.Clear();

            if(_contacts.Count() != 0) // Om kontaktlistan inte är tom...
            { 
                foreach (Contact contact in _contacts) // ...visas varje kontakt upp..
                {
                    Console.WriteLine($"{_contacts.IndexOf(contact) + 1}. {contact.FirstName} {contact.LastName}"); 
                    // ...med index, förnamn och efternamn. Index visas +1, det skulle se konstigt ut om den första var nummer 0.
                }
                Console.Write("Enter number to see details or M to go back to Main Menu: ");

                var show = Console.ReadLine();

                if(show.ToLower() == "m") // Om anv. väljer M eller m körs MainMenu igen.
                {
                    MainMenu();
                }
                else 
                {
                    Console.Clear();
                    try
                    {
                        ShowContact(Int32.Parse(show) - 1); // ShowContact körs för den kontakt anv. har valt, -1 eftersom det visades +1 i listan.
                    }
                    catch
                    {
                        Console.WriteLine("Invalid choice."); // Om anv. väjer något annat än M eller en befintlig kontakt visas detta.
                        Console.ReadKey();
                    }
                }                
            }
            else
            {
                Console.WriteLine("No contacts found."); // Om kontaktlistan är tom visas detta.
                Console.ReadKey();
            }
        }

        public void SearchContact() // För att se 1 kontakt måste anv. söka...
        {
            Console.Clear();
            Console.Write("Enter first name or last name of the contact you are looking for: "); // ...med för- eller efternamn.
            string searchName = Console.ReadLine().ToLower();
            Console.Clear();

            
            foreach (Contact contact in _contacts) // Sedan loopas _contacts igenom...
            {
                if (contact.FirstName.ToLower() == searchName || contact.LastName.ToLower() == searchName) // ...och om det sökta namnet finns i listan...
                {
                    ShowContact(_contacts.IndexOf(contact)); // ...visas ShowContact för den kontakten.
                }
            }          
        }

        public void ShowContact(Int32 index) // Här visas en kontakt. Index tas in som argument...
        {
            
            Contact contact = _contacts[index]; // ...och kontakten med det indexet skrivs ut med full info.
            Console.WriteLine();
            Console.WriteLine($"{contact.FirstName} {contact.LastName} - {contact.Phone} - {contact.Email}");
            Console.WriteLine("------------------------------------------------------");
            Console.WriteLine("To edit contact press E \nTo delete contact press D \nor press enter to continue.");
            string option = Console.ReadLine().ToLower(); 

            switch(option) // Om anv. väljer att redigera eller radera kontakten körs respektive metod,...
            {
                case "e":
                    EditContact(ref contact);
                    break;
                case "d":
                    DeleteContact(contact.Id);
                    break;
                default: // ... annars går vi vidare till nästa kontakt eller till huvudmenyn om det inte finns några fler.
                    break;
            }
        }

        public void DeleteContact(Guid id) // _contacts filtreras till att inte innehålla den kontakt vars id skickats in i metoden och sparas sedan.
        {
            _contacts = _contacts.Where(c => c.Id != id).ToList();
            _fileService.Save(_contacts);
            Console.WriteLine("Contact deleted.");
            Console.ReadKey();
            MainMenu();
        }

        public void EditContact(ref Contact contact) // En referens till kontakten skickas in istället för själva kontakten,
        {                                            // så att vi inte behöver skriva över hela kontakten med en kopia
            Console.WriteLine();
            Console.Write("Enter new first name (or press enter to keep current name): ");  
            string firstName = Console.ReadLine().Trim();
            contact.FirstName = (firstName != "") ? firstName : contact.FirstName; // En shorthand av en if-sats som ersätter namnet eller låter bli om anv. tryckt enter.
            Console.Write("Enter new last name (or press enter to keep current name): ");
            string lastName = Console.ReadLine().Trim();
            contact.LastName = (lastName != "") ? lastName : contact.LastName;
            Console.Write("Enter new phone number (or press enter to keep current number): ");
            string phone = Console.ReadLine().Trim();
            contact.Phone = (phone != "") ? phone : contact.Phone;
            Console.Write("Enter new E-mail (or press enter to keep current address): ");
            string eMail = Console.ReadLine().Trim();
            contact.Email = (eMail != "") ? eMail : contact.Email;

            _fileService.Save(_contacts); // Till sist sparas kontakten.
        }
    }
}
