using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebTelegram.Abstract;
using WebTelegram.Constants;
using WebTelegram.Models.Areas;
using WebTelegram.Models.Cities;
using WebTelegram.Services;

namespace WebTelegram.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = Roles.Admin)]
    public class CitiesController : ControllerBase
    {
        private readonly ICitiesService _citiesService;
        private readonly IMapper _mapper;
        public CitiesController(ICitiesService citiesService, IMapper mapper)
        {
            _citiesService = citiesService;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetList([FromQuery] CitySearchViewModel model)
        {
            var query = _citiesService.Cities
                .Include(x => x.Area)
                .Where(x => !x.IsDeleted)
                .AsQueryable();

            if (!string.IsNullOrEmpty(model.Search))
            {
                var searches = model.Search.Split(' ');
                foreach (var sear in searches)
                {
                    query = query.Where(x => x.Name.ToLower().Contains(sear.ToLower()));
                }
            }

            if (!string.IsNullOrEmpty(model.Area))
            {
                query = query.Where(x => x.Area.Name.ToLower().Contains(model.Area.ToLower()));
            }

            switch (model.Sort)
            {
                
                case Sorts.NameAscending:
                    query = query.OrderBy(x => x.Name);
                    break;
                case Sorts.NameDescending:
                    query = query.OrderByDescending(x => x.Name);
                    break;
                default:
                case Sorts.Default:
                    query = query.OrderBy(x => x.Id);
                    break;
            }

            var list = query
                .Skip((model.Page - 1) * model.CountOnPage)
                .Take(model.CountOnPage)
                .Select(x => _mapper.Map<CityItemViewModel>(x))
                .ToList();

            int total = query.Count();
            int pages = (int)Math.Ceiling(total / (double)model.CountOnPage);

            return Ok(new CitySearchResultViewModel()
            {
                CurrentPage = model.Page,
                Pages = pages,
                Total = total,
                Cities = list,
            });
        }


        [HttpGet("most-buys")]
        public IActionResult GetList(int count)
        {
            var list = _citiesService.Cities
                .Include(x => x.Area)
                .Where(x => !x.IsDeleted)
                .OrderBy(x => x.Id)
                .Take(count)
                .Select(x => _mapper.Map<CityItemViewModel>(x))
                .ToList();

            return Ok(list);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateCityViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var id = await _citiesService.Create(model);
            return Ok();
        }
        [HttpGet("id/{id}")]
        public IActionResult GetItem(int id)
        {
            var model = _citiesService.Cities
                .Include(x => x.Area)
                .SingleOrDefault(x => x.Id == id);
            if (model == null)
                return NotFound();
            return Ok(_mapper.Map<CityItemViewModel>(model));
        }

        [HttpPut]
        public async Task<IActionResult> Edit([FromBody] EditCityViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            await _citiesService.Update(model);

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _citiesService.Delete(id);
            return Ok();
        }

        [HttpGet("selector")]
        public IActionResult GetSelector()
        {
            var list = _citiesService.Cities
                .Include(x => x.Area)
                .Where(x => !x.IsDeleted)
                .OrderBy(x => x.Id)
                .Select(x => _mapper.Map<CityItemViewModel>(x))
                .ToList();
            return Ok(list);
        }
    }
}
