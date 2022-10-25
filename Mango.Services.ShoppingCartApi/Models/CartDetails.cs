using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mango.Services.ShoppingCartApi.Models
{
    public class CartDetails
    {
        [Key]
        public int CartDetailsId { get; set; }
        public int CartHeaderId { get; set; }
        public int ProductId { get; set; }
        public int Count { get; set; }

        [ForeignKey("CartHeader")]
        public virtual CartHeader CartHeader { get; set; }
        [ForeignKey("Product")]
        public virtual Product Product { get; set; }
    }
}
