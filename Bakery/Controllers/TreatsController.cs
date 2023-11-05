using Microsoft.AspNetCore.Mvc;
using Bakery.Models; 
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Security.Claims;

namespace Bakery.Controllers
{
    public class TreatsController : Controller
    {
        private readonly BakeryContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public TreatsController(UserManager<ApplicationUser> userManager, BakeryContext db)
        {
            _db = db;
            _userManager = userManager;
        }

        public async Task<ActionResult> Index()
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            ApplicationUser currentUser = await _userManager.FindByIdAsync(userId);
            List<Treat> userTreats = _db.Treats
                          .Where(entry => entry.User.Id == currentUser.Id)
                         // .Include(treat => treat.Name)
                          .ToList();
      return View(userTreats);
        }

    public ActionResult Create()
        {
        return View();
        }

    [HttpPost]
    public async Task<ActionResult> Create(Treat treat)
    {
      if (!ModelState.IsValid)
        {
        ViewBag.FlavorId = new SelectList(_db.Flavors, "FlavorId", "Name");
            return View(treat);
        }
        else
        {
        string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        ApplicationUser currentUser = await _userManager.FindByIdAsync(userId);
        treat.User = currentUser;
    //       treat.FlavorId = /* the selected Flavor's id */;
        _db.Treats.Add(treat);
        _db.SaveChanges();
        return RedirectToAction("Index");
        }
    }


        public ActionResult Details(int id)
        {
            Treat thisTreat = _db.Treats
                .Include(treat => treat.FlavorTreats)
                .ThenInclude(join => join.Flavor)
                .FirstOrDefault(treat => treat.TreatId == id);
            return View(thisTreat);
        }

        public ActionResult Edit(int id)
        {
    Treat thisTreat = _db.Treats.FirstOrDefault(treat => treat.TreatId == id);
      return View(thisTreat);
    }
         [HttpPost]
    public ActionResult Edit(Treat treat)
    {
      _db.Treats.Update(treat);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }
          public ActionResult Delete(int id)
    {
      Treat thisTreat = _db.Treats.FirstOrDefault(treat => treat.TreatId == id);
      return View(thisTreat);
    }


          [HttpPost, ActionName("Delete")]
    public ActionResult DeleteConfirmed(int id)
    {
      Treat thisTreat = _db.Treats.FirstOrDefault(treat => treat.TreatId == id);
      _db.Treats.Remove(thisTreat);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }


          
        public ActionResult AddFlavor(int id)
        {
            Treat thisTreat = _db.Treats.FirstOrDefault(treat => treat.TreatId == id);
            ViewBag.FlavorId = new SelectList(_db.Flavors, "FlavorId", "Name");
            return View(thisTreat);
        }
          
      
    [HttpPost]
    public ActionResult AddFlavor(Treat treat, int FlavorId)
    {
      #nullable enable
      FlavorTreat? joinEntity = _db.FlavorTreats.FirstOrDefault(join => (join.FlavorId == FlavorId && join.TreatId == treat.TreatId));
      #nullable disable
      if (joinEntity == null && FlavorId != 0)
      {
        _db.FlavorTreats.Add(new FlavorTreat() { FlavorId = FlavorId, TreatId = treat.TreatId });
        _db.SaveChanges();
      }
      return RedirectToAction("Details", new { id = treat.TreatId });
    }   

        [HttpPost]
        public ActionResult DeleteFlavor(int joinId)
        {
            FlavorTreat joinEntry = _db.FlavorTreats.FirstOrDefault(entry => entry.FlavorTreatId == joinId);
            _db.FlavorTreats.Remove(joinEntry);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}