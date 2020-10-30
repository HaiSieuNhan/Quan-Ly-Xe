using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using vroom.Controllers.Models;
using vroom.Views.ViewModels;
using VroomDb;
using VroomDb.Entities;

namespace vroom.Controllers
{
    public class ModelController : Controller
    {
        private readonly VroomDbContext _db;
        [BindProperty]
        public ModelViewModel ModelVM { get; set; }
        public ModelController(VroomDbContext db)
        {
            _db = db;
            ModelVM = new ModelViewModel()
            {
                Makes = _db.Makes.ToList(),
                Model = new VroomDb.Entities.Model()
            };

        }

        public IActionResult Index1()
        {
            var model = _db.Models.Include(m => m.Make);
            return View(model.ToList());
        }
        public IActionResult Index()
        {
            // cách của a Sỹ
            var models = _db.Models
                        .Select(s => new ModelsViewModel
                        {
                            Id = s.Id,
                            Name = s.Name,
                            MakeName = s.Make.Name
                        }).ToList();
            return View(models);
        }

        public IActionResult Create()
        {
            return View(ModelVM);
        }

        [HttpPost, ActionName("Create")]
        // [ValidateAntiForgeryToken]
        public IActionResult CreateModel()
        {
            if (!ModelState.IsValid)
            {
                return View(ModelVM);
            }
            _db.Models.Add(ModelVM.Model);
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int id)
        {
            ModelVM.Model = _db.Models.Include(m => m.Make).SingleOrDefault(m => m.Id == id);
            if (ModelVM.Model == null) {
                return NotFound();
             }
            return View(ModelVM);
        }
        [HttpPost,ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public IActionResult EditPost(int id)
        {
            if (!ModelState.IsValid)
            {

                return View(ModelVM);
            }
            _db.Update(ModelVM.Model);
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Delete(int id)
        {
            var models = _db.Models.Find(id);
            if (models == null)
            {
                return NotFound();
            }
            _db.Models.Remove(models);
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
