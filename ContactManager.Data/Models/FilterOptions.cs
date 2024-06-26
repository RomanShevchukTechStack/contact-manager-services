using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactManager.Data.Models
{
  public class FilterOptions
  {
    public string? SearchValue { get; }

    public FilterOptions(string searchValue)
    {
      SearchValue = searchValue;
    }
  }
}
