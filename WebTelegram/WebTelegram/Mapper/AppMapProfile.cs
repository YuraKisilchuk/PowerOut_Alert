using AutoMapper;
using System.Globalization;
using WebTelegram.Data.Entities;
using WebTelegram.Models.Areas;
using WebTelegram.Models.Cities;
using WebTelegram.Models.Exclusions;
using WebTelegram.Models.TelegramChats;

namespace WebTelegram.Mapper
{
    public class AppMapProfile : Profile
    {
        public AppMapProfile()
        {
            CreateMap<AreaEntity, AreaItemViewModel>();
            CreateMap<AreaCreateViewModel, AreaEntity>();
            CreateMap<AreaEntity, AreaMainItemViewModel>()
                .ForMember(x => x.CountProducts, opt => opt.MapFrom(x => 0));//opt => opt.MapFrom(x => x.Products.Count));

            CreateMap<CityEntity, CityItemViewModel>()
                .ForMember(x => x.Area, dto => dto.MapFrom(x => x.Area.Name));
            CreateMap<CreateCityViewModel, CityEntity>();
            CreateMap<Telegram.Bot.Types.Chat, TelegramChatEntity>()
                .ForMember(x => x.ChatId, opt => opt.MapFrom(x => x.Id))
                .ForMember(x => x.Id, opt => opt.Ignore());

            CreateMap<ExclusionEntity, ExclusionItemViewModel>()
                .ForMember(x => x.City, dto => dto.MapFrom(x => $"{x.City.Name} ({x.City.Area.Name})"))
                .ForMember(x => x.BeginExclusion, dto => dto.MapFrom(x => x.BeginExclusion.ToString("dd.MM.yyyy HH:mm")))
                .ForMember(x => x.EndExclusion, dto => dto.MapFrom(x => x.EndExclusion.ToString("dd.MM.yyyy HH:mm")));

            var cultureInfo = new CultureInfo("uk-UA");

            CreateMap<CreateExclusionViewModel, ExclusionEntity>()
                .ForMember(x => x.BeginExclusion, opt => opt.MapFrom(x =>
                    DateTime.SpecifyKind(DateTime.Parse(x.BeginExclusion, cultureInfo), DateTimeKind.Utc)))
                .ForMember(x => x.EndExclusion, opt => opt.MapFrom(x =>
                    DateTime.SpecifyKind(DateTime.Parse(x.EndExclusion, cultureInfo), DateTimeKind.Utc)));

            CreateMap<TelegramChatEntity, TelegramChatItemViewModel>();

            CreateMap<CreateTelegramChatViewModel, TelegramChatEntity>();
        }
    }
}
