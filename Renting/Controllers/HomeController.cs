using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Renting.Data;
using Renting.Models;
using System.Diagnostics;
using System.Security.Claims;

namespace Renting.Controllers
{

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly RentingContext _context;

        private readonly UserManager<User> _userManager;


        public HomeController(RentingContext context, ILogger<HomeController> logger, UserManager<User> userManager)
        {
            _context = context;
            _logger = logger;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public IActionResult AdminRentals(string status, DateTime? fromDate, DateTime? toDate, int? assetId)
        {
            var rentalsQuery = _context.Rentals.Include(r => r.Assets).AsQueryable();

            if (!string.IsNullOrEmpty(status) && Enum.TryParse<Statusenum>(status, out var statusEnum))
            {
                rentalsQuery = rentalsQuery.Where(r => r.Status == statusEnum);
            }
            if (fromDate.HasValue)
            {
                rentalsQuery = rentalsQuery.Where(r => r.FromDate >= fromDate.Value);
            }
            if (toDate.HasValue)
            {
                rentalsQuery = rentalsQuery.Where(r => r.ToDate <= toDate.Value);
            }
            if (assetId.HasValue)
            {
                rentalsQuery = rentalsQuery.Where(r => r.AssetId == assetId.Value);
            }

            var rentals = rentalsQuery.ToList();
            var assets = _context.Assets.ToList();
            ViewBag.Assets = assets;

            return View(rentals);
        }

        [Authorize]
        public async Task<IActionResult> UserRentals() { 
            var userId = _userManager.GetUserId(User);
            var rentalsList = await _context.Rentals
                .Include(r => r.Assets)
                .Where(r => r.UserId == userId)
                .ToListAsync();

            return View(rentalsList);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CancelRental(int id)
        {
            var userId = _userManager.GetUserId(User);
            var rental = await _context.Rentals.FirstOrDefaultAsync(r => r.Id == id);

            if (rental == null)
            {
                return NotFound();
            }

            // Tylko w³aœciciel lub admin mo¿e anulowaæ
            bool isAdmin = User.IsInRole("Admin");
            if (rental.UserId != userId && !isAdmin)
            {
                return Forbid();
            }

            if (rental.Status == Statusenum.CheckedOut)
            {
                TempData["Message"] = "Nie mo¿na anulowaæ wynajmu, który jest w trakcie realizacji.";
                return isAdmin
                    ? RedirectToAction(nameof(AdminRentals))
                    : RedirectToAction(nameof(UserRentals));
            }

            if (rental.Status == Statusenum.Returned)
            {
                TempData["Message"] = "Nie mo¿na anulowaæ wynajmu, który zosta³ zakoñczony.";
                return isAdmin
                    ? RedirectToAction(nameof(AdminRentals))
                    : RedirectToAction(nameof(UserRentals));
            }

            if (rental.Status == Statusenum.Cancelled)
            {
                TempData["Message"] = "Wynajem ju¿ zosta³ anulowany.";
                return isAdmin
                    ? RedirectToAction(nameof(AdminRentals))
                    : RedirectToAction(nameof(UserRentals));
            }

            rental.Status = Statusenum.Cancelled;
            await _context.SaveChangesAsync();

            TempData["Message"] = "Wynajem zosta³ anulowany.";
            return isAdmin
                ? RedirectToAction(nameof(AdminRentals))
                : RedirectToAction(nameof(UserRentals));
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Approve(int id)
        {
            var userId = _userManager.GetUserId(User);
            var rental = await _context.Rentals.FirstOrDefaultAsync(r => r.Id == id);

            if (rental == null)
            {
                return NotFound();
            }

            // Tylko w³aœciciel lub admin mo¿e anulowaæ 
            bool isAdmin = User.IsInRole("Admin");
            if (rental.UserId != userId && !isAdmin)
            {
                return Forbid();
            }

            if (rental.Status == Statusenum.CheckedOut)
            {
                TempData["Message"] = "Nie mo¿na zaakceptowaæ wynajmu, który jest w trakcie realizacji.";
                return isAdmin
                    ? RedirectToAction(nameof(AdminRentals))
                    : RedirectToAction(nameof(UserRentals));
            }

            if (rental.Status == Statusenum.Returned)
            {
                TempData["Message"] = "Nie mo¿na zaakceptowaæ wynajmu, który zosta³ zakoñczony.";
                return isAdmin
                    ? RedirectToAction(nameof(AdminRentals))
                    : RedirectToAction(nameof(UserRentals));
            }

            if (rental.Status == Statusenum.Cancelled)
            {
                TempData["Message"] = "Nie mo¿na zaakceptowaæ wynajmu, który zosta³ anulowany.";
                return isAdmin
                    ? RedirectToAction(nameof(AdminRentals))
                    : RedirectToAction(nameof(UserRentals));
            }

            if (rental.Status == Statusenum.Rejected)
            {
                TempData["Message"] = "Nie mo¿na zaakceptowaæ wynajmu, który zosta³ odrzucony.";
                return isAdmin
                    ? RedirectToAction(nameof(AdminRentals))
                    : RedirectToAction(nameof(UserRentals));
            }

            if (rental.Status == Statusenum.Approved)
            {
                TempData["Message"] = "Wynajem ju¿ zosta³ zaakceptowany.";
                return isAdmin
                    ? RedirectToAction(nameof(AdminRentals))
                    : RedirectToAction(nameof(UserRentals));
            }

            rental.Status = Statusenum.Approved;
            await _context.SaveChangesAsync();

            TempData["Message"] = "Wynajem zosta³ zaakceptowany.";
            return isAdmin
                ? RedirectToAction(nameof(AdminRentals))
                : RedirectToAction(nameof(UserRentals));
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Reject(int id)
        {
            var userId = _userManager.GetUserId(User);
            var rental = await _context.Rentals.FirstOrDefaultAsync(r => r.Id == id);

            if (rental == null)
            {
                return NotFound();
            }

            // Tylko w³aœciciel lub admin mo¿e anulowaæ 
            bool isAdmin = User.IsInRole("Admin");
            if (rental.UserId != userId && !isAdmin)
            {
                return Forbid();
            }

            if (rental.Status == Statusenum.CheckedOut)
            {
                TempData["Message"] = "Nie mo¿na odrzuciæ wynajmu, który jest w trakcie realizacji.";
                return isAdmin
                    ? RedirectToAction(nameof(AdminRentals))
                    : RedirectToAction(nameof(UserRentals));
            }

            if (rental.Status == Statusenum.Returned)
            {
                TempData["Message"] = "Nie mo¿na odrzuciæ wynajmu, który zosta³ zakoñczony.";
                return isAdmin
                    ? RedirectToAction(nameof(AdminRentals))
                    : RedirectToAction(nameof(UserRentals));
            }

            if (rental.Status == Statusenum.Cancelled)
            {
                TempData["Message"] = "Nie mo¿na odrzuciæ wynajmu, który zosta³ anulowany.";
                return isAdmin
                    ? RedirectToAction(nameof(AdminRentals))
                    : RedirectToAction(nameof(UserRentals));
            }

            if (rental.Status == Statusenum.Approved)
            {
                TempData["Message"] = "Nie mo¿na odrzuciæ wynajmu, który zosat³ ju¿ akceptowany.";
                return isAdmin
                    ? RedirectToAction(nameof(AdminRentals))
                    : RedirectToAction(nameof(UserRentals));
            }

            if (rental.Status == Statusenum.Rejected)
            {
                TempData["Message"] = "Wynajem ju¿ zosta³ odrzucony.";
                return isAdmin
                    ? RedirectToAction(nameof(AdminRentals))
                    : RedirectToAction(nameof(UserRentals));
            }

            rental.Status = Statusenum.Rejected;
            await _context.SaveChangesAsync();

            TempData["Message"] = "Wynajem zosta³ odrzucony.";
            return isAdmin
                ? RedirectToAction(nameof(AdminRentals))
                : RedirectToAction(nameof(UserRentals));
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CheckOut(int id)
        {
            var userId = _userManager.GetUserId(User);
            var rental = await _context.Rentals.FirstOrDefaultAsync(r => r.Id == id);

            if (rental == null)
            {
                return NotFound();
            }

            bool isAdmin = User.IsInRole("Admin");
            if (rental.UserId != userId && !isAdmin)
            {
                return Forbid();
            }

            if (rental.Status == Statusenum.Rejected)
            {
                TempData["Message"] = "Nie mo¿na realizowaæ wynajmu, który jest odrzucony.";
                return isAdmin
                    ? RedirectToAction(nameof(AdminRentals))
                    : RedirectToAction(nameof(UserRentals));
            }

            if (rental.Status == Statusenum.Returned)
            {
                TempData["Message"] = "Nie mo¿na realizowaæ wynajmu, który zosta³ zakoñczony.";
                return isAdmin
                    ? RedirectToAction(nameof(AdminRentals))
                    : RedirectToAction(nameof(UserRentals));
            }

            if (rental.Status == Statusenum.Cancelled)
            {
                TempData["Message"] = "Nie mo¿na realizowaæ wynajmu, który zosta³ anulowany.";
                return isAdmin
                    ? RedirectToAction(nameof(AdminRentals))
                    : RedirectToAction(nameof(UserRentals));
            }

            if (rental.Status == Statusenum.CheckedOut)
            {
                TempData["Message"] = "Wynajem ju¿ jest realizowany.";
                return isAdmin
                    ? RedirectToAction(nameof(AdminRentals))
                    : RedirectToAction(nameof(UserRentals));
            }

            rental.Status = Statusenum.CheckedOut;
            rental.CheckedOutAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            TempData["Message"] = "Wynajem jest realizowany.";
            return isAdmin
                ? RedirectToAction(nameof(AdminRentals))
                : RedirectToAction(nameof(UserRentals));
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Return(int id, string condition, string damageNotes)
        {
            var userId = _userManager.GetUserId(User);
            var rental = await _context.Rentals
                .Include(r => r.Assets)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (rental == null)
                return NotFound();

            bool isAdmin = User.IsInRole("Admin");
            if (rental.UserId != userId && !isAdmin)
                return Forbid();

            if (rental.Status == Statusenum.Rejected)
            {
                TempData["Message"] = "Nie mo¿na zwróciæ wynajmu, który jest odrzucony.";
                return isAdmin ? RedirectToAction(nameof(AdminRentals)) : RedirectToAction(nameof(UserRentals));
            }
            if (rental.Status == Statusenum.Approved || rental.Status == Statusenum.Cancelled)
            {
                TempData["Message"] = "Nie mo¿na zwróciæ wynajmu, który nie zosta³ wydany.";
                return isAdmin ? RedirectToAction(nameof(AdminRentals)) : RedirectToAction(nameof(UserRentals));
            }
            if (rental.Status == Statusenum.Returned)
            {
                TempData["Message"] = "Wynajem ju¿ jest oddany.";
                return isAdmin ? RedirectToAction(nameof(AdminRentals)) : RedirectToAction(nameof(UserRentals));
            }

            rental.Status = Statusenum.Returned;
            rental.ReturnedAt = DateTime.UtcNow;
            rental.Condition = condition;
            rental.DamageNotes = damageNotes;

            // Aktualizacja stanu assetu
            if (rental.Assets != null && Enum.TryParse<Stanenum>(condition, out var stan))
            {
                // Pobierz asset z kontekstu, aby mieæ pewnoœæ, ¿e jest œledzony
                var asset = await _context.Assets.FirstOrDefaultAsync(a => a.Id == rental.Assets.Id);
                if (asset != null)
                {
                    asset.Condition = stan;
                    _context.Entry(asset).State = EntityState.Modified;
                }
            }

            await _context.SaveChangesAsync();

            TempData["Message"] = "Wynajem jest oddany.";
            return isAdmin ? RedirectToAction(nameof(AdminRentals)) : RedirectToAction(nameof(UserRentals));
        }

        [Authorize(Roles ="Admin")]
        public IActionResult Assets()
        {
            var assets = _context.Assets.ToList();
            ViewBag.Categories = assets;
            return View(assets);
        }

        public IActionResult CreateEditAsset()
        {
            ViewBag.Categories = Enum.GetValues(typeof(Renting.Models.Stanenum)).Cast<Renting.Models.Stanenum>();
            ViewBag.TF = Enum.GetValues(typeof(Renting.Models.TFenum)).Cast<Renting.Models.TFenum>();
            return View(new Asset()); // <-- przekazanie pustego modelu
        }

        [HttpPost]
        public async Task<IActionResult> CreateEditAssetForm(Asset assetModel)
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

        public IActionResult CreateRental(int? id)
        {
            var assets = _context.Assets.ToList();
            ViewBag.Assets = assets
                .Select(a => new SelectListItem
                {
                    Value = a.Id.ToString(),
                    Text = a.Name // lub inna w³aœciwoœæ opisuj¹ca
                })
                .ToList();

            if (id != null)
            {
                var assetInDb = _context.Rentals.SingleOrDefault(asset => asset.Id == id);
                return View(assetInDb);
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateRentalForm(Rental model)
        {
            if (!User.Identity.IsAuthenticated)
                return Forbid();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Forbid();
            model.UserId = userId;  

            if (string.IsNullOrWhiteSpace(model.Notes))
            {
                ModelState.AddModelError("", "Notatka jest wymagana.");
                ViewBag.Assets = _context.Assets
                    .Select(a => new SelectListItem
                    {
                        Value = a.Id.ToString(),
                        Text = a.Name
                    })
                    .ToList();
                return View("CreateRental", model);
            }

            if (model.FromDate > model.ToDate)
            {
                ModelState.AddModelError("", "Data rozpoczêcia nie mo¿e byæ po dacie zakoñczenia.");
                ViewBag.Assets = _context.Assets
                    .Select(a => new SelectListItem
                    {
                        Value = a.Id.ToString(),
                        Text = a.Name
                    })
                    .ToList();
                return View("CreateRental", model);
            }

            // Sprawdzenie kolizji dat dla tego samego AssetId
            bool isCollision = _context.Rentals
                .Any(r =>
                    r.AssetId == model.AssetId &&
                    r.Id != model.Id &&
                    r.FromDate <= model.ToDate &&
                    r.ToDate >= model.FromDate);

            if (isCollision)
            {
                ModelState.AddModelError(string.Empty, "Wybrany przedmiot jest ju¿ wynajêty w podanym okresie.");
                ViewBag.Assets = _context.Assets
                    .Select(a => new SelectListItem
                    {
                        Value = a.Id.ToString(),
                        Text = a.Name
                    })
                    .ToList();
                return View("CreateRental", model);
            }

            if (model.Id == 0)
            {
                model.Status = Statusenum.Pending;
                _context.Rentals.Add(model);
            }
            else
            {
                _context.Rentals.Update(model);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
