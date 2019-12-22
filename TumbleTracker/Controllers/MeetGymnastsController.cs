using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TumbleTracker.Data;
using TumbleTracker.Models;

namespace TumbleTracker.Controllers
{
    public class MeetGymnastsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public MeetGymnastsController(ApplicationDbContext ctx,
                          UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _context = ctx;
        }

        private Task<ApplicationUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);

        // GET: MeetGymnasts
        public async Task<IActionResult> Index()
        {
            var user = await GetCurrentUserAsync();
            var applicationDbContext = _context.MeetGymnasts
                .Include(m => m.Gymnast)
                .Include(m => m.Meet)
                .Where(m => m.Gymnast.UserId == user.Id);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: MeetGymnasts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var meetGymnast = await _context.MeetGymnasts
                .Include(m => m.Gymnast)
                .Include(m => m.Meet)
                .FirstOrDefaultAsync(m => m.MeetGymnastId == id);
            if (meetGymnast == null)
            {
                return NotFound();
            }

            var user = await GetCurrentUserAsync();
            if (meetGymnast.Gymnast.UserId != user.Id)
            {
                //! this should redirect to a "Not Authorized to View this Content" view
                return NotFound();
            }

            return View(meetGymnast);
        }

        // GET: MeetGymnasts/Create
        public IActionResult Create()
        {
            ViewData["GymnastId"] = new SelectList(_context.Gymnasts, "GymnastId", "FirstName");
            ViewData["MeetId"] = new SelectList(_context.Meets, "MeetId", "EventName");
            return View();
        }

        // POST: MeetGymnasts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MeetGymnastId,Group,ScoreBars,ScoreBeam,ScoreFloor,ScoreVault,Notes,Place,GymnastId,MeetId")] MeetGymnast meetGymnast)
        {
            ModelState.Remove("Meet");
            ModelState.Remove("Gymnast");
            if (ModelState.IsValid)
            {
                _context.Add(meetGymnast);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["GymnastId"] = new SelectList(_context.Gymnasts, "GymnastId", "FirstName", meetGymnast.GymnastId);
            ViewData["MeetId"] = new SelectList(_context.Meets, "MeetId", "EventName", meetGymnast.MeetId);
            return View(meetGymnast);
        }

        // GET: MeetGymnasts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var meetGymnast = await _context.MeetGymnasts.FindAsync(id);
            if (meetGymnast == null)
            {
                return NotFound();
            }
            ViewData["GymnastId"] = new SelectList(_context.Gymnasts, "GymnastId", "FirstName", meetGymnast.GymnastId);
            ViewData["MeetId"] = new SelectList(_context.Meets, "MeetId", "EventName", meetGymnast.MeetId);
            return View(meetGymnast);
        }

        // POST: MeetGymnasts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MeetGymnastId,Group,ScoreBars,ScoreBeam,ScoreFloor,ScoreVault,Notes,Place,GymnastId,MeetId")] MeetGymnast meetGymnast)
        {
            if (id != meetGymnast.MeetGymnastId)
            {
                return NotFound();
            }

            ModelState.Remove("Meet");
            ModelState.Remove("Gymnast");
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(meetGymnast);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MeetGymnastExists(meetGymnast.MeetGymnastId))
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
            ViewData["GymnastId"] = new SelectList(_context.Gymnasts, "GymnastId", "FirstName", meetGymnast.GymnastId);
            ViewData["MeetId"] = new SelectList(_context.Meets, "MeetId", "EventName", meetGymnast.MeetId);
            return View(meetGymnast);
        }

        // GET: MeetGymnasts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var meetGymnast = await _context.MeetGymnasts
                .Include(m => m.Gymnast)
                .Include(m => m.Meet)
                .FirstOrDefaultAsync(m => m.MeetGymnastId == id);
            if (meetGymnast == null)
            {
                return NotFound();
            }

            return View(meetGymnast);
        }

        // POST: MeetGymnasts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var meetGymnast = await _context.MeetGymnasts.FindAsync(id);
            _context.MeetGymnasts.Remove(meetGymnast);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MeetGymnastExists(int id)
        {
            return _context.MeetGymnasts.Any(e => e.MeetGymnastId == id);
        }
    }
}
