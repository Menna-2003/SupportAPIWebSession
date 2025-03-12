using SupportAPIWebSession.Models.DTO;

namespace SupportAPIWebSession.Data {
    public class Store {

        public static List<ProductDTO> products = new List<ProductDTO> {
                    new ProductDTO { Id = 1,ProductName="product1", Quantity = 10, Description = "description for product 1", Price = 20 },
                    new ProductDTO { Id = 2,ProductName="product2", Quantity = 0, Description = "description for product 2" , Price = 25},
                    new ProductDTO { Id = 3,ProductName="product3", Quantity = 5, Description = "description for product 3" , Price = 60},
                    new ProductDTO { Id = 4,ProductName="product4", Quantity = 30, Description = "description for product 4" , Price = 80},
            };

    }
}
