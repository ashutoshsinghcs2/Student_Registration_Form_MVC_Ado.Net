using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using StudentSingleModel.Models;

namespace StudentSingleModel.Controllers
{
    public class HomeController : Controller
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["StudentDbContext"].ConnectionString;
        public ActionResult Index()
        {
            List<Student> students = new List<Student>();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Students";
                SqlDataAdapter da = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                da.Fill(dt);

                foreach (DataRow row in dt.Rows)
                {
                    students.Add(new Student
                    {
                        Id = Convert.ToInt32(row["Id"]),
                        Name = row["Name"].ToString(),
                        DateOfBirth = Convert.ToDateTime(row["DateOfBirth"]),
                        Gender = row["Gender"].ToString(),
                        Hobbies = row["Hobbies"].ToString(),
                        ProfilePicture = row["ProfilePicture"].ToString()
                    });
                }
            }
            return View(students);
        }


        // GET: Student/Create
        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.Hobbies = new List<string> { "Reading", "Travelling", "Playing" };
            return View();
        }

        // POST: Student/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Student student)
        {
            if (ModelState.IsValid)
            {
                var selectedHobbies = student.Hobbies;
                string hobbiesList = string.Join(",", selectedHobbies);
                hobbiesList = hobbiesList.TrimStart(',');

                if (student.ProfilePicFile != null && student.ProfilePicFile.ContentLength > 0)
                {
                    string fileName = Path.GetFileName(student.ProfilePicFile.FileName);
                    // string directoryPath = Server.MapPath("~/Upload");
                    string directoryPath = Path.Combine(Server.MapPath("~"), "Upload");
                    if (!Directory.Exists(directoryPath))
                    {
                        Directory.CreateDirectory(directoryPath);
                    }
                    string path = Path.Combine(directoryPath, fileName);
                    student.ProfilePicFile.SaveAs(path);
                    student.ProfilePicture = fileName;
                }


                // Save student to database
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "INSERT INTO Students (Name, DateOfBirth, Gender, Hobbies, ProfilePicture) VALUES (@Name, @DateOfBirth, @Gender, @Hobbies, @ProfilePicture)";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Name", student.Name);
                    cmd.Parameters.AddWithValue("@DateOfBirth", student.DateOfBirth);
                    cmd.Parameters.AddWithValue("@Gender", student.Gender);
                    cmd.Parameters.AddWithValue("@Hobbies", hobbiesList);
                    cmd.Parameters.AddWithValue("@ProfilePicture", student.ProfilePicture);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }

                return RedirectToAction("Create"); // Redirect to a list or confirmation page
            }
          
            return View(student);
        }
    }
}