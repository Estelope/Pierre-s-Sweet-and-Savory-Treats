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
using System;
using Bakery.ViewModels;


namespace Bakery.Controllers
{

  public class TreatsController : Controller
  {
    private readonly BakeryContext _db; 
    private readonly UserManager<ApplicationUser> _userManager;

    public TreatsController(UserManager<ApplicationUser> userManager, BakeryContext db)
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
            List<Treat> userTreats = _db.Treats
                .Where(entry => entry.User.Id == currentUser.Id)
                .ToList();
            return View(userTreats);
        }
    }
        List<Treat> allTreats = _db.Treats.ToList();
      return View(allTreats);
}

    [Authorize]
    public ActionResult Create()
    {
      return View();
    }

    [HttpPost]
    public async Task<ActionResult> Create (Treat treat)
    {
      if (!ModelState.IsValid)
      {
        ModelState.AddModelError("", "Please correct the errors and try again.");
        return View(treat);
      }
      else
      {
        string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        ApplicationUser currentUser = await _userManager.FindByIdAsync(userId);
        treat.User = currentUser;
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
    [Authorize]
    public ActionResult AddFlavor(int id)
    {
      Treat thisTreat = _db.Treats.FirstOrDefault(treat => treat.TreatId == id);
      ViewBag.FlavorId = new SelectList(_db.Flavors, "FlavorId", "Type");
      return View(thisTreat);
    }
    [Authorize]
    [HttpPost]
    public ActionResult AddFlavor(Treat treat, int flavorId)
    {
      #nullable enable
      FlavorTreat? joinEntity = _db.FlavorTreats.FirstOrDefault(join => join.FlavorId == treat.TreatId && join.FlavorId == flavorId );
      #nullable disable
      if (flavorId != 0 && joinEntity == null)
      {
        _db.FlavorTreats.Add(new FlavorTreat() { TreatId = treat.TreatId, FlavorId = flavorId });
        _db.SaveChanges();
      }
      return RedirectToAction("Details", new { id = treat.TreatId });
    }

    
    [HttpPost]
    public ActionResult DeleteJoin(int joinId)
    {
      if (!User.Identity.IsAuthenticated)
      {
        ErrorViewModel error = new ErrorViewModel();
        error.ErrorMessage = "You must be logged in to do that.";
        FlavorTreat joinEntry = _db.FlavorTreats.FirstOrDefault(entry => entry.FlavorTreatId == joinId);
        int treatId = joinEntry.TreatId;
        Dictionary<string, object> model = new Dictionary<string, object>();
        model.Add("error", error);
        model.Add("treatId", treatId);
        return View("Error", model);
      }
      else
      {
        FlavorTreat joinEntry = _db.FlavorTreats.FirstOrDefault(entry => entry.FlavorTreatId == joinId);
        _db.FlavorTreats.Remove(joinEntry);
        _db.SaveChanges();
        return RedirectToAction("Details", new { id = joinEntry.TreatId });
      }
    }

    [Authorize]
    public ActionResult Edit (int id)
    {
      Treat model = _db.Treats.FirstOrDefault(treat => treat.TreatId == id);
      return View(model);
    }

    [Authorize]
    [HttpPost]
    public ActionResult Edit(Treat treat)
    {
      _db.Treats.Update(treat);
      _db.SaveChanges();
      return RedirectToAction("Details", new { id = treat.TreatId });
    }

    [Authorize]
    public ActionResult Delete(int id)
    {
      Treat model = _db.Treats.FirstOrDefault(treat => treat.TreatId == id);
      return View(model);
    }

    [Authorize]
    [HttpPost, ActionName("Delete")]
    public ActionResult DeleteConfirmed(int id)
    {
      Treat thisTreat = _db.Treats.FirstOrDefault(treat => treat.TreatId == id);
      _db.Treats.Remove(thisTreat);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }
  }
}