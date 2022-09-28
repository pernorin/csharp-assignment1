using AdressBook.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace AdressBook.Services
{
    internal interface IFileService
    {
        public List<Contact> Read();
        public void Save(List<Contact> contacts);
    }
    internal class FileService : IFileService
    {
        private string _filePath;

        public FileService(string filePath)
        {
            _filePath = filePath;
        }

        public List<Contact> Read()
        {
            var contacts = new List<Contact>();

            using var sr = new StreamReader(_filePath);
            contacts = JsonConvert.DeserializeObject<List<Contact>>(sr.ReadToEnd());

            return contacts;
            /*
            try
            {
                using var sr = new StreamReader(_filePath);
                contacts = JsonConvert.DeserializeObject<List<Contact>>(sr.ReadToEnd());
            }
            catch
            {
                
            }
            return contacts;
            */
        }
        public void Save(List<Contact> contacts)
        {
            using StreamWriter sw = new StreamWriter(_filePath);
            sw.WriteLine(JsonConvert.SerializeObject(contacts, Formatting.Indented));
        }
    }
}
