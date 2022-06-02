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

        [HttpPost("approve")]
        public IActionResult approve(ProposedOptional po)
        {
            string userID = Authentication.AccountController.getUserIDFromRequest(HttpContext.Request);
            SqlDataReader dreader;

            if (userID == null)
            {
                return BadRequest("Invalid Token");
            }


            string q = "select type from Teachers where userID = " + userID;



            Console.WriteLine(q);


            string myConn = _configuration.GetConnectionString("AcademicInfo");

            try
            {
                using (SqlConnection conn = new SqlConnection(myConn))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(q, conn))
                    {
                        dreader = cmd.ExecuteReader();

                        if (dreader.HasRows == false)
                            return BadRequest("An error occured!");

                        dreader.Read();

                        String type = dreader.GetString(0);

                        if (!type.Equals("chief"))
                            return Unauthorized("Only chief teachers can approve optionals!");

                    }

                    dreader.Close();
                    conn.Close();
                }
            }


            catch (Exception ex)
            { return new JsonResult(ex.Message); }


            string del = @"delete from ProposedOptionals where CourseName='" + po.CourseName + "'";

            string insert = @"insert into Courses values ( " + userID + ",'" + po.department + "'," + po.year + "," + po.semester + "," + po.credits + ",'o'" + "," + "'" + po.CourseName + "'" +")";

            Console.WriteLine(del);
            Console.WriteLine(insert);


            try
            {
                using (SqlConnection conn = new SqlConnection(myConn))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(del, conn))
                    {
                        cmd.ExecuteNonQuery();

                    }

                    conn.Close();
                }
            }


            catch (Exception ex)
            { return new JsonResult(ex.Message); }

            try
            {
                using (SqlConnection conn = new SqlConnection(myConn))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(insert, conn))
                    {
                        cmd.ExecuteNonQuery();

                    }

                    conn.Close();
                }
            }


            catch (Exception ex)
            { return new JsonResult(ex.Message); }

            return new JsonResult("Course succesfully approved");


        }

        [HttpGet("get_approvedOptionals")]

        public IActionResult get_approvedOptionals()
        {

            string userID = Authentication.AccountController.getUserIDFromRequest(HttpContext.Request);
            SqlDataReader dreader;

            if (userID == null)
            {
                return BadRequest("Invalid Token");
            }


            string q = "select type from Teachers where userID = " + userID;



            Console.WriteLine(q);


            string myConn = _configuration.GetConnectionString("AcademicInfo");

            try
            {
                using (SqlConnection conn = new SqlConnection(myConn))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(q, conn))
                    {
                        dreader = cmd.ExecuteReader();

                        if (dreader.HasRows == false)
                            return BadRequest("An error occured!");

                        dreader.Read();

                        String type = dreader.GetString(0);

                        if (!type.Equals("chief"))
                            return Unauthorized("Only chief teachers can see all the optionals!");

                    }

                    dreader.Close();
                    conn.Close();
                }
            }


            catch (Exception ex)
            { return new JsonResult(ex.Message); }

            string query = "select * from ProposedOptionals where maxStudents>0";

            SqlDataReader myReader;

            DataTable tbl = new DataTable();

            string con_string = _configuration.GetConnectionString("AcademicInfo");

            List<int> ls = new List<int>();

            try
            {
                using (SqlConnection myCon = new SqlConnection(con_string))
                {
                    myCon.Open();
                    using (SqlCommand cmd = new SqlCommand(query, myCon))
                    {
                        myReader = cmd.ExecuteReader();

                        if (myReader.HasRows == false)
                            return BadRequest("There are no optionals in the DataBase!");

                        tbl.Load(myReader);


                    }
                }


            }

            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }


            try
            {
                using (SqlConnection myCon = new SqlConnection(con_string))
                {
                    myCon.Open();

                    tbl.Columns.Add("TeacherName");

                    foreach (DataRow dr in tbl.Rows)
                    {
                        q = "select Name from Teachers where userID=" + Convert.ToInt32(dr["teacherID"]);

                        using (SqlCommand cmd = new SqlCommand(q, myCon))
                        {
                            myReader = cmd.ExecuteReader();

                            myReader.Read();

                            dr["TeacherName"] = myReader.GetString(0);
                        }

                        myReader.Close();
                    }

                    myReader.Close();
                    myCon.Close();
                }
            }



            catch (Exception ex)
            { return BadRequest(ex.Message); }




            return new JsonResult(tbl);
        }


        [HttpGet("get_Optionals")]

        public IActionResult get_Optionals()
        {

            string userID = Authentication.AccountController.getUserIDFromRequest(HttpContext.Request);
            SqlDataReader dreader;

            if (userID == null)
            {
                return BadRequest("Invalid Token");
            }


            string q = "select type from Teachers where userID = " + userID;

         

            Console.WriteLine(q);


            string myConn = _configuration.GetConnectionString("AcademicInfo");

            try
            {
                using (SqlConnection conn = new SqlConnection(myConn))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(q, conn))
                    {
                        dreader = cmd.ExecuteReader();

                        if (dreader.HasRows == false)
                            return BadRequest("An error occured!");

                        dreader.Read();

                        String type = dreader.GetString(0);

                        if (!type.Equals("chief"))
                            return Unauthorized("Only chief teachers can see all the optionals!");

                    }

                    dreader.Close();
                    conn.Close();
                }
            }


            catch (Exception ex)
            { return new JsonResult(ex.Message); }

            string query = "select * from ProposedOptionals";

            SqlDataReader myReader;

            DataTable tbl = new DataTable();

            string con_string = _configuration.GetConnectionString("AcademicInfo");

            List<int> ls = new List<int>();

            try
            {
                using (SqlConnection myCon = new SqlConnection(con_string))
                {
                    myCon.Open();
                    using (SqlCommand cmd = new SqlCommand(query, myCon))
                    {
                        myReader = cmd.ExecuteReader();

                        if (myReader.HasRows == false)
                            return BadRequest("There are no optionals in the DataBase!");

                        tbl.Load(myReader);

                       
                        myReader.Close();
                        myCon.Close();
                    }


                }


            }

            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }


            try
            {
                using (SqlConnection myCon = new SqlConnection(con_string))
                {
                    myCon.Open();

                    tbl.Columns.Add("TeacherName");

                    foreach (DataRow dr in tbl.Rows)
                    {
                        q = "select Name from Teachers where userID=" + Convert.ToInt32(dr["teacherID"]);

                        using (SqlCommand cmd = new SqlCommand(q, myCon))
                        {
                            myReader = cmd.ExecuteReader();

                            myReader.Read();

                            dr["TeacherName"] = myReader.GetString(0);
                        }

                        myReader.Close();
                    }

                    myReader.Close();
                    myCon.Close();
                }
            }



            catch (Exception ex)
            { return BadRequest(ex.Message); }




            return new JsonResult(tbl);
        }



        [HttpPut("setMaxNumberOfStud/{nr}/{courseName}")]
        public IActionResult setMaxNumberOfStud(int nr,string courseName)
        {
            string userID = Authentication.AccountController.getUserIDFromRequest(HttpContext.Request);

            if (userID == null)
            {
                return BadRequest("Invalid Token");
            }


            string q = "select type from Teachers where userID = " + userID;

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
                        
                        if(dr.HasRows==false)
                            return BadRequest("An error occured!");

                        dr.Read();

                        String type=dr.GetString(0);

                        if(!type.Equals("chief"))
                            return Unauthorized("Only chief teachers can set the maximum number of students!");

                    }

                    dr.Close();
                    conn.Close();
                }
            }

          
            catch (Exception ex)
            { return new JsonResult(ex.Message); }


            string @query="update ProposedOptionals set maxStudents="+nr + " where teacherID="+userID  + " and courseName='" + courseName+"'";


                 try
            {
                using (SqlConnection conn = new SqlConnection(myConn))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.ExecuteNonQuery();  

                    }
                }
            }

          
            catch (Exception ex)
            { return new JsonResult(ex.Message); }

            return new JsonResult("Succesfully updated!");

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

            string query = @"update Teachers set Name='" + s.Name + "',department='" + s.department + "' where userID="+ id;


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


            string delete_courses = @"delete from Courses where teacherID=" + id;
            string delete_courses2 = @"delete from StudentsCourses where courseID in ( select courseID from Courses where teacherID=" + id +")";
            string delete_courses3 = @"delete from StudentsOptionals where courseID in ( select courseID from Courses where teacherID=" + id + ")";
            string delete_courses4 = @"delete from Grades where courseID in ( select courseID from Courses where teacherID=" + id + ")";

            Console.WriteLine(delete_courses);
            Console.WriteLine(delete_courses2);
            Console.WriteLine(delete_courses3);
            Console.WriteLine(delete_courses4);


            try
            {
                using (SqlConnection myCon = new SqlConnection(sqlDataSource))
                {
                    myCon.Open();
                   SqlCommand c1=new SqlCommand(delete_courses, myCon);
                    SqlCommand c2=new SqlCommand(delete_courses2, myCon);
                    SqlCommand c3=new SqlCommand(delete_courses3, myCon);
                    SqlCommand c4 = new SqlCommand(delete_courses4, myCon);

                    c4.ExecuteNonQuery();
                    c3.ExecuteNonQuery();
                    c2.ExecuteNonQuery();
                    c1.ExecuteNonQuery();
                    
                    

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


            string query = "insert into ProposedOptionals values ( " + id + ",'" + department + "'," + cs.year + "," + cs.semester + "," + cs.credits + ",'" + cs.CourseName + "'," + 0 + " )";


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
                            return BadRequest("The student is not enrolled in any course!");

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

            return new JsonResult("Student Graded!");
        }

       


}



}
    
