﻿using System;
using System.Linq;
using phiNdus.fundus.Business.Dto;
using phiNdus.fundus.Domain.Entities;
using phiNdus.fundus.Domain.Repositories;
using Rhino.Commons;

namespace phiNdus.fundus.Business.Assembler
{
    public class CartAssembler
    {
        public CartDto CreateDto(Cart cart)
        {
            var result = new CartDto();
            result.Id = cart.Id;
            result.Version = cart.Version;
            foreach (var each in cart.Items)
            {
                result.Items.Add(CreateItemDto(each));
            }
            return result;
        }

        private static CartItemDto CreateItemDto(CartItem each)
        {
            var result = new CartItemDto();
            result.Id = each.Id;
            result.Version = each.Version;
            result.Text = each.LineText;
            result.ArticleId = each.Article.Id;
            result.From = each.From;
            result.To = each.To;
            result.Quantity = each.Quantity;
            result.UnitPrice = each.UnitPrice;
            result.LineTotal = each.LineTotal;

            return result;
        }

        public Cart CreateDomainObject(CartDto cartDto)
        {
            var carts = IoC.Resolve<ICartRepository>();
            var cart = carts.FindById(cartDto.Id);

            if (cart == null)
                throw new EntityNotFoundException();
            if (cart.Version != cartDto.Version)
                throw new DtoOutOfDateException();
            
            foreach (var itemDto in cartDto.Items)
            {
                var item = cart.Items.SingleOrDefault(p => p.Id == itemDto.Id);
                if (item == null)
                    throw new EntityNotFoundException();
                if (item.Version != itemDto.Version)
                    throw new DtoOutOfDateException();

                item.Quantity = itemDto.Quantity;
                item.From = itemDto.From;
                item.To = itemDto.To;
            }

            return cart;
        }
    }
}