using ApplicationTrackerApp.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using ApplicationTrackerApp.Models;
using ApplicationTrackerApp.ViewModels;

namespace ApplicationTrackerApp.Controllers
{
    [Authorize]
    public class JobApplicationsController : Controller
    {
        private readonly AppDbContext _context;

        public JobApplicationsController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            string firstName = _context.Users.FirstOrDefault(x => x.Id == userId)?.FirstName;
            var apps = await _context.JobApplications
                .Where(j => j.UserId == userId)
                .OrderByDescending(j => j.DateApplied)
                .ToListAsync();

            var viewModel = new JobApplicationsViewModel
            {
                Applications = apps,
                FirstName = firstName,
            };

            return View(viewModel);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(JobApplication model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            model.UserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            model.DateApplied = DateTime.Now;

            _context.JobApplications.Add(model);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int id)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            JobApplication model = _context.JobApplications.FirstOrDefault(j => j.UserId == userId && j.Id == id);
            if(model != null)
            {
                return View(model);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(JobApplication model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var existing = await _context.JobApplications.FirstOrDefaultAsync(j => j.Id == model.Id && j.UserId == userId);

            if(existing == null)
            {
                return RedirectToAction("Index");
            }

            existing.Position = model.Position;
            existing.Company = model.Company;
            existing.DateEdited = DateTime.Now;
            existing.Notes = model.Notes;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var app = await _context.JobApplications.FirstOrDefaultAsync(j => j.UserId == userId && j.Id == id);

            if(app == null)
            {
                return NotFound();
            }

            _context.JobApplications.Remove(app);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
