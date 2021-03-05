using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrackerZ.Models;

namespace TrackerZ.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _iCategoryRepository;
        public CategoryController(ICategoryRepository categoryRepository)
        {
            _iCategoryRepository = categoryRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Category()
        {
            CategoryViewModel categoryViewModel = new CategoryViewModel()
            {
                BaseCategory = (List<Category>)_iCategoryRepository.GetBaseCategory()
            };
            return View(categoryViewModel);
        }

        public ActionResult AddCategory(string category)
        {
            EditCategory.Add(category);
            return RedirectToAction("Category");
        }
    }
}
