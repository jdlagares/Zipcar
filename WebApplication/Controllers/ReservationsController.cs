using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication.Data;
using WebApplication.Models;
using WebApplication.Models.ReservationsViewModels;

namespace WebApplication.Controllers
{
    [Authorize]
    public class ReservationsController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public ReservationsController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Reservations
        public async Task<IActionResult> MyReservations()
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null)
            {
                return Unauthorized();
            }

            var reservations = _context.Reservation
                .Include(r => r.Car)
                .Where(r => r.ApplicationUserId == userId);
            return View(await reservations.ToListAsync());
        }

        // GET: Reservations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservation
                .Include(r => r.Transactions)
                .Include(r => r.Reports)
                .Include(r => r.Car.Parking)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        // GET: Reservations/Create
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> Create(int carId)
        {
            var car = await _context.Cars
                .Include(r => r.Parking)
                .FirstOrDefaultAsync(m => m.Id == carId);
            if (car == null)
            {
                return NotFound();
            }

            ViewBag.Car = car;
            return View(new CreateViewModels() { CarId = carId, DateStart = DateTime.Now.Date.AddDays(1), EstimateDateEnd = DateTime.Now.Date.AddDays(2) });
        }

        [Authorize(Roles = "Customer")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int carId, CreateViewModels model)
        {
            var car = await _context.Cars
                .Include(r => r.Parking)
                .FirstOrDefaultAsync(m => m.Id == carId);
            if (car == null)
            {
                return NotFound();
            }

            var userId = _userManager.GetUserId(User);
            if (userId == null)
            {
                return Unauthorized();
            }

            if (!GetAvailableCars(model.DateStart, model.EstimateDateEnd).Any(p => p.Id == carId))
            {
                ModelState.AddModelError("", "Error! el carro no se encuentra disponible para esta fecha");
            }

            switch (model.Plan)
            {
                case Enumerations.EnumPlan.PerMonth:
                    if ((model.EstimateDateEnd - model.DateStart).Duration().TotalDays / 30 < 1)
                    {
                        ModelState.AddModelError("", "Error! Para elegir el plan mensual debes reservar por lo menos 1 mes");
                    }
                    break;
                case Enumerations.EnumPlan.PerYear:
                    if ((model.EstimateDateEnd - model.DateStart).Duration().TotalDays / 360 < 1)
                    {
                        ModelState.AddModelError("", "Error! Para elegir el plan anual debes reservar por lo menos 1 año");
                    }
                    break;
            }

            if (ModelState.IsValid)
            {
                var value = 0m;
                switch (model.Plan)
                {
                    case Enumerations.EnumPlan.PerDay:
                        value = car.PricePerDay * Convert.ToDecimal((model.EstimateDateEnd - model.DateStart).Duration().TotalDays);
                        break;
                    case Enumerations.EnumPlan.PerMonth:
                        value =car.PricePerMonth * Convert.ToDecimal((model.EstimateDateEnd - model.DateStart).Duration().TotalDays / 30);
                        break;
                    case Enumerations.EnumPlan.PerYear:
                        value = car.PricePerYear * Convert.ToDecimal((model.EstimateDateEnd - model.DateStart).Duration().TotalDays / 360);
                        break;
                    default:
                        value = car.PricePerDay;
                        break;
                }

                var reservation = new Reservation()
                {
                    ApplicationUserId = userId,
                    DateStart = model.DateStart,
                    EstimateDateEnd = model.EstimateDateEnd,
                    CarId = carId,
                    Plan = model.Plan,
                    Transactions = new List<Transaction>()
                    {
                        new Transaction() { Concept = "Reserva y pago por uso", Value = value  }
                    }
                };

                _context.Add(reservation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(MyReservations));
            }

            ViewBag.Car = car;
            return View(model);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index(string applicationUserId = null)
        {
            IQueryable<Reservation> reservations = _context.Reservation
                .Include(p => p.Car)
                .Include(p => p.ApplicationUser);

            if (!string.IsNullOrEmpty(applicationUserId))
            {
                reservations = reservations.Where(p => p.ApplicationUserId == applicationUserId);
            }

            return View(await reservations.ToListAsync());
        }

        // GET: Reservations/Details/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DetailsAdmin(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservation
                .Include(r => r.Transactions)
                .Include(r => r.Reports)
                .Include(r => r.Car.Parking)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delivered(int id)
        {
            var reservation = await _context.Reservation
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reservation == null)
            {
                return NotFound();
            }

            if (reservation.Delivered == true)
            {
                ModelState.AddModelError("", "Ya fue entregado");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    reservation.Delivered = true;
                    _context.Update(reservation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReservationExists(reservation.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var reservation = await _context.Reservation
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reservation == null)
            {
                return NotFound();
            }

            return View(new EditViewModel() 
            {
                Id = reservation.Id,
                RealDateEnd = reservation.EstimateDateEnd
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, EditViewModel model)
        {
            var reservation = await _context.Reservation
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reservation == null)
            {
                return NotFound();
            }

            if (reservation.Returned == true)
            {
                ModelState.AddModelError("", "Ya fue recibido");
            }

            if (reservation.DateStart > model.RealDateEnd)
            {
                ModelState.AddModelError("", "La fecha real de entrega no puede ser inferior a la fecha inicial de la reserva");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    reservation.Returned = true;
                    reservation.RealDateEnd = model.RealDateEnd;

                    if ((reservation.EstimateDateEnd - model.RealDateEnd).Duration().TotalHours >= 1)
                    {
                        _context.Add(new Transaction() 
                        {
                            ReservationId = model.Id,
                            Concept = "Retraso en entrega",
                            Value = Convert.ToDecimal((reservation.EstimateDateEnd - model.RealDateEnd).Duration().TotalHours * 10)
                        });
                    }

                    _context.Update(reservation);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReservationExists(reservation.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            return View(model);
        }

        // GET: Reservations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservation
                .Include(r => r.ApplicationUser)
                .Include(r => r.Car)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        // POST: Reservations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var reservation = await _context.Reservation.FindAsync(id);
            _context.Reservation.Remove(reservation);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(MyReservations));
        }

        private bool ReservationExists(int id)
        {
            return _context.Reservation.Any(e => e.Id == id);
        }

        private IQueryable<Car> GetAvailableCars(DateTime start, DateTime end)
        {
            return _context.Cars.Include(p => p.Reservations)
                .Where(p => !p.Reservations.Any(r => (r.DateStart <= end) && (r.EstimateDateEnd >= start)));
        }
    }
}
