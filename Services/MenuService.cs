using AdressBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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
        //public void ShowContact(string name); // använd polymorphism? https://www.w3schools.com/cs/cs_polymorphism.php
        //public void ShowContact(Int32 index);  // dessa två kanske interna och inte med i interface
        //public void EditContact(Guid id);
        //public void DeleteContact(Guid id);
    }

    internal class MenuService : IMenuService
    {
        //private IFileService _fileService = new FileService($@"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}\contacts.json"); // kanske ändra?
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

        public void DeleteContact(Guid id)
        {
            throw new NotImplementedException();
        }

        public void EditContact(Guid id)
        {
            throw new NotImplementedException();
        }

        

        public  void ShowContacts()
        {
                    

            Console.Clear();

            /*
            try
            {
                _contacts = _fileService.Read();
            }
            catch
            {
                Console.WriteLine("No Contacts Found.");
                Console.ReadKey();
                //MainMenu();
            }
            */

            if(_contacts.Count() != 0)
            { 
                foreach (Contact contact in _contacts)
                {
                    Console.WriteLine($"{_contacts.IndexOf(contact) + 1}. {contact.FirstName} {contact.LastName} - {contact.Phone} - {contact.Email}");// - - {contact.Id}

                    
                    //eller: https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.keyvaluepair-2?view=net-6.0
                }
                Console.Write("Enter number to see details or M to go back to Main Menu: ");

                var show = Console.ReadLine();

                if(show.ToLower() == "m")
                {
                    MainMenu();
                }
                else 
                { 
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
                
                // Int32 show = Int32.Parse(Console.ReadLine()) - 1;

                // Console.Write(_contacts[show].FirstName);
               // ShowContact(show);

                //Console.ReadKey();
            }
            else
            {
                Console.WriteLine("No contacts found.");
                Console.ReadKey();
            }
        }

        public void SearchContact()
        {
            throw new NotImplementedException();
        }

        public void ShowContact(string name)
        {
            Console.Write(name);
        }
        public void ShowContact(Int32 index)
        {
            Console.Clear();
            Contact contact = _contacts[index];
            Console.WriteLine($"{contact.FirstName} {contact.LastName} - {contact.Phone} - {contact.Email}");
            Console.ReadKey();
        }

        


        // main menu
        // create contact menu
        // view all menu
        // view one menu -> delete/edit - menu   -ska kunna kallas från view all och search
        // search menu  -search metod: overload där en tar in int från view all och en tar in string från search
    }
}
