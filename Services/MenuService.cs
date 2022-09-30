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
    internal interface IMenuService
    {
        public void MainMenu();
        public void CreateContact();
        public void ShowContacts();
        public void SearchContact();
        
    }

    internal class MenuService : IMenuService
    {
        private IFileService _fileService;
        private static List<Contact> _contacts = new List<Contact>();

        public MenuService(string filePath)
        {
            _fileService = new FileService(filePath);
            try
            {
                _contacts = _fileService.Read();
            }
            catch
            {
                _contacts = new List<Contact>();
            }
        }

        public void MainMenu()
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

        public void CreateContact()
        {
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

        public  void ShowContacts()
        {
            Console.Clear();

            if(_contacts.Count() != 0)
            { 
                foreach (Contact contact in _contacts)
                {
                    Console.WriteLine($"{_contacts.IndexOf(contact) + 1}. {contact.FirstName} {contact.LastName}");// - {contact.Phone} - {contact.Email} - - {contact.Id}

                }
                Console.Write("Enter number to see details or M to go back to Main Menu: ");

                var show = Console.ReadLine();

                if(show.ToLower() == "m")
                {
                    MainMenu();
                }
                else 
                {
                    Console.Clear();
                    try
                    {
                        ShowContact(Int32.Parse(show) - 1);
                    }
                    catch
                    {
                        Console.WriteLine("Invalid choice.");
                        Console.ReadKey();
                    }
                }                
            }
            else
            {
                Console.WriteLine("No contacts found.");
                Console.ReadKey();
            }
        }

        public void SearchContact()
        {
            Console.Clear();
            Console.Write("Enter first name or last name of the contact you are looking for: ");
            string searchName = Console.ReadLine().ToLower();
            Console.Clear();

            foreach (Contact contact in _contacts)
            {
                if (contact.FirstName.ToLower() == searchName || contact.LastName.ToLower() == searchName)
                {
                    ShowContact(_contacts.IndexOf(contact));
                }
            }    //om inget namn hittas går det tillbaka till mainMenu
        }

        public void ShowContact(Int32 index)
        {
            
            Contact contact = _contacts[index];
            Console.WriteLine();
            Console.WriteLine($"{contact.FirstName} {contact.LastName} - {contact.Phone} - {contact.Email}");
            Console.WriteLine("------------------------------------------------------");
            Console.WriteLine("To edit contact press E \nTo delete contact press D \nor press enter to continue.");
            string option = Console.ReadLine().ToLower(); 

            switch(option)
            {
                case "e":
                    EditContact(ref contact);
                    break;
                case "d":
                    DeleteContact(contact.Id);
                    break;
                default:
                    break;
            }
        }

        public void DeleteContact(Guid id)
        {
            _contacts = _contacts.Where(c => c.Id != id).ToList();
            _fileService.Save(_contacts);
            Console.WriteLine("Contact deleted.");
            Console.ReadKey();
            MainMenu();
        }

        public void EditContact(ref Contact contact)
        {
            Console.WriteLine();
            Console.Write("Enter new first name (or press enter to keep current name): ");
            string firstName = Console.ReadLine().Trim();
            contact.FirstName = (firstName != "") ? firstName : contact.FirstName;
            Console.Write("Enter new last name (or press enter to keep current name): ");
            string lastName = Console.ReadLine().Trim();
            contact.LastName = (lastName != "") ? lastName : contact.LastName;
            Console.Write("Enter new phone number (or press enter to keep current number): ");
            string phone = Console.ReadLine().Trim();
            contact.Phone = (phone != "") ? phone : contact.Phone;
            Console.Write("Enter new E-mail (or press enter to keep current address): ");
            string eMail = Console.ReadLine().Trim();
            contact.Email = (eMail != "") ? eMail : contact.Email;

            _fileService.Save(_contacts);
            MainMenu();

        }
    }
}
