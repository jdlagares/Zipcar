using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication.Data;
using WebApplication.Models;
using WebApplication.Models.CarsViewModels;
using WebApplication.Services;

namespace WebApplication.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CarsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IFileProvider _fileProvider;

        public CarsController(ApplicationDbContext context, IFileProvider fileProvider)
        {
            _context = context;
            _fileProvider = fileProvider;
        }

        // GET: Cars
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Cars.Include(c => c.Parking);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Cars/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var car = await _context.Cars
                .Include(c => c.Parking)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (car == null)
            {
                return NotFound();
            }

            return View(car);
        }

        // GET: Cars/Create
        public IActionResult Create()
        {
            ViewData["ParkingId"] = new SelectList(_context.Parkings, "Id", "Address");
            return View();
        }

        // POST: Cars/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Brand,Year,Model,Capacity,CarCode,PricePerDay,PricePerMonth,PricePerYear,CarType,ParkingId")] Car car)
        {
            if (ModelState.IsValid)
            {
                _context.Add(car);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ParkingId"] = new SelectList(_context.Parkings, "Id", "Address", car.ParkingId);
            return View(car);
        }

        // GET: Cars/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var car = await _context.Cars.FindAsync(id);
            if (car == null)
            {
                return NotFound();
            }
            ViewData["ParkingId"] = new SelectList(_context.Parkings, "Id", "Address", car.ParkingId);
            return View(car);
        }

        // POST: Cars/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Brand,Year,Model,Capacity,CarCode,PricePerDay,PricePerMonth,PricePerYear,CarType,ParkingId")] Car car)
        {
            if (id != car.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(car);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CarExists(car.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ParkingId"] = new SelectList(_context.Parkings, "Id", "Address", car.ParkingId);
            return View(car);
        }

        // GET: Cars/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var car = await _context.Cars
                .Include(c => c.Parking)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (car == null)
            {
                return NotFound();
            }

            return View(car);
        }

        // POST: Cars/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var car = await _context.Cars.FindAsync(id);
            _context.Cars.Remove(car);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Picture(int carId)
        {
            return View(new PictureViewModel() 
            {
                CarId = carId
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Picture(int carId, PictureViewModel model)
        {
            if (carId != model.CarId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var car = await _context.Cars
                    .FirstOrDefaultAsync(m => m.Id == carId);
                if (car == null)
                {
                    return NotFound();
                }

                try
                {
                    _fileProvider.FileDelete(car.Picture);

                    string extension = Path.GetExtension(model.File.FileName);
                    var directory = "/images";
                    var fileName = $"{Guid.NewGuid().ToString()}{extension}";
                    var filePath = $"{directory}/{fileName}";
                    
                    car.Picture = filePath;
                    _fileProvider.ImageFileCreate(directory, fileName, model.File.OpenReadStream());
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Details), new { id = carId });
                }
                catch (Exception)
                {
                    _fileProvider.FileDelete(car.Picture);
                    throw;
                }
            }

            return View(model);
        }

        [AllowAnonymous]
        public IActionResult Search(SearchViewModel m)
        {
            if (m.Start == null || m.End == null)
            {
                m.Start = DateTime.Now.Date.AddDays(7);
                m.End = DateTime.Now.Date.AddDays(16);
            }

            if (m.Start > m.End)
            {
                m.Start = DateTime.Now.Date.AddDays(7);
                m.End = DateTime.Now.Date.AddDays(16);
            }

            var qry = GetAvailableCars(m.Start.Value, m.End.Value);

            if (!string.IsNullOrWhiteSpace(m.Brand))
            {
                qry = qry.Where(p => p.Brand == m.Brand);
            }

            if (!string.IsNullOrWhiteSpace(m.Model))
            {
                qry = qry.Where(p => p.Model == m.Model);
            }

            if (m.Year.HasValue)
            {
                qry = qry.Where(p => p.Year == m.Year);
            }

            ViewBag.Cars = qry;
            ViewBag.Brands = _context.Cars.Select(p => p.Brand).Distinct();
            ViewBag.Capacities = _context.Cars.Select(p => p.Capacity).Distinct();
            ViewBag.Year = _context.Cars.Select(p => p.Year).Distinct();
            ViewBag.Model = _context.Cars.Select(p => p.Model).Distinct();
            return View(m);
        }


        private bool CarExists(int id)
        {
            return _context.Cars.Any(e => e.Id == id);
        }

        private IQueryable<Car> GetAvailableCars(DateTime start, DateTime end)
        {
            return _context.Cars.Include(p => p.Reservations)
                .Where(p => !p.Reservations.Any(r => (r.DateStart <= end) && (r.EstimateDateEnd >= start)));
        }
    }
}
