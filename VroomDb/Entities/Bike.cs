using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace VroomDb.Entities
{
    public class Bike
    {

        public int Id { get; set; }
        public Make Make { get; set; }
        public int MakeID { get; set; }

        public Model Model { get; set; }
        public int ModelID { get; set; }
       

        [Required(ErrorMessage = "Provide Year")]

        public int Year { get; set; }

        [Required(ErrorMessage = "Enter Mileage")]
        [Range(1, int.MaxValue, ErrorMessage = "Not with in the valid mileage range")]
        public int Mileage { get; set; }


        public string Features { get; set; }

        [Required(ErrorMessage = "Provide Seller Name")]
        [StringLength(50)]
        public string SellerName { get; set; }

        [EmailAddress(ErrorMessage = "Invalid Email ID")]
        [StringLength(50)]
        public string SellerEmail { get; set; }

        [Required(ErrorMessage = "Provide Phone No.")]
        [Phone]
        [StringLength(15)]
        public string SellerPhone { get; set; }

        [Required(ErrorMessage = "Provide Price")]
        [Range(1, 999999999, ErrorMessage = "Not with in the valid price range")]
        public int Price { get; set; }

        [Required]
        [StringLength(10)]
        [RegularExpression("^[a-zA-Z]*$", ErrorMessage = "Select Currency")]
        public string Currency { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string ImageName { get; set; }

        [NotMapped]
        [DisplayName("Upload File")]
        public IFormFile ImageFile { get; set; }

    }
}
