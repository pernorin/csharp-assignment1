using AdressBook.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace AdressBook.Services
{
    internal interface IFileService // Ett interface som visar vad klassen ska innehålla och kan användas för att instansera klassen
    {
        public List<Contact> Read();
        public void Save(List<Contact> contacts);
    }
    internal class FileService : IFileService
    {
        private string _filePath; // Ett fält som endast kan nås i denna klass skapas

        public FileService(string filePath) // Konstruktorn tar in en filepath och lägger in det i klassens privata fält
        {
            _filePath = filePath;
        }

        public List<Contact> Read() // En metod som läser från en fil och returnerar resultatet
        {
            var contacts = new List<Contact>();
            
            using var sr = new StreamReader(_filePath);
            contacts = JsonConvert.DeserializeObject<List<Contact>>(sr.ReadToEnd());

            return contacts;
           
        }
        public void Save(List<Contact> contacts) // En metod som sparar listan contacts till en fil
        {
            using StreamWriter sw = new StreamWriter(_filePath);
            sw.WriteLine(JsonConvert.SerializeObject(contacts, Formatting.Indented));
        }
    }
}
