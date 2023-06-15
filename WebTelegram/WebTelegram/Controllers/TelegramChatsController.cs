using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebTelegram.Abstract;
using WebTelegram.Constants;
using WebTelegram.Models.Exclusions;
using WebTelegram.Models.TelegramChats;

namespace WebTelegram.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = Roles.Admin)]
    public class TelegramChatsController : ControllerBase
    {
        private readonly ITelegramChatsService _telegramChatsService;
        private readonly IMapper _mapper;

        public TelegramChatsController(ITelegramChatsService telegramChatsService, IMapper mapper)
        {
            _telegramChatsService = telegramChatsService;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetList([FromQuery] TelegramChatSearchViewModel model)
        {
            var query = _telegramChatsService.TelegramChats
                .Include(x => x.Subscriptions)
                .Where(x => !x.IsDeleted)
                .AsQueryable();

            if (!string.IsNullOrEmpty(model.Search))
            {
                var searches = model.Search.Split(' ');
                foreach (var sear in searches)
                {
                    query = query.Where(x => x.Username.ToLower().Contains(sear.ToLower()));
                }
            }


            var list = query
                .Skip((model.Page - 1) * model.CountOnPage)
                .Take(model.CountOnPage)
                .Select(x => _mapper.Map<TelegramChatItemViewModel>(x))
                .ToList();

            int total = query.Count();
            int pages = (int)Math.Ceiling(total / (double)model.CountOnPage);

            return Ok(new TelegramChatSearchResultViewModel()
            {
                CurrentPage = model.Page,
                Pages = pages,
                Total = total,
                TelegramChats = list,
            });
        }


        [HttpGet("most-buys")]
        public IActionResult GetList(int count)
        {
            var list = _telegramChatsService.TelegramChats
                .Where(x => !x.IsDeleted)
                .OrderBy(x => x.Id)
                .Take(count)
                .Select(x => _mapper.Map<TelegramChatItemViewModel>(x))
                .ToList();

            return Ok(list);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateTelegramChatViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var id = await _telegramChatsService.Create(model);
            return Ok();
        }
        [HttpGet("id/{id}")]
        public IActionResult GetItem(int id)
        {
            var model = _telegramChatsService.TelegramChats
                
                .SingleOrDefault(x => x.Id == id);
            if (model == null)
                return NotFound();
            return Ok(_mapper.Map<TelegramChatItemViewModel>(model));
        }

        [HttpPut]
        public async Task<IActionResult> Edit([FromBody] EditTelegramChatViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            await _telegramChatsService.Update(model);

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _telegramChatsService.Delete(id);
            return Ok();
        }
    }
}
