using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrackerZ.Models
{
    public interface IBugsRepository
    {
        IEnumerable<Bugs> GetAllBugs();
        Bugs GetEditBugs(Guid id);
        IEnumerable<Category> GetBaseCategory();
        IEnumerable<SelectListItem> GetStatusCategory();
        IEnumerable<SelectListItem> GetBaseCatList();
        int CountBugs(bool all);
    }
}
