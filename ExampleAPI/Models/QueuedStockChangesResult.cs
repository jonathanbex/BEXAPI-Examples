using System;
using System.Collections.Generic;
using System.Text;

namespace ExampleAPI.Models
{
  public class QueuedStockChangesResult
  {

    public bool hasMore { get; set; }

    public StockChangeEntryModel[] stockChangeEntries { get; set; }
  }

  public class StockChangeEntryModel
  {

    public long id { get; set; }

    public string productIdentifier { get; set; }
  }
}
