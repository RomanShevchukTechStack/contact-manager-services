using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactManager.Data.Models
{
  public class TableContactsDTO
  {
    public TableContactsDTO(int totalRecords, int currentPage, int totalPages, IEnumerable<Contact> contacts)
    {
      TotalRecords = totalRecords;
      CurrentPage = currentPage;
      TotalPages = totalPages;
      Contacts = contacts;
    }

    public int TotalRecords { get; set; }
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public IEnumerable<Contact> Contacts { get; set; }
  }

}
