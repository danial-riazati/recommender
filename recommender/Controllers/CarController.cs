using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using recommender.DataProvide;
using recommender.Models;
using recommender.Repos;

namespace recommender.Controllers
{


    [Route("api/[controller]")]
    [ApiController]
    public class CarController : ControllerBase
    {
        private readonly RecommenderDBContext _context;
        private readonly IConfiguration _configuration;

        private readonly ICarRepo _carRepo;

        public CarController(

            IConfiguration configuration,
            RecommenderDBContext context,

            ICarRepo carRepo
            )
        {

            _configuration = configuration;
            _context = context;
            _carRepo = carRepo;
        }

        [HttpPost]
        [Route("CarByParams")]
        public async Task<IActionResult> AddCourse([FromBody] CarParamsDTO dto)
        {
            try
            {


                var res = await _carRepo.GetCarByParams(dto);

                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost]
        [Route("SaveCSV")]
        public async Task<IActionResult> SaveCSV(IFormFile file)
        {
            try
            {

                _carRepo.SaveFileToDB(file);

                return Ok("file added successfully");

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
       

    }
}
