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

        public IActionResult Index()
        {
            var Bikes = _db.Bikes.Include(m => m.Make).Include(m => m.Model);
            return View(Bikes.ToList());
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


        //[HttpGet]
        //public IActionResult Edit(int id)
        //{
        //    ModelVM.Model = _db.Models.Include(m => m.Make).SingleOrDefault(m => m.Id == id);
        //    ModelVM.Model = _db.Models.Find(id);
        //    if (ModelVM.Model == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(ModelVM);

        //}
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult Edit()
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View(ModelVM);
        //    }
        //    _db.Update(ModelVM.Model);
        //    _db.SaveChanges();
        //    return RedirectToAction(nameof(Index));
        //}
        
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
