using Microsoft.AspNetCore.Mvc;
using serverside.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace serverside.Models
{

    public class Student
    {

        [Key]
        public int Id { get; set; }


        [Required(ErrorMessage = "User Name is required")]
        [RegularExpression(@"^.*@.*$", ErrorMessage = "User Name must contain the '@' symbol")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
        public string City { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string State { get; set; }

        [Required(ErrorMessage = "Salary is required")]
        [Range(1000, double.MaxValue, ErrorMessage = "Salary must be greater than 1000")]
        public decimal Salary { get; set; }

        // Navigation property for Academic
        public List<Academic> Academic { get; set; }

    }
}