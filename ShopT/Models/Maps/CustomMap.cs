using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Xamarin.Forms.Maps;

namespace ShopT.Models.Maps
{
    public class CustomMap : Map
    {
        public List<CustomPin> CustomPins { get; set; }

        public event EventHandler<ShopSelectedEventArgs> ShopSelected;

        public virtual void OnShopSelected(ShopSelectedEventArgs e)
        {
            EventHandler<ShopSelectedEventArgs> handler = ShopSelected;
            if (handler != null)
            {
                handler(this, e);
            }
        }
    }
}
