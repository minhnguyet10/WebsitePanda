using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Website_Panda.Models.QueryModel
{
    public class WishListItem
    {
        public Product _shopping_product { get; set; }
        public int _shopping_quantity { get; set; }
    }
    public class WishList
    {
        List<WishListItem> items = new List<WishListItem>();
        public IEnumerable<WishListItem> Items
        {
            get { return items; }
        }
        public void Add(Product _pro, int _quantity = 1)
        {
            var item = items.FirstOrDefault(s => s._shopping_product.IDProduct == _pro.IDProduct);
            if (item == null)
            {
                items.Add(new WishListItem
                {
                    _shopping_product = _pro,
                    _shopping_quantity = _quantity
                });
            }
            else
            {
                item._shopping_quantity += _quantity;
            }
        }

        public void Remove_WishList_Item(int id)
        {
            items.RemoveAll(s => s._shopping_product.IDProduct == id);
        }
        public int Total_Quantity()
        {
            return items.Sum(s => s._shopping_quantity);
        }
        public void ClearWishlist()
        {
            items.Clear();
        }
    }
}