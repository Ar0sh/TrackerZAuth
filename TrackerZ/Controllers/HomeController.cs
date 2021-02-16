using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using TrackerZ.Models;

namespace TrackerZ.Controllers
{
    public class HomeController : Controller
    {
       private readonly IBugsRepository _iBugsRepository;
        public HomeController(IBugsRepository bugsRepository)
        {
            _iBugsRepository = bugsRepository;
        }
        [HttpPost]
        public ActionResult SubmitBug(string title, string text, string status, string category)
        {
            BugData.SaveData(title, text, status, category);
            return RedirectToAction("Table");
        }
        public ActionResult Remove(Guid id)
        {
            BugData.DeleteData(id);
            return RedirectToAction("Table");
        }
        public IActionResult New()
        {
            BugsViewModel bugsViewModel = new BugsViewModel()
            {
                BaseCategory = (List<Category>)_iBugsRepository.GetBaseCategory()
            };
            return View(bugsViewModel);
        }
        public ActionResult EditTheBug(Guid id, string title, string text, string StatusIndex, string CatIdNr, string oldStatus)
        {
            BugData.EditTheBug(id, title, text, StatusIndex, CatIdNr, oldStatus);
            return RedirectToAction("Table");
        }
        public ActionResult CloseBug(Guid id)
        {
            BugData.CloseBug(id);
            return RedirectToAction("Table");
        }
        public IActionResult Edit(Guid id)
        {
            BugsViewModel bugsViewModel = new BugsViewModel()
            {
                EditBug = _iBugsRepository.GetEditBugs(id),
                BaseCategory = (List<Category>)_iBugsRepository.GetBaseCategory(),
                StatusIndex = _iBugsRepository.GetEditBugs(id).Status == "Open" ? 0 : 1,
                StatusOC = (List<SelectListItem>)_iBugsRepository.GetStatusCategory(),
                BaseCatSelectList = (List<SelectListItem>)_iBugsRepository.GetBaseCatList(),
                BaseCatIndex = 3
            };
            return View(bugsViewModel);
        }
        public IActionResult Index()
        {
            BugsViewModel bugsViewModel = new BugsViewModel()
            {
                BugCounter = _iBugsRepository.CountBugs(true),
                ClosedBugs = _iBugsRepository.CountBugs(false)
            };
            return View(bugsViewModel);
        }
        public IActionResult Table()
        {
            BugsViewModel bugsViewModel = new BugsViewModel()
            {
                Bugs = (List<Bugs>)_iBugsRepository.GetAllBugs()
            };
            return View(bugsViewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
