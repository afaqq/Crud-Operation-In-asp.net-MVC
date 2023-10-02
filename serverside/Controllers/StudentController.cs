using serverside.Data;
using serverside.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System;
using System.Data;

namespace serverside.Controllers
{
    public class StudentController : Controller
    {
        private readonly ApplicationContext context;

        public StudentController(ApplicationContext context)
        {
            this.context = context;
        }

        public IActionResult Index()
        {
            var result = context.Students.ToList();
            return View(result);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }


      /*  [AcceptVerbs("Get","Post")]
        public JsonResult MyFunctionAsync(string UserName)
        {
            Student student = context.Students.FirstOrDefault(u=>u.UserName == UserName);
            if (student == null)
                return Json(false);
            else
                return Json(true);
        } */
       

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Student model)
        {
            if (ModelState.IsValid)
            {
                var stud = new Student()
                {
                    UserName = model.UserName,
                    Name = model.Name,
                    City = model.City,
                    State = model.State,
                    Salary = model.Salary,

                };
                context.Students.Add(stud);
                context.SaveChanges();
                TempData["error"] = "Record Saved!";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["message"] = "Empty Field Can't Submit";
                return View(model);
            }

        }
        public IActionResult Delete(int Id)
        {
            var stud = context.Students.SingleOrDefault(e => e.Id == Id);
            context.Students.Remove(stud);
            context.SaveChanges();
            TempData["error"] = "Record Deleted!";
            return RedirectToAction("Index");
        }
        public IActionResult Edit(int Id)
        {
            var stud = context.Students.SingleOrDefault(e => e.Id == Id);
            var result = new Student()
            {  
                UserName = stud.UserName,
                Name = stud.Name,
                City = stud.City,
                State = stud.State,
                Salary = stud.Salary,
            };
            return View(result);
        }

        [HttpPost]
        public IActionResult Edit(Student model)
        {
            var stud = new Student()
            {
                Id = model.Id,
                UserName = model.UserName,
                Name = model.Name,
                City = model.City,
                State = model.State,
                Salary = model.Salary,
            };
            context.Students.Update(stud);
            context.SaveChanges();
            TempData["error"] = "Record Updated!";
            return RedirectToAction("Index");

        }

        // Manage Academic Records
        public IActionResult Manage(int Id)
        {
            var student = context.Students
                .Include(s => s.Academic) 
                .FirstOrDefault(e => e.Id == Id);

            if (student == null)
            {
                
                return NotFound();
            }

            return View(student);
        }



        // Add Academic Record (GET)
        [HttpGet]
        public IActionResult AddAcademicRecord(int studentId)
        {
            var student = context.Students.FirstOrDefault(s => s.Id == studentId);

            if (student == null)
            {
               
                return NotFound();
            }

           
            var academicViewModel = new Academic
            {
                StudentId = studentId
            };

            return View(academicViewModel);
        }

        // Add Academic Record (POST)
        [HttpPost]
        public IActionResult AddAcademicRecord(Academic academicViewModel)
        {
            if (ModelState.IsValid)
            {
                var student = context.Students.FirstOrDefault(s => s.Id == academicViewModel.StudentId);

                if (student != null)
                {
                    var academicRecord = new Academic
                    {
                        Level = academicViewModel.Level,
                        UniName = academicViewModel.UniName,
                        ObtainedMarks = academicViewModel.ObtainedMarks,
                        CGPA = academicViewModel.CGPA,
                        TotalMarks = academicViewModel.TotalMarks,
                        PassingYear = academicViewModel.PassingYear,
                        StudentId = academicViewModel.StudentId
                    };

                    context.Academics.Add(academicRecord);
                    context.SaveChanges();

                    TempData["success"] = "Academic Record added successfully.";

                   
                    var updatedStudent = context.Students
                        .Include(s => s.Academic)
                        .FirstOrDefault(e => e.Id == academicViewModel.StudentId);

                    if (updatedStudent != null)
                    {
                        
                        return PartialView("_AcademicTable", updatedStudent.Academic);
                    }
                }
                else
                {
                    ModelState.AddModelError("StudentId", "Invalid StudentId");
                }
            }

            return View(academicViewModel);
        }


        // Edit Academic Record (GET)
        [HttpGet]
        public IActionResult EditAcademicRecord(int id)
        {
            var academicRecord = context.Academics.FirstOrDefault(a => a.Id == id);

            var result = new Academic()
            {
                Level = academicRecord.Level,
                UniName = academicRecord.UniName,
                ObtainedMarks = academicRecord.ObtainedMarks,
                CGPA = academicRecord.CGPA,
                TotalMarks = academicRecord.TotalMarks,
                PassingYear = academicRecord.PassingYear,
                StudentId = academicRecord.StudentId
            };
            return View(result);

          
        }

        // Edit Academic Record (POST)
        [HttpPost]
        public IActionResult EditAcademicRecord(Academic academicRecord)
        {
            if (ModelState.IsValid)
            {
                
                context.Academics.Update(academicRecord);
                context.SaveChanges();

                TempData["success"] = "Academic Record updated successfully.";
                return RedirectToAction("Manage", new { Id = academicRecord.StudentId });
            }

           
            return View(academicRecord);
        }

        // Delete Academic Record 
        [HttpPost]
        public IActionResult DeleteAcademicRecord(int id, int studentId)
        {
            try
            {
                var academicRecord = context.Academics.FirstOrDefault(a => a.Id == id);

                if (academicRecord != null)
                {
                    context.Academics.Remove(academicRecord);
                    context.SaveChanges();

                    return Json(new { success = true, message = "Academic Record deleted successfully" });
                }
                else
                {
                    return Json(new { success = false, message = "Academic Record not found" });
                }
            }
            catch (Exception ex)
            {
                
                return Json(new { success = false, message = ex.Message });
            }
        }



        [HttpGet]
        public IActionResult GetData()
        {
            var query = context.Students.Select(s => new
            {
                s.Id,
                s.UserName,
                s.Name,
                s.City,
                s.State,
                s.Salary
            });

            var draw = int.Parse(Request.Query["draw"]);
            var start = int.Parse(Request.Query["start"]);
            var length = int.Parse(Request.Query["length"]);
            var searchValue = Request.Query["search[value]"].ToString();

            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(s =>
                    s.UserName.Contains(searchValue) ||
                    s.Name.Contains(searchValue) ||
                    s.City.Contains(searchValue) ||
                    s.State.Contains(searchValue)
                );
            }

            var recordsTotal = query.Count();

            var sortColumn = Request.Query["columns[" + Request.Query["order[0][column]"] + "][data]"].ToString();
            var sortDirection = Request.Query["order[0][dir]"].ToString();

            if (!string.IsNullOrEmpty(sortColumn))
            {
                query = sortDirection == "asc"
                    ? query.OrderByDynamic(sortColumn)
                    : query.OrderByDescendingDynamic(sortColumn);
            }

            var recordsFiltered = query.Count();
            var data = query.Skip(start).Take(length).ToList();

            return Json(new
            {
                draw,
                recordsTotal,
                recordsFiltered,
                data
            });
        }



    }
}
