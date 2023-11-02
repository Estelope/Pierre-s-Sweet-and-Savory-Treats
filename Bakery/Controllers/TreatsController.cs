using Microsoft.AspNetCore.Mvc;
using Bakery.Models; 
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Bakery.Controllers
{
    public class TreatsController : Controller
    {
        private readonly ApplicationDbContext _db;

        public TreatsController(ApplicationDbContext db)
        {
            _db = db;
        }

        public ActionResult Index()
        {
            return View(_db.Treats.ToList());
        }

        public ActionResult Details(int id)
        {
            Treat thisTreat = _db.Treats
                .Include(treat => treat.FlavorTreats)
                .ThenInclude(joinEntity => joinEntity.Flavor)
                .FirstOrDefault(treat => treat.TreatId == id);
            return View(thisTreat);
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
            FlavorTreat joinEntity = _db.FlavorTreats
                .FirstOrDefault(join => join.FlavorId == FlavorId && join.TreatId == treat.TreatId);

            if (joinEntity == null && FlavorId != 0)
            {
                _db.FlavorTreats.Add(new FlavorTreat { FlavorId = FlavorId, TreatId = treat.TreatId });
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