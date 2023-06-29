using System;
using System.Collections.Generic;

#nullable disable

namespace recommender.DataProvide
{
    public partial class Car
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? Year { get; set; }
        public int? SellingPrice { get; set; }
        public int? KmDriven { get; set; }
        public int FuelTypeId { get; set; }
        public int TransmissionId { get; set; }
        public int OwnerTypeId { get; set; }
    }
}
