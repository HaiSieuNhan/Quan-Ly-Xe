using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting.Internal;
using System;
using System.IO;
using System.Linq;
using vroom.Helpers;
using vroom.Views.ViewModels;
using VroomDb;
using VroomDb.Entities;
using cloudscribe.Pagination.Models;
using Microsoft.EntityFrameworkCore.Internal;

namespace vroom.Controllers
{
    [Authorize(Roles = Roles.Admin + "," + Roles.Executive)]
    public class BikeController : Controller
    {
        private readonly VroomDbContext _db;

        private readonly IWebHostEnvironment _hostingEnvironment;
        [BindProperty]
        public BikeViewModel BikeVM { get; set; }
        public BikeController(VroomDbContext db, IWebHostEnvironment hostEnvironment)
        {
            _db = db;
            this._hostingEnvironment = hostEnvironment;
            BikeVM = new BikeViewModel()
            {
                Makes = _db.Makes.ToList(),
                Models = _db.Models.ToList(),
                Bike = new VroomDb.Entities.Bike()
            };

        }
        public IActionResult Index(string searchSorting, string sortOrder, int pageNumber = 1, int pageSize = 2)
        {
            ViewBag.CurrentSortOrder = sortOrder;
            ViewBag.CurrentFilter = searchSorting;
            ViewBag.PriceSortParam = String.IsNullOrEmpty(sortOrder) ? "price_desc" :"";
            int ExcludeRecords = (pageNumber * pageSize) - pageSize;

            var Bikes = from b in _db.Bikes.Include(m => m.Make)
                                 .Include(m => m.Model) select b;
            var BikeCount = Bikes.Count();
            if (!String.IsNullOrEmpty(searchSorting))
            {
                Bikes = Bikes.Where(b => b.Make.Name.Contains(searchSorting));
                BikeCount = Bikes.Count();
            }
            //sorting
            switch (sortOrder)
            {
                case "price_desc":
                    Bikes = Bikes.OrderByDescending(b => b.Price);
                    break;
                default:
                    Bikes = Bikes.OrderBy(b => b.Price);
                    break;
            }
            //paging
            Bikes = Bikes
             .Skip(ExcludeRecords)
                 .Take(pageSize);

            var result = new PagedResult<Bike>
            {
                Data = Bikes.AsNoTracking().ToList(),
                TotalItems = BikeCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };


            return View(result);
        }

        public IActionResult Create()
        {
            return View(BikeVM);
        }

        
        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]
        public IActionResult CreateBike(Bike bike)
        {
            if (!ModelState.IsValid)
            {
                BikeVM.Makes = _db.Makes.ToList();
                BikeVM.Models = _db.Models.ToList();
                return View(BikeVM);
            }
            ///upload images
            string wwwRootPath = _hostingEnvironment.WebRootPath;
            string fileName = Path.GetFileNameWithoutExtension(bike.ImageFile.FileName);
            string extension = Path.GetExtension(bike.ImageFile.FileName);
            bike.ImageName = fileName = fileName + extension;
            string path = Path.Combine(wwwRootPath + "/images/Bike/", fileName);

            using (var fileStream = new FileStream(path, FileMode.Create))
            {
                bike.ImageFile.CopyTo(fileStream);
            }
            ///
            _db.Bikes.Add(bike);
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public IActionResult Edit(int id)
        {
            BikeVM.Bike = _db.Bikes.SingleOrDefault(m => m.Id == id);
            BikeVM.Models = _db.Models.Where(m => m.MakeID == BikeVM.Bike.MakeID);
            if (BikeVM.Bike == null)
            {
                return NotFound();
            }
            return View(BikeVM);

        }
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public IActionResult EditBike(Bike bike)
        {
            if (!ModelState.IsValid)
            {
                BikeVM.Makes = _db.Makes.ToList();
                BikeVM.Models = _db.Models.ToList();
                return View(BikeVM);
            }
            if (bike.ImageFile != null)
            {
                ///
                string wwwRootPath = _hostingEnvironment.WebRootPath;
                string fileName = Path.GetFileNameWithoutExtension(bike.ImageFile.FileName);
                string extension = Path.GetExtension(bike.ImageFile.FileName);
                bike.ImageName = fileName = fileName + extension;
                string path = Path.Combine(wwwRootPath + "/images/Bike/", fileName);

                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    bike.ImageFile.CopyTo(fileStream);
                }
            }
            _db.Bikes.Update(bike);
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            Bike Bike = _db.Bikes.Find(id);
            if (Bike == null)
            {
                return NotFound();
            }
            var imagePath = Path.Combine(_hostingEnvironment.WebRootPath, "/images/Bike/", Bike.ImageName);
            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }
            _db.Bikes.Remove(Bike);
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
