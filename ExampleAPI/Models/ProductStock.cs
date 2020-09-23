using System;
using System.Collections.Generic;
using System.Text;

namespace ExampleAPI.Models
{
  public class ProductStock
  {

    public string productIdentifier { get; set; }

    public int? variantId { get; set; }

    public int physicalQuantity { get; set; }

    public int availableQuantity { get; set; }

    public DateTime? nextReceivalDate { get; set; }

    public decimal nextReceivalQuantity { get; set; }
  }
}
