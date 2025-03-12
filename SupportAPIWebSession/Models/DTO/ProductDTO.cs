using System.ComponentModel.DataAnnotations;

namespace SupportAPIWebSession.Models.DTO {
    public class ProductDTO {

        public int Id {
            get; set;
        }
        [Required]
        [MaxLength( 30 )]
        public string ProductName {
            get; set;
        }
        public int Quantity {
            get; set;
        }
        public int Price {
            get; set;
        }
        public string Description {
            get; set;
        }

    }
}
