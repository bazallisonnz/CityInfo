using System.Collections.Generic;
using AutoMapper;
using CityInfo.API.Models;
using CityInfo.API.Services;

namespace CityInfo.API.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using System.Linq;

    [Route("api/cities")]
    public class CitiesController : Controller
    {
        private readonly ICityInfoRepository _cityInfoRepository;

        public CitiesController(ICityInfoRepository cityInfoRepository)
        {
            _cityInfoRepository = cityInfoRepository;
        }

        [HttpGet()]
        public IActionResult GetCities()
        {
            var cityEntities = _cityInfoRepository.GetCities();

            var results = Mapper.Map<IEnumerable<CityWithoutPointsOfInterestDto>>(cityEntities);
                
            return Ok(results);
        }

        [HttpGet("{id}")]
        public IActionResult GetCity(int id, bool includePointsOfInterest = false)
        {
            var cityEntity = _cityInfoRepository.GetCity(id, includePointsOfInterest);

            if (cityEntity == null)
            {
                return NotFound();
            }

            if (includePointsOfInterest)
            {
                var cityResult = Mapper.Map<CityDto>(cityEntity);

                return Ok(cityResult);
            }


            var cityWtihoutPointsOfInterest = Mapper.Map<CityWithoutPointsOfInterestDto>(cityEntity);

            return Ok(cityWtihoutPointsOfInterest);
        }
    }
}
