using CartingService.BLL.Models;
using CartingService.DAL.Models;
using CartingService.DAL.Repositories;
using System;
using System.Collections.Generic;

namespace CartingService.BLL
{

    namespace CartingService.BLL
    {

        public class CartService : ICartService
        {
            private readonly ICartRepository _cartRepository;

            public CartService(ICartRepository cartRepository)
            {
                _cartRepository = cartRepository;
            }

            public Cart GetCart(Guid cartId)
            {
                var cartDbModel = _cartRepository.GetById(cartId);

                if (cartDbModel == null)
                {
                    return null;
                }

                var cart = MapToCart(cartDbModel);
                return cart;
            }


            public void AddItemToCart(Guid cartId, AddCartItem item)
            {
                var cartDbModel = _cartRepository.GetById(cartId);

                if (cartDbModel == null)
                {
                    cartDbModel = new CartDBModel
                    {
                        Id = Guid.NewGuid()
                    };
                    _cartRepository.Insert(

                        new CartDBModel() { Id = cartId, Items = new List<CartItemDBModel>() { MapToCartItemDBModel(item) } });
                }
                else
                {
                    cartDbModel.Items.Add(MapToCartItemDBModel(item));
                    _cartRepository.Update(cartDbModel);
                }

            }

            public void RemoveItemFromCart(Guid cartId, Guid itemId)
            {
                var cartDbModel = _cartRepository.GetById(cartId);

                if (cartDbModel == null)
                {
                    return;
                }

                cartDbModel.Items.RemoveAll(i => i.Id == itemId);
                _cartRepository.Update(cartDbModel);
            }

            private Cart MapToCart(CartDBModel cartDbModel)
            {
                var cart = new Cart
                {
                    Id = cartDbModel.Id,
                    Items = new List<CartItem>()
                };

                foreach (var item in cartDbModel.Items)
                {
                    cart.Items.Add(MapToCartItem(item));
                }

                return cart;
            }

            private CartItem MapToCartItem(CartItemDBModel itemDbModel)
            {
                return new CartItem
                {
                    Id = itemDbModel.Id,
                    Name = itemDbModel.Name,
                    Image = itemDbModel.Image,
                    Price = itemDbModel.Price,
                    Quantity = itemDbModel.Quantity
                };
            }

            private CartItemDBModel MapToCartItemDBModel(AddCartItem item)
            {
                return new CartItemDBModel
                {
                    Id = Guid.NewGuid(),
                    Name = item.Name,
                    Image = item.Image,
                    Price = item.Price,
                    Quantity = item.Quantity
                };
            }

            public IEnumerable<Cart> GetAllCarts()
            {
                IList<Cart> cart = new List<Cart>();
                var collection = _cartRepository.GetAll();
                foreach (var item in collection)
                {
                    cart.Add(MapToCart(item));
                }
                return cart;
            }
        }

        public interface ICartService
        {
            public Cart GetCart(Guid cartId);
            public void AddItemToCart(Guid cartId, AddCartItem item);
            public IEnumerable<Cart> GetAllCarts();
            public void RemoveItemFromCart(Guid cartId, Guid itemId);
        }
    }
}
