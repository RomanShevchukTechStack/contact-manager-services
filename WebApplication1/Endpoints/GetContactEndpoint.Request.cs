using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace ContactManager.Endpoints;

public class GetContactRequest
{
  public int Id { get; set; }
}