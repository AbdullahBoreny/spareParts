using System;


namespace spareParts.Models{
    public class ShopWithProducts
    {
            public int ProductCount { get; set; }
            public double Rating { get; set; }
            public double Distance { get; set; }
            public List<Product> FeaturedProducts { get; set; } = new List<Product>();

            public int Id {get; set;}
            public string Name {get; set;}
            public string Description {get; set;}
            public string Address {get; set;}
            public string Phone {get; set;}
            public string Email {get; set;}
            public bool IsActive {get; set;}
            public string UserID {get; set;}
    }
}
