using ContactManager.Data.Entities;
using ContactManager.Data.Models;
using Newtonsoft.Json;

namespace RepositoryTests
{
  public class Contact : BaseEntity
  {
    public string Name { get; set; }
    public string Email { get; set; }
  }

  [TestClass]
  public class RepositoryTests
  {
    private string filePath;
    private Repository<Contact> repository;

    [TestInitialize]
    public void Setup()
    {
      filePath = "test_contacts.json";
      File.WriteAllText(filePath, JsonConvert.SerializeObject(new List<Contact>()));
      repository = new Repository<Contact>(filePath);
    }

    [TestCleanup]
    public void TearDown()
    {
      if (File.Exists(filePath))
      {
        File.Delete(filePath);
      }
    }

    [TestMethod]
    public void Add_ShouldAddContact()
    {
      var contact = new Contact { Name = "John Doe", Email = "john.doe@example.com" };

      var result = repository.Add(contact);

      Assert.IsTrue(result.IsSuccess);
      Assert.AreEqual(1, result.Value.Id);
    }

    [TestMethod]
    public void GetAll_ShouldReturnAllContacts()
    {
      var contact1 = new Contact { Name = "John Doe", Email = "john.doe@example.com" };
      var contact2 = new Contact { Name = "Jane Smith", Email = "jane.smith@example.com" };
      repository.Add(contact1);
      repository.Add(contact2);

      var result = repository.GetAll();

      Assert.IsTrue(result.IsSuccess);
      Assert.AreEqual(2, result.Value.Count);
    }

    [TestMethod]
    public void GetById_ShouldReturnCorrectContact()
    {
      var contact = new Contact { Name = "John Doe", Email = "john.doe@example.com" };
      repository.Add(contact);

      var result = repository.GetById(contact.Id);

      Assert.IsTrue(result.IsSuccess);
      Assert.AreEqual("John Doe", result.Value.Name);
    }

    [TestMethod]
    public void Update_ShouldUpdateContact()
    {
      var contact = new Contact { Name = "John Doe", Email = "john.doe@example.com" };
      repository.Add(contact);
      contact.Name = "Johnathan Doe";

      var result = repository.Update(contact);

      Assert.IsTrue(result.IsSuccess);
      Assert.AreEqual("Johnathan Doe", result.Value.Name);
    }

    [TestMethod]
    public void Delete_ShouldRemoveContact()
    {
      var contact = new Contact { Name = "John Doe", Email = "john.doe@example.com" };
      repository.Add(contact);

      var result = repository.Delete(contact.Id);

      Assert.IsTrue(result.IsSuccess);
      Assert.IsNull(repository.GetById(contact.Id).Value);
    }

    [TestMethod]
    public void GetAll_ShouldFilterContacts()
    {
      var contact1 = new Contact { Name = "John Doe", Email = "john.doe@example.com" };
      var contact2 = new Contact { Name = "Jane Smith", Email = "jane.smith@example.com" };
      repository.Add(contact1);
      repository.Add(contact2);

      var filterOptions = new FilterOptions("Jane");
      var result = repository.GetAll(filterOptions: filterOptions);

      Assert.IsTrue(result.IsSuccess);
      Assert.AreEqual(1, result.Value.Count);
      Assert.AreEqual("Jane Smith", result.Value[0].Name);
    }

    [TestMethod]
    public void GetAll_ShouldPaginateContacts()
    {
      for (int i = 1; i <= 10; i++)
      {
        repository.Add(new Contact { Name = $"Contact {i}", Email = $"contact{i}@example.com" });
      }

      var paginationOptions = new PaginationOptions(page: 2, count: 3);
      var result = repository.GetAll(paginationOptions: paginationOptions);

      Assert.IsTrue(result.IsSuccess);
      Assert.AreEqual(3, result.Value.Count);
      Assert.AreEqual("Contact 4", result.Value[0].Name);
    }

    [TestMethod]
    public void GetAll_ShouldOrderContactsAscending()
    {
      var contact1 = new Contact { Name = "John Doe", Email = "john.doe@example.com" };
      var contact2 = new Contact { Name = "Jane Smith", Email = "jane.smith@example.com" };
      var contact3 = new Contact { Name = "Albert Johnson", Email = "albert.johnson@example.com" };
      repository.Add(contact1);
      repository.Add(contact2);
      repository.Add(contact3);

      var orderOptions = new OrderOptions(orderBy: "Name", orderDirection: "asc");
      var result = repository.GetAll(orderOptions: orderOptions);

      Assert.IsTrue(result.IsSuccess);
      Assert.AreEqual(3, result.Value.Count);
      Assert.AreEqual("Albert Johnson", result.Value[0].Name);
      Assert.AreEqual("Jane Smith", result.Value[1].Name);
      Assert.AreEqual("John Doe", result.Value[2].Name);
    }

    [TestMethod]
    public void GetAll_ShouldOrderContactsDescending()
    {
      var contact1 = new Contact { Name = "John Doe", Email = "john.doe@example.com" };
      var contact2 = new Contact { Name = "Jane Smith", Email = "jane.smith@example.com" };
      var contact3 = new Contact { Name = "Albert Johnson", Email = "albert.johnson@example.com" };
      repository.Add(contact1);
      repository.Add(contact2);
      repository.Add(contact3);

      var orderOptions = new OrderOptions(orderBy: "Name", orderDirection: "desc");
      var result = repository.GetAll(orderOptions: orderOptions);

      Assert.IsTrue(result.IsSuccess);
      Assert.AreEqual(3, result.Value.Count);
      Assert.AreEqual("John Doe", result.Value[0].Name);
      Assert.AreEqual("Jane Smith", result.Value[1].Name);
      Assert.AreEqual("Albert Johnson", result.Value[2].Name);
    }

    [TestMethod]
    public void GetAll_ShouldApplyAllOptions()
    {
      for (int i = 1; i <= 10; i++)
      {
        repository.Add(new Contact { Name = $"Contact {i}", Email = $"contact{i}@example.com" });
      }

      var filterOptions = new FilterOptions("5");
      var paginationOptions = new PaginationOptions(page: 1, count: 2);
      var orderOptions = new OrderOptions(orderBy: "Name", orderDirection: "desc");

      var result = repository.GetAll(filterOptions: filterOptions, paginationOptions: paginationOptions, orderOptions: orderOptions);

      Assert.IsTrue(result.IsSuccess);
      Assert.AreEqual(1, result.Value.Count);
      Assert.AreEqual("Contact 5", result.Value[0].Name);
    }
  }
}