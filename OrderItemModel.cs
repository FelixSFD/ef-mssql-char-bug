using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ef_mssql_char_bug
{
  public class OrderItemModel
  {
    [Key]
    public int ItemId { get; set; }


    [Column(TypeName = "char(10)")]
    [MinLength(10)]
    [MaxLength(10)]
    public string OrderId { get; set; }


    public string ItemName { get; set; }


    public double Quantity { get; set; }


    public virtual OrderModel Order { get; set; }
  }
}
