using System;
using System.Collections.Generic;
using System.Text;

namespace ShopT.Models.Maps
{
    public class ShopSelectedEventArgs : EventArgs
    {
        public int shopId;

        public ShopSelectedEventArgs(int shopId) 
        {
            this.shopId = shopId;
        }
    }
}
