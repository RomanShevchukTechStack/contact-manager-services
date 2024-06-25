using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ContactManager.Endpoints;

public class DeleteContactRequest
{
  public int Id { get; set; }
}
