using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Renting.Data;
using Renting.Models;
using System.Diagnostics;

namespace Renting.Controllers
{

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly RentingContext _context;

       public HomeController(RentingContext context, ILogger<HomeController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [Authorize(Roles ="Admin")]
        public IActionResult Assets()
        {
            var assets = _context.Assets.ToList();
            ViewBag.Categories = assets;
            return View(assets);
        }

        public IActionResult CEA()
        {
            ViewBag.Categories = Enum.GetValues(typeof(Renting.Models.Stanenum)).Cast<Renting.Models.Stanenum>();
            ViewBag.TF = Enum.GetValues(typeof(Renting.Models.TFenum)).Cast<Renting.Models.TFenum>();
            return View(new Asset()); // <-- przekazanie pustego modelu
        }

        [HttpPost]
        public async Task<IActionResult> CEAForm(Asset assetModel)
        {
            if (!User.Identity.IsAuthenticated)
                return Forbid();

            if (!ModelState.IsValid)
                return View(assetModel);

            if (assetModel.Id == 0)
                _context.Assets.Add(assetModel);
            else
                _context.Assets.Update(assetModel);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Assets));
        }

        public IActionResult DelAssets(int id)
        {
            var assetInDb = _context.Assets.SingleOrDefault(asset => asset.Id == id);
            if (assetInDb != null)
            {
                _context.Assets.Remove(assetInDb);
                _context.SaveChanges();
            }
            return RedirectToAction("Assets");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
