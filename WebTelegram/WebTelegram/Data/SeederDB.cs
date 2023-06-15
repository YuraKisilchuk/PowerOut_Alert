using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Linq;
using WebTelegram.Constants;
using WebTelegram.Data.Entities;
using WebTelegram.Data.Entities.Identity;

namespace WebTelegram.Data
{
    public static class SeederDB
    {
        public static void SeedData(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<AppEFContext>();
                var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
                context.Database.Migrate();

                var userManager = scope.ServiceProvider
                    .GetRequiredService<UserManager<UserEntity>>();

                var roleManager = scope.ServiceProvider
                    .GetRequiredService<RoleManager<RoleEntity>>();

                if (!context.Roles.Any())
                {
                    foreach (var role in Roles.All)
                    {
                        var result = roleManager.CreateAsync(new RoleEntity
                        {
                            Name = role
                        }).Result;
                    }
                }

                if (!context.Users.Any())
                {
                    UserEntity user = new()
                    {
                        FirstName = "Юра",
                        LastName = "Admin",
                        Email = "yura@gmail.com",
                        UserName = "yura@gmail.com",
                    };
                    var result = userManager.CreateAsync(user, "123456")
                        .Result;
                    if (result.Succeeded)
                    {
                        result = userManager
                            .AddToRoleAsync(user, Roles.Admin)
                            .Result;
                    }
                }

                if (!context.Areas.Any())
                {
                    var areas = new string[]
                    {
                        "Рівненська",
                        "Волинська"
                    }; 
                    foreach (var name in areas)
                    {
                        var area = new AreaEntity
                        {
                            Name = name
                        };
                        context.Areas.Add(area);
                        context.SaveChanges();
                    }
                }

                if(!context.Cities.Any())
                {
                    var area = context.Areas.SingleOrDefault(x=>x.Name=="Рівненська");
                    if(area != null)
                    {
                        var cities = new CityEntity[]
                        {
                            new CityEntity { Name = "Рівне", AreaId=area.Id},
                            new CityEntity { Name = "Здолбунів", AreaId =area.Id}
                        };
                        context.Cities.AddRange(cities);
                        context.SaveChanges();
                    }
                    area = context.Areas.SingleOrDefault(x => x.Name == "Волинська");
                    if (area != null)
                    {
                        var cities = new CityEntity[]
                        {
                            new CityEntity { Name = "Луцьк", AreaId=area.Id},
                            new CityEntity { Name = "Ковель", AreaId =area.Id},
                            new CityEntity { Name = "Володимир-Волинський", AreaId =area.Id},
                        };
                        context.Cities.AddRange(cities);
                        context.SaveChanges();
                    }
                }

                if(!context.Exclusions.Any())
                {
                    var city = context.Cities.SingleOrDefault(x => x.Name == "Рівне");
                    if (city != null)
                    {
                        var exclusions = new ExclusionEntity[]
                        {
                            new ExclusionEntity { 
                                Name = "вул. Соборна 1-25",
                                BeginExclusion=DateTime.SpecifyKind(new DateTime(2023, 5, 24, 14, 00, 00), DateTimeKind.Utc),
                                EndExclusion=DateTime.SpecifyKind(new DateTime(2023, 5, 24, 18, 00, 00), DateTimeKind.Utc),
                                CityId=city.Id
                            },
                            new ExclusionEntity {
                                Name = "вул. Небесної сотні 2,4,2,3,1",
                                BeginExclusion=DateTime.SpecifyKind(new DateTime(2023, 5, 24, 18, 00, 00).ToUniversalTime(), DateTimeKind.Utc),
                                EndExclusion=DateTime.SpecifyKind(new DateTime(2023, 5, 24, 22, 00, 00).ToUniversalTime(), DateTimeKind.Utc),
                                CityId=city.Id
                            },
                        };
                        context.Exclusions.AddRange(exclusions);
                        context.SaveChanges();
                    }
                }

            }
        }
    }
}
