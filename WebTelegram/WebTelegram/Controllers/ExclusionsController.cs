using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebTelegram.Abstract;
using WebTelegram.Constants;
using WebTelegram.Models.Exclusions;

namespace WebTelegram.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = Roles.Admin)]
    public class ExclusionsController : ControllerBase
    {
        private readonly IExclusionsService _exclusionsService;
        private readonly IMapper _mapper;

        public ExclusionsController(IExclusionsService exclusionsService, IMapper mapper)
        {
            _exclusionsService = exclusionsService;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetList([FromQuery] ExclusionSearchViewModel model)
        {
            var query = _exclusionsService.Exclusions
                .Include(x => x.City)
                .ThenInclude(x=>x.Area)
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

            if (!string.IsNullOrEmpty(model.City))
            {
                query = query.Where(x => x.City.Name.ToLower().Contains(model.City.ToLower()));
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
                .Select(x => _mapper.Map<ExclusionItemViewModel>(x))
                .ToList();

            int total = query.Count();
            int pages = (int)Math.Ceiling(total / (double)model.CountOnPage);

            return Ok(new ExclusionSearchResultViewModel()
            {
                CurrentPage = model.Page,
                Pages = pages,
                Total = total,
                Exclusions = list,
            });
        }


        [HttpGet("most-buys")]
        public IActionResult GetList(int count)
        {
            var list = _exclusionsService.Exclusions
                .Include(x => x.City)
                .Where(x => !x.IsDeleted)
                .OrderBy(x => x.Id)
                .Take(count)
                .Select(x => _mapper.Map<ExclusionItemViewModel>(x))
                .ToList();

            return Ok(list);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateExclusionViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var id = await _exclusionsService.Create(model);
            return Ok();
        }
        [HttpGet("id/{id}")]
        public IActionResult GetItem(int id)
        {
            var model = _exclusionsService.Exclusions
                .Include(x => x.City)
                .SingleOrDefault(x => x.Id == id);
            if (model == null)
                return NotFound();
            return Ok(_mapper.Map<ExclusionItemViewModel>(model));
        }

        [HttpPut]
        public async Task<IActionResult> Edit([FromBody] EditExclusionViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            await _exclusionsService.Update(model);

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _exclusionsService.Delete(id);
            return Ok();
        }
    }
}
