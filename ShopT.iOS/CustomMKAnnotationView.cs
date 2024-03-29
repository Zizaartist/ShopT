﻿using MapKit;

namespace ShopT.iOS
{
    public class CustomMKAnnotationView : MKAnnotationView
	{
		public string Name { get; set; }

		public string Url { get; set; }

		public string Images { get; set; }

		public CustomMKAnnotationView(IMKAnnotation annotation, string id)
			: base(annotation, id)
		{
		}
	}
}