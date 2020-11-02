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

        //[HttpPost, ActionName("Create")]
        //[ValidateAntiForgeryToken]
        //public IActionResult CreateBike()
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View(BikeVM);
        //    }
        //    _db.Bikes.Add(BikeVM.Bike);

        //    UploadImageIfAvailable();

        //    _db.SaveChanges();

        //    return RedirectToAction(nameof(Index));
        //}
        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]
        public IActionResult CreateBike(Bike bike)
        {
            if (ModelState.IsValid)
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
                ///
                _db.Bikes.Add(bike);
               //UploadImageIfAvailable(bike);
                _db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            
            return View(bike);
        }

        private void UploadImageIfAvailable(Bike bike)
        {
            //Get BikeID we have saved in database            
            var BikeID = bike.Id;

            //Get wwrootPath to save the file on server
            string wwrootPath = _hostingEnvironment.WebRootPath;

            //Get the Uploaded files
            var files = HttpContext.Request.Form.Files;

            //Get the reference of DBSet for the bike we have saved in our database
            var SavedBike = _db.Bikes.Find(BikeID);


            //Upload the file on server and save the path in database if user have submitted file
            if (files.Count != 0)
            {
                //Extract the extension of submitted file
                var Extension = Path.GetExtension(files[0].FileName);

                //Create the relative image path to be saved in database table 
                var RelativeImagePath = Image.BikeImagePath + files + Extension;

                //Create absolute image path to upload the physical file on server
                var AbsImagePath = Path.Combine(wwrootPath, RelativeImagePath);


                //Upload the file on server using Absolute Path
                using (var filestream = new FileStream(AbsImagePath, FileMode.Create))
                {
                    files[0].CopyTo(filestream);
                }

                //Set the path in database
                SavedBike.ImageName = RelativeImagePath;
            }
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
        //public IActionResult Delete(int id)
        //{
        //    var models = _db.Models.Find(id);
        //    if (models == null)
        //    {
        //        return NotFound();
        //    }
        //    _db.Models.Remove(models);
        //    _db.SaveChanges();
        //    return RedirectToAction(nameof(Index));
        //}
    }
}
