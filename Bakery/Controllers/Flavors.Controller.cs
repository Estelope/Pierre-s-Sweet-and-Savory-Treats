using Microsoft.AspNetCore.Mvc;
using Bakery.Models; 
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Security.Claims;

namespace Bakery.Controllers
{
    public class FlavorsController : Controller
    {
        private readonly BakeryContext _db;
        private readonly UserManager<ApplicationUser> _userManager; 

        public FlavorsController(UserManager<ApplicationUser> userManager, BakeryContext db)
        {
        _userManager = userManager;
        _db = db;
        }

         public async Task<ActionResult> Index()
        {
    if (User.Identity.IsAuthenticated)
    {
        string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        ApplicationUser currentUser = await _userManager.FindByIdAsync(userId);

        if (currentUser != null)
        {
            List<Flavor> userFlavors = _db.Flavors
                .Where(entry => entry.User.Id == currentUser.Id)
                .ToList();
            return View(userFlavors);
        }
    }
        List<Flavor> allFlavors = _db.Flavors.ToList();
      return View(allFlavors);
        }

        public ActionResult Create()
        {
        return View();
        }

        [Authorize]
        [HttpPost]
    public async Task<ActionResult> Create(Flavor flavor)
        {
      if (!ModelState.IsValid)
        {
            return View(flavor);
        }
        else
        {
        string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        ApplicationUser currentUser = await _userManager.FindByIdAsync(userId);
        flavor.User = currentUser;
    //       treat.FlavorId = /* the selected Flavor's id */;
        _db.Flavors.Add(flavor);
        _db.SaveChanges();
        return RedirectToAction("Index");
        }
         }


        public ActionResult Details(int id)
        {
            Flavor thisFlavor = _db.Flavors
                .Include(flavor => flavor.FlavorTreats)
                .ThenInclude(join => join.Treat)
                .FirstOrDefault(flavor => flavor.FlavorId == id);
            return View(thisFlavor);
        }
        [Authorize]
         public ActionResult Edit(int id)
        {
        Flavor thisFlavor = _db.Flavors.FirstOrDefault(flavor => flavor.FlavorId == id);
        return View(thisFlavor);
        }
        [Authorize]
         [HttpPost]
            public ActionResult Edit(Flavor flavor)
        {
            _db.Flavors.Update(flavor);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
        [Authorize]
            public ActionResult Delete(int id)
        {
        Flavor thisFlavor = _db.Flavors.FirstOrDefault(flavor => flavor.FlavorId == id);
        return View(thisFlavor);
        }

        [Authorize]
        [HttpPost, ActionName("Delete")]
    public ActionResult DeleteConfirmed(int id)
        {
        Flavor thisFlavor = _db.Flavors.FirstOrDefault(flavor => flavor.FlavorId == id);
        _db.Flavors.Remove(thisFlavor);
        _db.SaveChanges();
        return RedirectToAction("Index");
        }


        public ActionResult AddTreat(int id)
        {
            Flavor thisFlavor = _db.Flavors.FirstOrDefault(flavor => flavor.FlavorId == id);
            ViewBag.TreatId = new SelectList(_db.Treats, "TreatId", "Name");
            return View(thisFlavor);
        }

        [HttpPost]
        public ActionResult AddTreat(Flavor flavor, int TreatId)
        {
            FlavorTreat joinEntity = _db.FlavorTreats
                .FirstOrDefault(join => join.TreatId == TreatId && join.FlavorId == flavor.FlavorId);

            if (joinEntity == null && TreatId != 0)
            {
                _db.FlavorTreats.Add(new FlavorTreat { TreatId = TreatId, FlavorId = flavor.FlavorId });
                _db.SaveChanges();
            }
            return RedirectToAction("Details", new { id = flavor.FlavorId });
        }

        [HttpPost]
        public ActionResult DeleteTreat(int joinId)
        {
            FlavorTreat joinEntry = _db.FlavorTreats.FirstOrDefault(entry => entry.FlavorTreatId == joinId);
            _db.FlavorTreats.Remove(joinEntry);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}