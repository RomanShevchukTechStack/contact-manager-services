using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace ContactManager.Endpoints;

public class CreateContactRequest
{
  [Required]
  public string FirstName { get; set; }

  [Required]
  public string LastName { get; set; }

  [Required]
  [EmailAddress]
  public string Email { get; set; }
}
