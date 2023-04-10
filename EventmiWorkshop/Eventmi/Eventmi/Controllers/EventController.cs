using Eventmi.Core.Contracts;
using Eventmi.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Eventmi.Controllers;

public class EventController : Controller
{
    private readonly IEventService eventService;
    private readonly ILogger logger;

    public EventController(IEventService eventService, ILogger<EventController> logger)
    {
        this.eventService = eventService;
        this.logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        IEnumerable<EventDto> model = Enumerable.Empty<EventDto>();

        try
        {
            model = await eventService.GetAllAsync();
        }
        catch (Exception ex)
        {
            logger.LogError("EventController/Index", ex);
            ViewBag.ErrorMassage = "Възникна непредвидена грешка!";
        }

        return View("All", model);
    }

    [HttpGet]
    public IActionResult Add() 
    {
        var model = new EventDto() 
        {
            Start = DateTime.Today,
            End = DateTime.Today
        };

        return View("Add", model); 
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Add(EventDto model) 
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
            await eventService.AddAsync(model);
        }
        catch (Exception ex)
        {
            logger.LogError("EventController/Add", ex);
            ViewBag.ErrorMassage = "Възникна непредвидена грешка!";
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id) 
    {
        EventDto eventDto;

        try
        {
            await eventService.GetEventAsync(id);
        }
        catch(ArgumentException aex)
        {
            ViewBag.ErrorMassages = aex.Message;
        }
        catch (Exception ex)
        {
            logger.LogError("EventController/Details", ex);
            ViewBag.ErrorMassage = "Възникна непредвидена грешка!";
        }

        return RedirectToAction(nameof(Index));
    }
}
