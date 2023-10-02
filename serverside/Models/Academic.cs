using System.ComponentModel.DataAnnotations;

namespace serverside.Models
{
    public class Academic
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Academic Level is required")]
        public string Level { get; set; }

        [Required(ErrorMessage = "University Name is required")]
        public string UniName { get; set; }


        [Required(ErrorMessage = "Passing Year is required")]
        public int PassingYear { get; set; }

        [Required(ErrorMessage = "Marks is required")]
        [Range(0, int.MaxValue, ErrorMessage = "Obtained Marks must be between 0 and Total Marks")]
        public int ObtainedMarks { get; set; }

        [Required(ErrorMessage = "Obtained Marks in % is required")]
        [Range(0, 100, ErrorMessage = "Obtained Marks in % must be between 0 and 100%")]
        public decimal CGPA { get; set; }

        [Required(ErrorMessage = "Marks is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Total Marks must be greater than 0")]
        public int TotalMarks { get; set; }

        // Foreign key to Student
        public int StudentId { get; set; }

       
        public Student Student { get; set; }

    }
}
