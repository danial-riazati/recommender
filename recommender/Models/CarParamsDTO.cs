namespace recommender.Models
{
    public class CarParamsDTO
    {
        public int PriceRange { get; set; }
        public int YearRange { get; set; }
        public int kmDrivenRange { get; set; }
        public int fuelTypeId { get; set; }
        public int transmissionId { get; set; }
        public int ownerTypeId { get; set; }

        public int maxCountNum { get; set; }


    }
}
