using ApiCaching.Data;
using ApiCaching.Models;
using ApiCaching.Services;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;

namespace ApiCaching.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DriversController : ControllerBase
    {
        private readonly ICacheService _cacheService;
        private readonly AppDbContext _context;

        public DriversController(ICacheService cacheService, AppDbContext context)
        {
            _cacheService = cacheService;
            _context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Driver>> GetDrivers()
        {
            var cachedData = _cacheService.GetData<IEnumerable<Driver>>("drivers");
            if(cachedData != null && cachedData.Count() > 0)
            {
                return Ok(cachedData);
            } 
            
            var drivers = _context.Drivers.ToList();
            _cacheService.SetData<IEnumerable<Driver>>("drivers", drivers, DateTimeOffset.Now.AddMinutes  (2));

            return Ok(drivers);
        }

        [HttpGet("/${id}", Name = "GetDriver")]
        public ActionResult<Driver> GetDriver(int id)
        {
            var cachedData = _cacheService.GetData<IEnumerable<Driver>>("drivers");
            if (cachedData != null && cachedData.Count() > 0)
            {
                var cachedDriver = cachedData.FirstOrDefault(x => x.Id == id);
                return Ok(cachedDriver);
            }
            var drivers = _context.Drivers.ToList();
            _cacheService.SetData<IEnumerable<Driver>>("drivers", drivers, DateTimeOffset.Now.AddMinutes(2));

            var driver = drivers.FirstOrDefault(x => x.Id == id);
            return Ok(driver);
        }

        [HttpPost]
        public async Task<ActionResult> SetDriver(Driver driver)
        {
            try
            {
                await _context.Drivers.AddAsync(driver);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetDriver), new { id = driver.Id }, driver);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
