using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TumbleTracker.Data;
using TumbleTracker.Models;

namespace TumbleTracker.Controllers
{
    [Authorize]
    public class GymnastsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public GymnastsController(ApplicationDbContext ctx,
                          UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _context = ctx;
        }

        private Task<ApplicationUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);

        // GET: Gymnasts
        public async Task<IActionResult> Index()
        {
            // Get the current user
            var user = await GetCurrentUserAsync();

            var applicationDbContext = _context.Gymnasts.Include(g => g.User).Where(g => g.UserId == user.Id);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Gymnasts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gymnast = await _context.Gymnasts
                .Include(g => g.User)
                .FirstOrDefaultAsync(m => m.GymnastId == id);
            if (gymnast == null)
            {
                return NotFound();
            }

            return View(gymnast);
        }

        // GET: Gymnasts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Gymnasts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("GymnastId,FirstName,LastName,DOB,UserId")] Gymnast gymnast)
        {
            ModelState.Remove("User");
            ModelState.Remove("UserId");
            if (ModelState.IsValid)
            {
                var user = await GetCurrentUserAsync();
                gymnast.UserId = user.Id;
                _context.Add(gymnast);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id", gymnast.UserId);
            return View(gymnast);
        }

        // GET: Gymnasts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gymnast = await _context.Gymnasts.FindAsync(id);
            if (gymnast == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id", gymnast.UserId);
            return View(gymnast);
        }

        // POST: Gymnasts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("GymnastId,FirstName,LastName,DOB,UserId")] Gymnast gymnast)
        {
            if (id != gymnast.GymnastId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(gymnast);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GymnastExists(gymnast.GymnastId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id", gymnast.UserId);
            return View(gymnast);
        }

        // GET: Gymnasts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gymnast = await _context.Gymnasts
                .Include(g => g.User)
                .FirstOrDefaultAsync(m => m.GymnastId == id);
            if (gymnast == null)
            {
                return NotFound();
            }

            return View(gymnast);
        }

        // POST: Gymnasts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var gymnast = await _context.Gymnasts.FindAsync(id);
            _context.Gymnasts.Remove(gymnast);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GymnastExists(int id)
        {
            return _context.Gymnasts.Any(e => e.GymnastId == id);
        }
    }
}
