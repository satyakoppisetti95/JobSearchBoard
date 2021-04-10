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
    public class JobPostingsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public JobPostingsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: JobPostings
        [Authorize]
        public async Task<IActionResult> Index()
        {
            string currentUserId = User.Identity.GetUserId();
            IdentityUser currentUser = _context.Users.FirstOrDefault(x => x.Id == currentUserId);
            List<JobApplication> jobApplicationsOfUser = _context.JobApplications.Select(m => m).Where(m => m.User == currentUser).ToList();
            List<int> jobsApplicationIdsOfUser = new List<int>();
            foreach(var application in jobApplicationsOfUser)
            {
                jobsApplicationIdsOfUser.Add(application.JobPostingID);
            }
            ViewBag.applied_job_ids = jobsApplicationIdsOfUser;
            return View(await _context.JobPostings.ToListAsync());
        }

        // GET: JobPostings/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jobPosting = await _context.JobPostings
                .FirstOrDefaultAsync(m => m.ID == id);
            if (jobPosting == null)
            {
                return NotFound();
            }

            return View(jobPosting);
        }

        // GET: JobPostings/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: JobPostings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("ID,Title,Description,Salary,Company")] JobPosting jobPosting)
        {
            if (ModelState.IsValid)
            {
                string currentUserId = User.Identity.GetUserId();
                IdentityUser currentUser = _context.Users.FirstOrDefault(x => x.Id == currentUserId);
                jobPosting.User = currentUser;

                _context.Add(jobPosting);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(jobPosting);
        }

        

        public async Task<IActionResult> Apply([FromQuery(Name = "JobID")] int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jobPosting = await _context.JobPostings
                .FirstOrDefaultAsync(m => m.ID == id);
            if (jobPosting == null)
            {
                return NotFound();
            }


            string currentUserId = User.Identity.GetUserId();
            IdentityUser currentUser = _context.Users.FirstOrDefault(x => x.Id == currentUserId);
            bool application_exists = _context.JobApplications.Select(m => m).Where(m => m.JobPostingID == id && m.User == currentUser).ToList().Count > 0;
            ViewBag.application_exists = application_exists;

            List<JobApplication> applcations_of_user = _context.JobApplications.Select(m => m).Where(m => m.JobPostingID==id && m.User == currentUser).ToList();
            ViewBag.applications = applcations_of_user;
            ViewBag.JobPostingID = id;
            ViewBag.JobPostingTitle = jobPosting.Title;

            return View();

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Apply([Bind("ID,JobPostingID,AppliedDateTime")] JobApplication jobApplication)
        {
            if (ModelState.IsValid)
            {
                string currentUserId = User.Identity.GetUserId();
                IdentityUser currentUser = _context.Users.FirstOrDefault(x => x.Id == currentUserId);
                jobApplication.User = currentUser;
                jobApplication.AppliedDateTime = DateTime.Now;

                _context.Add(jobApplication);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View();
        }



        private bool JobPostingExists(int id)
        {
            return _context.JobPostings.Any(e => e.ID == id);
        }
    }
}
