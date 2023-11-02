using Microsoft.AspNetCore.Mvc;
using Bakery.Models; 
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Bakery.Controllers
{
    public class FlavorsController : Controller
    {
        private readonly ApplicationDbContext _db;

        public FlavorsController(ApplicationDbContext db)
        {
            _db = db;
        }

        public ActionResult Index()
        {
            return View(_db.Flavors.ToList());
        }

        public ActionResult Details(int id)
        {
            Flavor thisFlavor = _db.Flavors
                .Include(flavor => flavor.FlavorTreats)
                .ThenInclude(joinEntity => joinEntity.Treat)
                .FirstOrDefault(flavor => flavor.FlavorId == id);
            return View(thisFlavor);
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