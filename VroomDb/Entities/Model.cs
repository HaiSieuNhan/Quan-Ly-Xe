using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace VroomDb.Entities
{
    public class Model
    {
        public int Id { get; set; }
        [Required]
        [StringLength(255)]
        [DisplayName("Model")]
        public string Name { get; set; }

        public Make Make { get; set; }

        public int MakeID { get; set; }
    }
}
