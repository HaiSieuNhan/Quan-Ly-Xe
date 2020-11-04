using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using vroom.Controllers.Models;
using vroom.Controllers.Resources;
using vroom.Helpers;
using vroom.Views.ViewModels;
using VroomDb;
using VroomDb.Entities;

namespace vroom.Controllers
{
    [Authorize(Roles = Roles.Admin + "," + Roles.Executive)]
    public class ModelController : Controller
    {
        private readonly VroomDbContext _db;
        private readonly IMapper _mapper;

        [BindProperty]
        public ModelViewModel ModelVM { get; set; }
        public ModelController(VroomDbContext db,IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
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
        [HttpGet]
        public IActionResult Edit(int id)
        {
            //ModelVM.Model = _db.Models.Include(m => m.Make).SingleOrDefault(m => m.Id == id);
            ModelVM.Model = _db.Models.Find(id);
            if (ModelVM.Model == null)
            {
                return NotFound();
            }
            return View(ModelVM);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit()
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

        [AllowAnonymous]
        [HttpGet("api/models/{MakeID}")]
        public IEnumerable<Model> Models(int MakeID)
        {
            return _db.Models.ToList()
                    .Where(m=>m.MakeID == MakeID);
        }
        [AllowAnonymous]
        [HttpGet("api/models")]
        public IEnumerable<ModelResources> Models()
        {
            var models =  _db.Models.ToList();
             return _mapper.Map<List<Model>, List<ModelResources>>(models);
        }
    }
}
