using HR4You.Contexts.Customer;
using HR4You.Contexts.Holiday;
using HR4You.Contexts.HourEntry;
using HR4You.Contexts.Project;
using HR4You.Contexts.Tag;
using HR4You.Contexts.WorkTime;
using HR4You.Model.Base;
using HR4You.Model.Base.Models.Customer;
using HR4You.Model.Base.Models.Holiday;
using HR4You.Model.Base.Models.HourEntry;
using HR4You.Model.Base.Models.Project;
using HR4You.Model.Base.Models.Tag;
using HR4You.Model.Base.Models.WorkTime;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace HR4You.Controllers;

[Route("/api/master-data/seeding")]
[ApiController]
public class SeedingController : ControllerBase
{
    private readonly IServiceProvider _serviceProvider;

    public SeedingController(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    [HttpPost("seed-customer")]
    [SwaggerOperation("SeedCustomer")]
    public async Task<IActionResult> SeedCustomer()
    {
        var ok = await SeedCustomers();
        if (!ok)
        {
            return BadRequest("Error - if you read this good job :3");
        }

        return Ok();
    }

    [HttpPost("seed-project")]
    [SwaggerOperation("SeedProject")]
    public async Task<IActionResult> SeedProject()
    {
        var ok = await SeedProjects();
        if (!ok)
        {
            return BadRequest("Error - if you read this good job :3");
        }

        return Ok();
    }

    [HttpPost("seed-tag")]
    [SwaggerOperation("SeedTag")]
    public async Task<IActionResult> SeedTag()
    {
        var ok = await SeedTags();
        if (!ok)
        {
            return BadRequest("Error - if you read this good job :3");
        }

        return Ok();
    }
    
    [HttpPost("seed-work-time")]
    [SwaggerOperation("SeedWorkTime")]
    public async Task<IActionResult> SeedWorkTime()
    {
        var ok = await SeedWorkTimes();
        if (!ok)
        {
            return BadRequest("Error - if you read this good job :3");
        }

        return Ok();
    }
    
    [HttpPost("seed-holiday")]
    [SwaggerOperation("SeedHoliday")]
    public async Task<IActionResult> SeedHoliday()
    {
        var ok = await SeedHolidays();
        if (!ok)
        {
            return BadRequest("Error - if you read this good job :3");
        }

        return Ok();
    }
    
    [HttpPost("seed-hour-entry")]
    [SwaggerOperation("SeedHourEntry")]
    public async Task<IActionResult> SeedHourEntry()
    {
        var ok = await SeedHourEntries();
        if (!ok)
        {
            return BadRequest("Error - if you read this good job :3");
        }

        return Ok();
    }


    #region SeedMethods

    private async Task<bool> SeedCustomers()
    {
        var customers = new List<Customer>
        {
            new()
            {
                CustomerNumber = 42,
                Name = "MARS",
                Description = null,
                Address = "irgendwos in bruck an der muhr",
                Email = "irgendwasMit@wirMachenKatzenfutter",
                Website = "k i gibs auf",
                PhoneNumber = "123456789"
            },
            new()
            {
                CustomerNumber = 420,
                Name = "Test2",
                Description = "description N2",
                Address = "Erinnerungsstraße 1, 4310 Mauthausen",
                Email = "haraldTitler@gasmail.com",
                Website = "https://www.mauthausen-memorial.org/de",
                PhoneNumber = "07238 22690"
            },
            new()
            {
                CustomerNumber = 69,
                Name = "BadDragon",
                Description = "dont look it up",
                Address = "dont look it up",
                Email = "dont look it up",
                Website = "dont look it up",
                PhoneNumber = "seriously dont look it up!"
            },
            new()
            {
                CustomerNumber = 1234,
                Name = "Internal",
                Description = "Project for company internal use",
                Address = "not looking it up",
                Email = "Alexander.Binder@zh-tech.at",
                Website = "also not looking",
                PhoneNumber = "nope"
            }
        };

        using var scope = _serviceProvider.CreateScope();
        var customerContext = scope.ServiceProvider.GetService<CustomerContext>()!;
        foreach (var customer in customers)
        {
            var result = await customerContext.Create(customer);
            if (result.Error != MasterDataError.None)
            {
                return false;
            }
        }

        return true;
    }

    private async Task<bool> SeedProjects()
    {
        using var scope = _serviceProvider.CreateScope();
        var customerContext = scope.ServiceProvider.GetService<CustomerContext>()!;

        var customerResult = await customerContext.GetAll();
        if (customerResult.Error != MasterDataError.None)
        {
            return false;
        }

        var projects = new List<Project>
        {
            new()
            {
                ProjectNumber = 815,
                CustomerId =
                    customerResult.Entity!.FirstOrDefault(c => c.Name == "MARS")!
                        .Id, //Don't get the Id like this - I only did this because I couldn't be bothered

                Name = "New Production Line",
                State = ProjectState.Open,
                Description = "Some new production line or smth"
            },
            new()
            {
                ProjectNumber = 816,
                CustomerId = customerResult.Entity!.FirstOrDefault(c => c.Name == "MARS")!.Id,
                Name = "Update Line Management",
                State = ProjectState.Closed,
                Description = "Line Management needs updating"
            },
            new()
            {
                ProjectNumber = 420,
                CustomerId = customerResult.Entity!.FirstOrDefault(c => c.Name == "Test2")!.Id,
                Name = "New shower heads",
                State = ProjectState.Open,
                Description = "yeah u know where this is going, i was bored ok"
            },
            new()
            {
                ProjectNumber = 69420,
                CustomerId = customerResult.Entity!.FirstOrDefault(c => c.Name == "BadDragon")!.Id,
                Name = "I was bored on this one as well lel",
                State = ProjectState.Open,
                Description = "smth smth we dont need to know"
            }
        };

        var projectContext = scope.ServiceProvider.GetService<ProjectContext>()!;
        foreach (var project in projects)
        {
            var result = await projectContext.Create(project);
            if (result.Error != MasterDataError.None)
            {
                return false;
            }
        }

        return true;
    }

    private async Task<bool> SeedTags()
    {
        var tags = new List<Tag>
        {
            new()
            {
                Name = "Homeoffice"
            },
            new()
            {
                Name = "IBN"
            },
            new()
            {
                Name = "Word"
            }
        };

        using var scope = _serviceProvider.CreateScope();
        var tagContext = scope.ServiceProvider.GetService<TagContext>()!;
        foreach (var tag in tags)
        {
            var result = await tagContext.Create(tag);
            if (result.Error != MasterDataError.None)
            {
                return false;
            }
        }

        return true;
    }

    private async Task<bool> SeedWorkTimes()
    {
        var workTimes = new List<WorkTime>
        {
            new()
            {
                MinMonHours = 8,
                MinTueHours = 8,
                MinWedHours = 8,
                MinThuHours = 8,
                MinFriHours = 8,
                MinSatHours = 0,
                MinSunHours = 0
            }
        };

        using var scope = _serviceProvider.CreateScope();
        var workTimeContext = scope.ServiceProvider.GetService<WorkTimeContext>()!;
        foreach (var workTime in workTimes)
        {
            var result = await workTimeContext.Create(workTime);
            if (result.Error != MasterDataError.None)
            {
                return false;
            }
        }
        
        return true;
    }

    private async Task<bool> SeedHolidays()
    {
        var holidays = new List<Holiday>
        {
            new()
            {
                Date = DateOnly.FromDateTime(DateTime.Now),
                Name = "heite",
                HalfDay = false
            },
            new()
            {
                Date = DateOnly.FromDateTime(DateTime.Now + TimeSpan.FromDays(1)),
                Name = "murgen",
                HalfDay = true
            }
        };

        using var scope = _serviceProvider.CreateScope();
        var holidayContext = scope.ServiceProvider.GetService<HolidayContext>()!;
        foreach (var holiday in holidays)
        {
            var result = await holidayContext.Create(holiday);
            if (result.Error != MasterDataError.None)
            {
                return false;
            }
        }
        
        return true;
    }

    private async Task<bool> SeedHourEntries()
    {
        using var scope = _serviceProvider.CreateScope();
        var projectContext = scope.ServiceProvider.GetService<ProjectContext>()!;
        var tagContext = scope.ServiceProvider.GetService<TagContext>()!;

        var projectResult = await projectContext.GetAll();
        if (projectResult.Error != MasterDataError.None)
        {
            return false;
        }
        var tagResult = await tagContext.GetAll();
        if (tagResult.Error != MasterDataError.None)
        {
            return false;
        }
        
        var hourEntries = new List<HourEntry>
        {
            new()
            {
                UserId = Guid.NewGuid().ToString(),
                Date = DateOnly.FromDateTime(DateTime.Today),
                StartTime = new TimeOnly(12,30),
                EndTime = new TimeOnly(18,00),
                Duration = (float)5.5,
                Type = ActivityType.Apprentice,
                ProjectId = projectResult.Entity!.FirstOrDefault(p => p.Name == "New Production Line")!.Id,
                TagId = tagResult.Entity!.FirstOrDefault(t => t.Name == "IBN")!.Id,
                Description = "test description",
                IsBillable = true
            },
            new()
            {
                UserId = Guid.NewGuid().ToString(),
                Date = DateOnly.FromDateTime(DateTime.Today + TimeSpan.FromDays(1)),
                StartTime = new TimeOnly(13,00),
                EndTime = new TimeOnly(21,00),
                Duration = 8,
                Type = ActivityType.Planning,
                ProjectId = projectResult.Entity!.FirstOrDefault(p => p.Name == "New shower heads")!.Id,
                TagId = null,
                Description = "don`t ask why there are furnaces on the premise",
                IsBillable = false
            }
        };

        for (var i = 0; i < 1000; i++)
        {
            hourEntries.Add(new HourEntry
            {
                UserId = Guid.NewGuid().ToString(),
                Date = DateOnly.FromDateTime(DateTime.Today),
                StartTime = new TimeOnly(12, 30),
                EndTime = new TimeOnly(18, 00),
                Duration = (float)5.5,
                Type = ActivityType.Planning,
                ProjectId = projectResult.Entity!.FirstOrDefault(p => p.Name == "Update Line Management")!.Id,
                TagId = tagResult.Entity!.FirstOrDefault(t => t.Name == "Word")!.Id,
                Description = "this **** is a repetitive entry",
                IsBillable = true
            });
        }

        var hourEntryContext = scope.ServiceProvider.GetService<HourEntryContext>()!;
        foreach (var hourEntry in hourEntries)
        {
            var result = await hourEntryContext.Create(hourEntry);
            if (result.Error != HourEntryError.None)
            {
                return false;
            }
        }
        
        return true;
    }

    #endregion
}