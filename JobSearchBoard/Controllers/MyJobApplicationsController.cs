using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using JobSearchBoard.Data;
using JobSearchBoard.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace JobSearchBoard.Controllers
{
    public class MyJobApplicationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MyJobApplicationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Authorize]
        // GET: MyJobApplications
        public async Task<IActionResult> Index()
        {
            string currentUserId = User.Identity.GetUserId();
            IdentityUser currentUser = _context.Users.FirstOrDefault(x => x.Id == currentUserId);
            return View(await _context.JobApplications.Include(m=> m.JobPosting).Select(m => m).Where(m => m.User == currentUser).ToListAsync());
        }



        // GET: MyJobApplications/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jobApplication = await _context.JobApplications
                .Include(m => m.JobPosting).FirstOrDefaultAsync(m => m.ID == id);
            if (jobApplication == null)
            {
                return NotFound();
            }

            return View(jobApplication);
        }

        // POST: MyJobApplications/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var jobApplication = await _context.JobApplications.FindAsync(id);
            _context.JobApplications.Remove(jobApplication);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool JobApplicationExists(int id)
        {
            return _context.JobApplications.Any(e => e.ID == id);
        }
    }
}
