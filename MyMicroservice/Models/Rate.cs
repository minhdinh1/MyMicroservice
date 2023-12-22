namespace MyMicroservice.Models
{
    public class Rate
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Symbol { get; set; }
        public double Price { get; set; }
        public DateTime LastUpdated { get; set; }
        public DateTime CreatedDateTime { get; set; }
    }
}
