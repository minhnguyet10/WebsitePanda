using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Website_Panda.Models.QueryModel
{
    public class HistoryItem
    {
        public Product _shopping_product { get; set; }
    }
    public class History
    {
        List<HistoryItem> items = new List<HistoryItem>();
        public IEnumerable<HistoryItem> Items
        {
            get { return items; }
        }
        public void Add(Product _pro)
        {
            var item = items.FirstOrDefault(s => s._shopping_product.IDProduct == _pro.IDProduct);
            if (item == null)
            {
                items.Add(new HistoryItem
                {
                    _shopping_product = _pro
                });
            }
            else
            {
            }
        }
        public void ClearCart()
        {
            items.Clear();
        }
    }
}