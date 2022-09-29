using AdressBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AdressBook.Services
{
    internal interface IMenuService
    {
        public void MainMenu();
        public void CreateContact();
        //public void ShowContacts();
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
                    ShowContacts(_contacts);
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


            // _contacts.RemoveAt(<index>);
        }

        public void EditContact(Guid id)
        {
            throw new NotImplementedException();
        }

        

        public void ShowContacts(List<Contact>  contacts)
        {                  

            Console.Clear();

            if(contacts.Count() != 0)
            { 
                foreach (Contact contact in contacts)
                {
                    Console.WriteLine($"{contacts.IndexOf(contact) + 1}. {contact.FirstName} {contact.LastName} - {contact.Phone} - {contact.Email}");// - - {contact.Id}

                    // Kanske inte visa allt detta
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
            List<Int32> results = new();
            foreach (Contact contact in _contacts)
            {
                
                if (contact.FirstName.ToLower() == searchName || contact.LastName.ToLower() == searchName)
                {
                    results.Add(_contacts.IndexOf(contact));
                    //ShowContact(_contacts.IndexOf(contact));
                }                
            }

            if (results.Count == 1)
            {
                ShowContact(results[0]);
            }
            else
            {
                ShowContacts(results);
            }
            
            Console.ReadKey();

            /*
            foreach (Contact contact in _contacts)
            {
                if (contact.FirstName.ToLower() == searchName || contact.LastName.ToLower() == searchName)
                {
                    Console.WriteLine($"{contact.FirstName} {contact.LastName} - {contact.Phone} - {contact.Email}");
                }
            }
            Console.ReadKey();
            */
        }
/*
        public void ShowContact(Contact)
        {
            //Console.Clear();
            //Contact contact = _contacts[index];
            Console.WriteLine($"{contact.FirstName} {contact.LastName} - {contact.Phone} - {contact.Email}");
        }
*/
        public void ShowContact(Int32 index)
        {
            //Console.Clear();
            Contact contact = _contacts[index];
            Console.WriteLine($"{contact.FirstName} {contact.LastName} - {contact.Phone} - {contact.Email}");
            //Console.ReadKey();
        }

        


        // main menu
        // create contact menu
        // view all menu
        // view one menu -> delete/edit - menu   -ska kunna kallas från view all och search
        // search menu  -search metod: overload där en tar in int från view all och en tar in string från search
    }
}
