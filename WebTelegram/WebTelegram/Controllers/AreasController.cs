using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebTelegram.Abstract;
using WebTelegram.Constants;
using WebTelegram.Models.Areas;

namespace WebTelegram.Controllers
{
    [Authorize(Roles = Roles.Admin)]
    [Route("api/[controller]")]
    [ApiController]
    public class AreasController : ControllerBase
    {
        private readonly IAreasService _areasService;
        private readonly IMapper _mapper;

        public AreasController(IAreasService categoriesService, IMapper mapper)
        {
            _areasService = categoriesService;
            _mapper = mapper;
        }
        [HttpGet]
        public IActionResult GetList([FromQuery] AreaSearchViewModel model)
        {
            var query = _areasService.Areas
                .Where(x => !x.IsDeleted)
                .AsQueryable();

            if (!string.IsNullOrEmpty(model.Search))
            {
                query = query.Where(x => x.Name.ToLower().Contains(model.Search.ToLower()));
            }

            var list = query
                .Skip((model.Page - 1) * model.CountOnPage)
                .Take(model.CountOnPage)
                .Select(x => _mapper.Map<AreaItemViewModel>(x))
                .ToList();
            int total = query.Count();
            int pages = (int)Math.Ceiling(total / (double)model.CountOnPage);

            return Ok(new AreaSearchResultViewModel()
            {
                CurrentPage = model.Page,
                Pages = pages,
                Total = total,
                Areas = list,
            });
        }
        [HttpGet("mainPage")]
        public IActionResult GetAreas()
        {
            var list = _areasService.Areas
                //.Include(x => x.Products)
                .Where(x => !x.IsDeleted)
                .OrderBy(x => x.Name)
                .Select(x => _mapper.Map<AreaMainItemViewModel>(x))
                .ToList();
            return Ok(list);
        }

        [HttpGet("selector")]
        public IActionResult GetSelector()
        {
            var list = _areasService.Areas
                .Where(x => !x.IsDeleted)
                .OrderBy(x => x.Id)
                .Select(x => _mapper.Map<AreaItemViewModel>(x))
                .ToList();
            return Ok(list);
        }

        [HttpGet("id/{id}")]
        public IActionResult GetCategory(int id)
        {
            var model = _areasService.Areas.SingleOrDefault(x => x.Id == id);
            if (model == null)
                return NotFound();
            return Ok(_mapper.Map<AreaItemViewModel>(model));
        }

        [HttpPost]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> Create([FromBody] AreaCreateViewModel model)
        {
            await _areasService.Create(model);
            return Ok();
        }

        [HttpPut]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> Edit([FromBody] AreaEditViewModel model)
        {
            await _areasService.Update(model);
            return Ok();
        }

        [HttpDelete("{id}")]
 
        public async Task<IActionResult> Delete(int id)
        {
            await _areasService.Delete(id);
            return Ok();
        }
    }
}
