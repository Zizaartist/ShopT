using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms.Maps;

namespace ShopT.Models.Maps
{
    public class CustomPin : Pin
    {
        public string Name { get; set; }
        public string Images { get; set; }
        public int ShopId { get; set; }
    }
}
