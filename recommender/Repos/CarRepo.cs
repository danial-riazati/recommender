using Microsoft.EntityFrameworkCore;
using recommender.DataProvide;
using recommender.Models;
using System.IO;

namespace recommender.Repos
{
    public class CarRepo : ICarRepo
    {
        private readonly RecommenderDBContext _context;

        public CarRepo(RecommenderDBContext context)
        {
            _context = context;
        }

        public async Task<object> GetCarByParams(CarParamsDTO dto)
        {
            var kmDrivenMin = dto.YearRange == 0 ? 0 : dto.YearRange == 1 ? 50000 : dto.YearRange == 2 ? 200000 : 400000;
            var kmDrivenMax = dto.YearRange == 0 ? 50000 : dto.YearRange == 1 ? 200000 : dto.YearRange == 2 ? 400000 : 1000000;

            var priceMin = dto.YearRange == 0 ? 0 : dto.YearRange == 1 ? 40000 : dto.YearRange == 2 ? 100000 : dto.YearRange == 3 ? 300000 : dto.YearRange == 4 ? 1000000 : 5000000;
            var priceMax = dto.YearRange == 0 ? 40000 : dto.YearRange == 1 ? 100000 : dto.YearRange == 2 ? 300000 : dto.YearRange == 3 ? 1000000 : dto.YearRange == 4 ? 5000000 : 10000000;

            var yearMin = dto.YearRange == 0 ? 1950 : dto.YearRange == 1 ? 2000 : dto.YearRange == 2 ? 2010 : 2015;
            var yearMax = dto.YearRange == 0 ? 2000 : dto.YearRange == 1 ? 2010 : dto.YearRange == 2 ? 2015 : 2024;

            var cars = (from c in _context.Cars
                        join cc in _context.Transmissions on c.TransmissionId equals cc.Id
                        join ccc in _context.FuelTypes on c.FuelTypeId equals ccc.Id
                        join cccc in _context.OwnerTypes on c.OwnerTypeId equals cccc.Id

                        where c.Year >= yearMin && c.Year <= yearMax
                        && c.SellingPrice >= priceMin && c.SellingPrice <= priceMax
                        && c.KmDriven >= kmDrivenMin && c.KmDriven <= kmDrivenMax
                        && c.FuelTypeId == dto.fuelTypeId && c.TransmissionId == dto.transmissionId
                        && c.OwnerTypeId == dto.ownerTypeId
                        select new
                        {
                            c.Name,
                            c.Year,
                            c.SellingPrice,
                            c.KmDriven,
                            transmissions = cc.Name,
                            fuelType = ccc.Name,
                            ownerType = cccc.Name,
                        }
                       ).Take(dto.maxCountNum).ToList();

            return cars;

        }

        public async void SaveFileToDB(IFormFile file)
        {
            using (var stream = new MemoryStream())
            {
                file.CopyTo(stream);
                stream.Position = 0;

                using (var reader = new StreamReader(stream))
                {
                    using (var csvReader = new CsvHelper.CsvReader(reader, System.Globalization.CultureInfo.InvariantCulture))
                    {
                        var records = csvReader.GetRecords<dynamic>();
                        List<Car> cars = new List<Car>();
                        foreach (var record in records)
                        {
                            Car car = new Car();
                            foreach (var property in record)
                            {
                                var columnName = property.Key;
                                var columnValue = property.Value;
                                switch (columnName)
                                {
                                    case "name":
                                        car.Name = property.Value;
                                        break;
                                    case "year":
                                        car.Year = int.Parse(property.Value);
                                        break;
                                    case "sellingPrice":
                                        car.SellingPrice = int.Parse(property.Value);
                                        break;
                                    case "kmDriven":
                                        car.KmDriven = int.Parse(property.Value);
                                        break;
                                    case "fuel":
                                        if (property.Value == "Petrol")
                                            car.FuelTypeId = 0;
                                        if (property.Value == "Diesel")
                                            car.FuelTypeId = 1;
                                        if (property.Value == "CNG")
                                            car.FuelTypeId = 2;
                                        break;
                                    case "transmission":
                                        if (property.Value == "Manual")
                                            car.TransmissionId = 0;
                                        if (property.Value == "Automatic")
                                            car.TransmissionId = 1;

                                        break;
                                    case "owner":
                                        if (property.Value == "First Owner")
                                            car.OwnerTypeId = 0;
                                        if (property.Value == "Second Owner")
                                            car.OwnerTypeId = 1;
                                        if (property.Value == "Third Owner")
                                            car.OwnerTypeId = 2;
                                        if (property.Value == "Fourth & Above Owner")
                                            car.OwnerTypeId = 3;

                                        break;
                                }

                            }
                            cars.Add(car);
                        }
                        _context.AddRange(cars);
                        await _context.SaveChangesAsync();
                    }
                }
            }
        }
    }
}
