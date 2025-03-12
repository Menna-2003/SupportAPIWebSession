using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using SupportAPIWebSession.Data;
using SupportAPIWebSession.Models;
using SupportAPIWebSession.Models.DTO;

namespace SupportAPIWebSession.Controllers {

    //[Route( "api/[controller]" )]   // not effiecient because you will need to notify all the clients
    [Route( "api/[controller]" )]
    [ApiController]
    public class ProductController : ControllerBase {

        #region get 

        [HttpGet]
        [ProducesResponseType( StatusCodes.Status200OK )]
        public ActionResult<IEnumerable<ProductDTO>> GetProducts () {

            // instead of linking to a db for simplicity we will define static store
            //return new List<ProductDTO> {
            //        new ProductDTO { Id = 1,ProductName="p1" },
            //        new ProductDTO { Id = 2,ProductName="p2" }
            //};
            return Ok(Store.products);

        }


        //[HttpGet("id")]
        [HttpGet("{id:int}", Name = "GetProductById" )]  // can explicitly take certain datatype
        [ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(200, Type=typeof(ProductDTO)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<ProductDTO> GetProductById (int id) {

            if ( id == 0 ) {
                return BadRequest(); // 400
            }
            var product = Store.products.FirstOrDefault( p => p.Id == id );
            if ( product == null ) { 
                return NotFound(); //404 
            }
            return Ok( product );  // 200

        }

        #endregion

        #region post
        
        [HttpPost]
        [ProducesResponseType( StatusCodes.Status200OK )]
        [ProducesResponseType( StatusCodes.Status404NotFound )]
        [ProducesResponseType( StatusCodes.Status500InternalServerError )]
        public ActionResult<ProductDTO> CreateProduct ([FromBody]ProductDTO productDTO) {

            //if ( !ModelState.IsValid ) {
            //    return BadRequest( ModelState );
            //}

            if ( Store.products.FirstOrDefault(p=>p.ProductName.ToLower() == productDTO.ProductName.ToLower() ) != null ) {
                // custom error message
                ModelState.AddModelError( "CustomError", "Product with same name exist!" );
                return BadRequest( ModelState );
            }
            if ( productDTO == null ) {
                return BadRequest(productDTO);            
            }
            if ( productDTO.Id > 0 ) {  
                return StatusCode( StatusCodes.Status500InternalServerError ); 
            }

            productDTO.Id = Store.products.OrderByDescending( p => p.Id ).FirstOrDefault().Id + 1;
            Store.products.Add( productDTO );  // data will be deleted after the server restarts because the data is static. you need to connect it to database

            // return Ok( productDTO );
            // instead of just returning 'Ok = 200' you can return the location of the new obj  'it will be -> 201'
            return CreatedAtRoute( "GetProductById", new {id = productDTO.Id} , productDTO );
        }

        #endregion

        #region delete

        [HttpDelete( "{id:int}", Name = "DeleteProduct" )]
        [ProducesResponseType( StatusCodes.Status204NoContent )]
        [ProducesResponseType( StatusCodes.Status404NotFound )]
        [ProducesResponseType( StatusCodes.Status400BadRequest )]
        public IActionResult DeleteProduct ( int id ) {

            if ( id == 0 ) {
                return BadRequest();
            }
            var product = Store.products.FirstOrDefault(p=>p.Id == id);
            if ( product == null ) {
                return NotFound();
            }
            Store.products.Remove( product );
            return NoContent(); 
        }

        #endregion

        #region update

        [HttpPut( "{id:int}", Name = "UpdateProduct" )]
        [ProducesResponseType( StatusCodes.Status204NoContent )]
        [ProducesResponseType( StatusCodes.Status400BadRequest )]
        public IActionResult UpdateProduct ( int id, [FromBody] ProductDTO productDTO ) {
        
            if(id != productDTO.Id ) { return BadRequest(); }

            var product = Store.products.FirstOrDefault(p=>p.Id == id);
            if( product == null ) { return BadRequest(); }
            product.ProductName = productDTO.ProductName;
            product.Quantity = productDTO.Quantity;

            return NoContent();

        }


        [HttpPatch( "{id:int}", Name = "UpdateProductPartially" )]
        [ProducesResponseType( StatusCodes.Status204NoContent )]
        [ProducesResponseType( StatusCodes.Status400BadRequest )]
        public IActionResult UpdateProductPartially ( int id, JsonPatchDocument<ProductDTO>patchDTO) {

            if ( patchDTO == null || id == 0 ) { return BadRequest(); }
            var product = Store.products.FirstOrDefault( p => p.Id == id );
            if( product == null ) { return BadRequest(); }

            patchDTO.ApplyTo(product, ModelState); // if there are any errors they will be stored in ModelState

            if(!ModelState.IsValid) return BadRequest();
            
            return NoContent();

        }


        #endregion

    }
}
