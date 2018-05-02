namespace WebApplication1.Models
{
    public class Address
    {
        public string City { get; set; }

        public GeoCoords GeoCoords { get; set; }

        public string Street { get; set; }

        public string Suite { get; set; }

        public string ZipCode { get; set; }
    }
}