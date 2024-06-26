using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace ContactManager.Endpoints;

public class UpdateContactRequest
{
  [FromRoute]
  public int Id { get; set; }

  [Required]
  public string FirstName { get; set; }

  [Required]
  public string LastName { get; set; }

  [Required]
  [EmailAddress]
  public string Email { get; set; }
}