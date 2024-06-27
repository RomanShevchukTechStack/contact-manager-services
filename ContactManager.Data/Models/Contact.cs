using Ardalis.Result;
using ContactManager.Data.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace ContactManager.Data.Models
{
  public class Contact: BaseEntity
  {
    public Contact(string firstName, string lastName, string email)
    {
      FirstName = firstName;
      LastName = lastName;  
      Email = email;
    }

    [Required(ErrorMessage = "First Name is required.")]
    public string FirstName { get; set; }

    [Required(ErrorMessage = "Last Name is required.")]
    public string LastName { get; set; }

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid Email Address.")]
    public string Email { get; set; }

    public void Update(string firstName, string lastName, string email)
    {
      FirstName = firstName;
      LastName = lastName;
      Email = email;
    }
  }
}
