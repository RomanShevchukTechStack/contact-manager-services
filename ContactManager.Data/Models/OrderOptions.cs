using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactManager.Data.Models
{
  public class OrderOptions
  {
    public string OrderBy { get; }
    public string OrderDirection { get; }

    public OrderOptions(string orderBy, string orderDirection)
    {
      OrderBy = orderBy;
      OrderDirection = orderDirection;
    }
  }
}
