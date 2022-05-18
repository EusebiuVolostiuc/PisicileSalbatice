using AcademicInfoServer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace AcademicInfoServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "teacher")]
    public class TeacherController : ControllerBase

    {

        private IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;
        public TeacherController(IConfiguration config,IWebHostEnvironment env)
        {
            _configuration = config;
            _env = env;
        }

        [HttpPut("upload_Photo")]
        public IActionResult upload_Photo()
        {

            string userID = Authentication.AccountController.getUserIDFromRequest(HttpContext.Request);

            if (userID == null)
            {
                return BadRequest("Invalid Token");
            }

            try
            {
                var req = Request.Form;
                var file = req.Files[0];
                String[] extension = file.FileName.Split('.');
                String filename = userID + "." + extension[extension.Length - 1];
                var physicalPath = _env.ContentRootPath + "/Photos/Teachers/" + filename;

                using (var stream = new FileStream(physicalPath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                return new JsonResult("File succesfully uploaded!");

            }

            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }
        }

        [HttpPut]
        public IActionResult Put(Teacher s)
        {


            string userID = Authentication.AccountController.getUserIDFromRequest(HttpContext.Request);

            if (userID == null)
            {
                return BadRequest("Invalid Token");
            }

            int id = Convert.ToInt32(userID);

            string query = @"update Teachers set Name='" + s.Name + "',department='" + s.department + "'," + "type='" + s.type + "' where userID="+ id;


            Console.Write(query);
            DataTable tbl = new DataTable();

            string sqlDataSource = _configuration.GetConnectionString("AcademicInfo");

            SqlDataReader myReader;

            try
            {
                using (SqlConnection myCon = new SqlConnection(sqlDataSource))
                {
                    myCon.Open();
                    using (SqlCommand cmd = new SqlCommand(query, myCon))
                    {
                        myReader = cmd.ExecuteReader();

                        tbl.Load(myReader);

                        myReader.Close();
                        myCon.Close();
                    }

                }
            }

            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }

            

            return new JsonResult("Updated succesfully!");

        }

        [HttpGet("get_Grades")]

        public IActionResult get_Grades()
        {

            string userID = Authentication.AccountController.getUserIDFromRequest(HttpContext.Request);

            if (userID == null)
            {
                return BadRequest("Invalid Token");
            }

            int id = Convert.ToInt32(userID);


            string q = "select * from Grades where teacherID=" + id;

            DataTable dt = new DataTable();

            SqlDataReader dr;

            Console.WriteLine(q);


            string myConn = _configuration.GetConnectionString("AcademicInfo");

            try
            {
                using (SqlConnection conn = new SqlConnection(myConn))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(q, conn))
                    {
                        dr = cmd.ExecuteReader();
                        dt.Load(dr);

                        return new JsonResult(dt);

                    }
                }
            }

            catch (Exception ex)
            { return new JsonResult(ex.Message); }




        }


        [HttpGet("get_Courses")]
        public IActionResult get_Courses()
        {

            string userID = Authentication.AccountController.getUserIDFromRequest(HttpContext.Request);

            if (userID == null)
            {
                return BadRequest("Invalid Token");
            }

            int id = Convert.ToInt32(userID);


            string q = "select * from Courses where teacherID=" + id;

            DataTable dt = new DataTable();

            SqlDataReader dr;

            Console.WriteLine(q);


            string myConn = _configuration.GetConnectionString("AcademicInfo");

            try
            {
                using (SqlConnection conn = new SqlConnection(myConn))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(q, conn))
                    {
                        dr = cmd.ExecuteReader();
                        dt.Load(dr);

                        return new JsonResult(dt);

                    }
                }
            }

            catch (Exception ex)
            { return new JsonResult(ex.Message); }

         


        }



        [HttpGet("get_Teacher")]
        public IActionResult get_Teacher()
        {

            string userID = Authentication.AccountController.getUserIDFromRequest(HttpContext.Request);

            if (userID == null)
            {
                return BadRequest("Invalid Token");
            }

            int id = Convert.ToInt32(userID);


            string q = "select * from teachers where userID=" + id;

            DataTable dt = new DataTable();

            SqlDataReader dr;

            Console.WriteLine(q);


            string myConn = _configuration.GetConnectionString("AcademicInfo");

            try
            {
                using (SqlConnection conn = new SqlConnection(myConn))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(q, conn))
                    {
                        dr = cmd.ExecuteReader();
                        dt.Load(dr);

                        return new JsonResult(dt);

                    }
                }
            }

            catch (Exception ex)
            { return new JsonResult(ex.Message); }

            return Ok("Student Graded!");


        }

        [HttpPost("propose")]
        public IActionResult Propose(Course cs)
        {

            string userID = Authentication.AccountController.getUserIDFromRequest(HttpContext.Request);

            if (userID == null)
            {
                return BadRequest("Invalid Token");
            }

            int id = Convert.ToInt32(userID);


            string qr = "select proposed from teachers where userID=" + id;

            string myConn = _configuration.GetConnectionString("AcademicInfo");

            DataTable tbl = new DataTable();

            SqlDataReader myReader;

            string department;

            try
            {
                using (SqlConnection conn = new SqlConnection(myConn))
                {
                    conn.Open();

                    string q3 = "select * from ProposedOptionals where CourseName='" + cs.CourseName + "'";

                    SqlCommand cmdd = new SqlCommand(q3, conn);

                    myReader = cmdd.ExecuteReader();

                    if (myReader.HasRows)
                        return new JsonResult("The optional is already in the db!");


                    myReader.Close();


                    using (SqlCommand cmd = new SqlCommand(qr, conn))
                    {
                        myReader = cmd.ExecuteReader();

                        if (myReader.HasRows == false)
                            return new JsonResult("Error!");

                        myReader.Read();

                        int no_proposed = myReader.GetInt32(0);

                        if (no_proposed > 1)
                            return new JsonResult("The teacher already proposed 2 optionals!");

                        no_proposed++;

                        myReader.Close();

                        string q2 = "update Teachers set proposed=" + no_proposed + " where userId=" + id;

                        SqlCommand cmd2 = new SqlCommand(q2, conn);

                        cmd2.ExecuteNonQuery();
                    }

                    using (SqlCommand cmd = new SqlCommand("select department from Teachers where userId = " + userID, conn))
                    {
                        myReader = cmd.ExecuteReader();

                        if (myReader.HasRows == false)
                            return new JsonResult("Error!");

                        myReader.Read();

                        department = myReader.GetString(0);

                        myReader.Close();
                    }
                }
            }

            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }


            string query = "insert into ProposedOptionals values ( " + id + ",'" + department + "'," + cs.year + "," + cs.semester + "," + cs.credits + ",'" + cs.CourseName + "')";


            try
            {
                using (SqlConnection myCon = new SqlConnection(myConn))
                {
                    myCon.Open();
                    using (SqlCommand cmd = new SqlCommand(query, myCon))
                    {
                        cmd.ExecuteNonQuery();
                        myCon.Close();
                    }
                }
            }

            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }



            return new JsonResult("Optional added!");



        }

        [HttpGet("get_Proposed")]
        public IActionResult getProposed()
        {
            string userID = Authentication.AccountController.getUserIDFromRequest(HttpContext.Request);

            if (userID == null)
            {
                return BadRequest("Invalid Token");
            }


            string q = "select * from ProposedOptionals where teacherID = " + userID;

            DataTable dt = new DataTable();

            SqlDataReader dr;

            Console.WriteLine(q);


            string myConn = _configuration.GetConnectionString("AcademicInfo");

            try
            {
                using (SqlConnection conn = new SqlConnection(myConn))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(q, conn))
                    {
                        dr = cmd.ExecuteReader();
                        dt.Load(dr);

                        return new JsonResult(dt);

                    }
                }
            }

            catch (Exception ex)
            { return new JsonResult(ex.Message); }

        }

        [HttpGet("getByCourseID/{id}")]
        public IActionResult getByCourseID(int id)
        {
            string q = "select studentID from StudentsCourses where courseID=" + id;

            DataTable dt = new DataTable();

            SqlDataReader dr;

            Console.WriteLine(q);


            string myConn = _configuration.GetConnectionString("AcademicInfo");

            try
            {
                using (SqlConnection conn = new SqlConnection(myConn))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(q, conn))
                    {
                        dr = cmd.ExecuteReader();

                        if (!dr.HasRows)
                            return new JsonResult("The student is not enrolled in any course!");

                        dt.Load(dr);

                        dr.Close();

                        List<int> ls = new List<int>();


                        foreach (DataRow dr2 in dt.Rows)
                        {
                            int studId = Convert.ToInt32(dr2["studentID"]);
                            ls.Add(studId);

                        }

                        string q3 = "select * from students where userID in (";

                        for (int i = 0; i < ls.Count; i++)
                        {
                            q3 = q3 + ls[i] + ",";

                        }

                        StringBuilder sb = new StringBuilder(q3);

                        sb[sb.Length - 1] = ')';

                        q3 = sb.ToString();

                        Console.WriteLine(q3);

                        using (SqlCommand c = new SqlCommand(q3, conn))
                        {
                            dr = c.ExecuteReader();
                            
                            DataTable dt2 = new DataTable();

                            dt2.Load(dr);

                            return new JsonResult(dt2);

                        }

                    }

                }

            }

            catch (Exception ex)
            { return new JsonResult(ex.Message); }



        }

        [HttpPost("grade_Student")]
        public IActionResult grade_Student(Grade g)
        {


            string q = "insert into Grades values (" + g.studentID + "," + g.courseID + "," + g.value + "," + g.weight +")";

            DataTable dt = new DataTable();

            SqlDataReader dr;

            Console.WriteLine(q);


            string myConn = _configuration.GetConnectionString("AcademicInfo");

            try
            {
                using (SqlConnection conn = new SqlConnection(myConn))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(q, conn))
                    {


                        cmd.ExecuteNonQuery();

                    }
                }
            }

            catch (Exception ex)
            { return new JsonResult(ex.Message); }

            return Ok("Student Graded!");
        }

       


}



}
    
