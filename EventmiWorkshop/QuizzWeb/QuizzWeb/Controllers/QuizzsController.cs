using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuizzWeb.Core.Contracts;
using QuizzWeb.Core.Models;
using QuizzWeb.Data;
using QuizzWeb.Infrastructure.Data.Models;

namespace QuizzWeb.Controllers
{
    public class QuizzsController : Controller
    {
        private readonly IQuizzService quizzService;
        private readonly ILogger logger;
        public QuizzsController(IQuizzService _quizzService, ILogger<QuizzsController> _logger)
        {
            quizzService = _quizzService;
            logger = _logger;
        }

        // GET: Quizzs
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IEnumerable<QuizzModel> model = Enumerable.Empty<QuizzModel>();

            try
            {
                model = await quizzService.GetAllAsync();
            }
            catch (Exception ex)
            {
                logger.LogError("QuizzsController/Index,", ex);
                ViewBag.ErrorMassage = "Unexpected error occurred";
            }

            return View(model);
        }

        // GET: Quizzs/Details/5
        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            QuizzModel model;

            try
            {
                model = await quizzService.GetQuizzAsync(id ?? 0);
                return View(model);
            }
            catch (ArgumentException aex)
            {
                ViewBag.ErrorMessage = aex.Message;
            }
            catch (Exception ex)
            {
                logger.LogError("QuizzsController/Details,", ex);
                ViewBag.ErrorMassage = "Unexpected error occurred";
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Quizzs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Quizzs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name")] QuizzModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                await quizzService.CreateQuizzAsync(model);
            }
            catch (Exception ex)
            {
                logger.LogError("QuizzsController/Create,", ex);
                ViewBag.ErrorMassage = "Unexpected error occurred";
            }
            return RedirectToAction(nameof(Index));
        }
        // GET: Quizzs/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            QuizzModel model;

            try
            {
                model = await quizzService.GetQuizzAsync(id?? 0);

                return View(model);
            }
            catch(ArgumentException ae) 
            {
                ViewBag.ErrorMassage = ae.Message;
            }
            catch (Exception ex)
            {
                logger.LogError("QuizzsController/Edit,", ex);
                ViewBag.ErrorMassage = "Unexpected error occurred";
            }

            return RedirectToAction(nameof(Index));
        }

        // POST: Quizzs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] QuizzModel model)
        {
            if (!ModelState.IsValid) 
            {
                return View(model);
            }

            try
            {
                await quizzService.UpdateAsync(model);

                return RedirectToAction(nameof(Details), new {id = model.Id });
            }
            catch (ArgumentException ae)
            {
                ViewBag.ErrorMassage = ae.Message;
            }
            catch (Exception ex)
            {
                logger.LogError("QuizzsController/Edit,", ex);
                ViewBag.ErrorMassage = "Unexpected error occurred";
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Quizzs/Delete/5
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            QuizzModel model;

            try
            {
                model = await quizzService.GetQuizzAsync(id ?? 0);
                return View(model);
            }
            catch (ArgumentException aex)
            {
                ViewBag.ErrorMessage = aex.Message;
            }
            catch (Exception ex)
            {
                logger.LogError("QuizzsController/Delete,", ex);
                ViewBag.ErrorMassage = "Unexpected error occurred";
            }

            return RedirectToAction(nameof(Index));
        }

        // POST: Quizzs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await quizzService.DeleteAsync(id);
            }
            catch (ArgumentException aex)
            {
                ViewBag.ErrorMessage = aex.Message;
            }
            catch (Exception ex)
            {
                logger.LogError("QuizzsController/Delete,", ex);
                ViewBag.ErrorMassage = "Unexpected error occurred";
            }

            return RedirectToAction(nameof(Index));
        }

    }
}
