using Org.BouncyCastle.Asn1;
using recommender.Models;

namespace recommender.Repos
{
    public interface ICarRepo
    {
        public  Task<object> GetCarByParams(CarParamsDTO dto);
        public void SaveFileToDB(IFormFile file);

    }
}
