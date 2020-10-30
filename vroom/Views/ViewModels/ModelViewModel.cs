using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VroomDb.Entities;

namespace vroom.Views.ViewModels
{
    public class ModelViewModel
    {
        public Model Model { get; set; }
        public IEnumerable<Make> Makes { get; set; }
    }
}
