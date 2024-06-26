using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactManager.Data.Models
{
  public class PaginationOptions
  {
    public int Page { get; }
    public int Count { get; }

    public PaginationOptions(int page, int count)
    {
      Page = page;
      Count = count;
    }
  }
}
