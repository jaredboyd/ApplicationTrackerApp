using ApplicationTrackerApp.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using ApplicationTrackerApp.Models;

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
            var apps = await _context.JobApplications
                .Where(j => j.UserId == userId)
                .OrderByDescending(j => j.DateApplied)
                .ToListAsync();

            return View(apps);
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
