using Microsoft.AspNetCore.Mvc;
using StudentManagementSystem.Data;
using StudentManagementSystem.Models;

namespace StudentManagementSystem.Controllers
{
    public class StudentController : Controller
    {
        private readonly AppDbContext _context;

        public StudentController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var students = _context.StudentInfos
                                   
                                   .ToList();

            return View(students);
        }

        

        public IActionResult Create()
        {
            ViewBag.Countries = _context.Countries.ToList();
            return View();
        }

        [HttpPost]
        public IActionResult Create(StudentInfo student, IFormFile? ImageFile)
        {
            // Mobile validation
            if (!string.IsNullOrEmpty(student.ContactNo))
            {
                if (student.ContactNo.Length != 10 || !student.ContactNo.All(char.IsDigit))
                    ModelState.AddModelError("ContactNo", "Enter valid 10 digit mobile number.");
            }


            if (ImageFile != null && ImageFile.Length > 0)
            {
                var ext = Path.GetExtension(ImageFile.FileName).ToLower();

                if (ext != ".jpg" && ext != ".jpeg" && ext != ".png")
                    ModelState.AddModelError("", "Only JPG, JPEG, PNG images allowed.");
            }

            if (ModelState.IsValid)
            {
                // upload image only if user selected
                if (ImageFile != null && ImageFile.Length > 0)
                {
                    string fileName = Guid.NewGuid() + Path.GetExtension(ImageFile.FileName);
                    string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        ImageFile.CopyTo(stream);
                    }

                    student.ImagePath = "/images/" + fileName;
                }

                // default active
                student.Status = true;

                _context.StudentInfos.Add(student);
                _context.SaveChanges();

                return RedirectToAction("Index");
            }

            ViewBag.Countries = _context.Countries.ToList();
            return View(student);
        }


        public IActionResult Edit(int id)
        {
            var student = _context.StudentInfos.Find(id);
            if (student == null) return NotFound();

            ViewBag.Countries = _context.Countries.ToList();
            ViewBag.States = _context.States.Where(x => x.CountryId == student.CountryId).ToList();
            ViewBag.Cities = _context.Cities.Where(x => x.StateId == student.StateId).ToList();

            return View(student);
        }

        [HttpPost]
        public IActionResult Edit(StudentInfo student, IFormFile? ImageFile)
        {
            var existing = _context.StudentInfos.Find(student.StudentId);
            if (existing == null) return NotFound();

            // Mobile validation
            if (!string.IsNullOrEmpty(student.ContactNo))
            {
                if (student.ContactNo.Length != 10 || !student.ContactNo.All(char.IsDigit))
                    ModelState.AddModelError("ContactNo", "Enter valid 10 digit mobile number.");
            }

            if (ModelState.IsValid)
            {
                existing.FirstName = student.FirstName;
                existing.MiddleName = student.MiddleName;
                existing.LastName = student.LastName;
                existing.DOB = student.DOB;
                existing.Age = student.Age;
                existing.ContactNo = student.ContactNo;
                existing.Email = student.Email;
                existing.Address = student.Address;
                existing.Pincode = student.Pincode;
                existing.CountryId = student.CountryId;
                existing.StateId = student.StateId;
                existing.CityId = student.CityId;
                existing.Amount = student.Amount;
                existing.Status = student.Status;

                // upload new image only if selected
                if (ImageFile != null && ImageFile.Length > 0)
                {
                    string fileName = Guid.NewGuid() + Path.GetExtension(ImageFile.FileName);
                    string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        ImageFile.CopyTo(stream);
                    }

                    existing.ImagePath = "/images/" + fileName;
                }

                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Countries = _context.Countries.ToList();
            ViewBag.States = _context.States.Where(x => x.CountryId == student.CountryId).ToList();
            ViewBag.Cities = _context.Cities.Where(x => x.StateId == student.StateId).ToList();

            return View(student);
        }


        public IActionResult Delete(int id)
        {
            var student = _context.StudentInfos.Find(id);
            if (student == null) return NotFound();

            student.Status = false;
            _context.SaveChanges();

            return RedirectToAction("Index");
        }



        public JsonResult GetStates(int countryId)
        {
            var states = _context.States
                .Where(x => x.CountryId == countryId)
                .Select(x => new { x.StateId, x.StateName })
                .ToList();

            return Json(states);
        }

        public JsonResult GetCities(int stateId)
        {
            var cities = _context.Cities
                .Where(x => x.StateId == stateId)
                .Select(x => new { x.CityId, x.CityName })
                .ToList();

            return Json(cities);
        }
    }
}
