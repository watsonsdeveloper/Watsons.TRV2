using System.Runtime.ConstrainedExecution;
using Watsons.TRV2.DA.TR.Entities;
using Watsons.TRV2.DA.TR.Repositories;
using Watsons.TRV2.DTO.Mobile;
using Watsons.TRV2.Services.Mobile;

namespace Watsons.TRV2.MSTest
{
    [TestClass]
    public class CartUnitTest
    {
        //private static readonly CartService _cartService = new CartService();
        private static readonly TrContext _trContext = new TrContext();
        public CartUnitTest(CartService cartService)
        {
            //_cartService = cartService;
        }

        [TestMethod]
        public void AddToCart()
        {
            //// Arrange
            //var cart = new AddToTrCartRequest("62304", );
            //var product = new Product { Name = "Apple", Price = 1.2m };

            //// Act
            //cart.Add(product, 5);

            //// Assert
            //Assert.AreEqual(1, cart.Items.Count);
            //Assert.AreEqual(5, cart.Items[0].Quantity);
        }
    }
}