using ContactManager.Data.Models;
using Newtonsoft.Json;

namespace ContactManager.Data
{
  public class DataContext
  {
    private const string FilePath = "contacts.json";
    private List<Contact> contacts;

    public DataContext()
    {
      if (File.Exists(FilePath))
      {
        var json = File.ReadAllText(FilePath);
        contacts = JsonConvert.DeserializeObject<List<Contact>>(json) ?? new List<Contact>();
      }
      else
      {
        contacts = new List<Contact>();
      }
    }

    public List<Contact> GetContacts() => contacts;

    public Contact GetContact(int id) => contacts.FirstOrDefault(c => c.Id == id);

    public void AddContact(Contact contact)
    {
      contact.Id = contacts.Any() ? contacts.Max(c => c.Id) + 1 : 1;
      contacts.Add(contact);
      SaveChanges();
    }

    public void UpdateContact(Contact contact)
    {
      var existing = GetContact(contact.Id);
      if (existing != null)
      {
        existing.FirstName = contact.FirstName;
        existing.LastName = contact.LastName;
        existing.Email = contact.Email;
        SaveChanges();
      }
    }

    public void DeleteContact(int id)
    {
      var contact = GetContact(id);
      if (contact != null)
      {
        contacts.Remove(contact);
        SaveChanges();
      }
    }

    private void SaveChanges()
    {
      var json = JsonConvert.SerializeObject(contacts, Formatting.Indented);
      File.WriteAllText(FilePath, json);
    }
  }
}
