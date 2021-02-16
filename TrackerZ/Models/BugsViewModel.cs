using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrackerZ.Models
{
    public class BugsViewModel
    {
        public List<Bugs> Bugs { get; set; }
        public Bugs EditBug { get; set; }
        public List<SelectListItem> BaseCatName { get; set; }
        public List<SelectListItem> StatusOC { get; set; }
        public int StatusIndex { get; set; }
        public List<Category> BaseCategory { get; set; }
        public List<SelectListItem> BaseCatSelectList { get; set; }
        public int BaseCatIndex { get; set; }
        public int BugCounter { get; set; }
        public int ClosedBugs { get; set; }
    }
}
