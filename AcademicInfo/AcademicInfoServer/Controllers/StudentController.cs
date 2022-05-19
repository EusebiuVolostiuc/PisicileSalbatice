using AcademicInfoServer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace AcademicInfoServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "student")]
    public class StudentController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;

        public StudentController(IConfiguration configuration,IWebHostEnvironment env)
        {
            _configuration = configuration;
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
                var file=req.Files[0];
                String [] extension = file.FileName.Split('.');
                String filename = userID+"."+extension[extension.Length-1];
                var physicalPath = _env.ContentRootPath + "/Photos/Students/" + filename;

                using (var stream=new FileStream(physicalPath,FileMode.Create))
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

        [HttpGet("get_Grades")]

        public IActionResult get_Grades()
        {


            string userID = Authentication.AccountController.getUserIDFromRequest(HttpContext.Request);

            if (userID == null)
            {
                return BadRequest("Invalid Token");
            }

            int id = Convert.ToInt32(userID);

            string query = @"select * from grades where studentID=" +id;


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
                return BadRequest(ex.Message);
            }



            return new JsonResult(tbl);

        }



        [HttpPut]
        public IActionResult Put(Student s)
        {


            string userID = Authentication.AccountController.getUserIDFromRequest(HttpContext.Request);

            if (userID == null)
            {
                return BadRequest("Invalid Token");
            }

            int id = Convert.ToInt32(userID);


            string query = @"update Students set Name='" + s.name + "',department='" + s.department + "'," + "year=" + s.year + ",groupp=" + s.group + " where userID="+ id;


            Console.WriteLine(query);
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

            string delete_courses=@"delete from StudentsCourses where studentID="+id;

            Console.WriteLine(delete_courses);

            
               try
            {
                using (SqlConnection myCon = new SqlConnection(sqlDataSource))
                {
                    myCon.Open();
                    using (SqlCommand cmd = new SqlCommand(delete_courses, myCon))
                    {
                        myReader = cmd.ExecuteReader();

                        myReader.Close();
                        myCon.Close();
                    }

                }
            }

            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }

            



            string enroll_student=@"select courseID from courses where department='" + s.department + "' and " + "year=" + s.year + " and courseType='m'";

            Console.WriteLine(enroll_student);

               try
            {
                using (SqlConnection myCon = new SqlConnection(sqlDataSource))
                {
                    myCon.Open();
                    using (SqlCommand cmd = new SqlCommand(enroll_student, myCon))
                    {
                        myReader = cmd.ExecuteReader();

                        List<int> ids = new List<int>();
                        
                        while(myReader.Read())
                        {
                            int cid = myReader.GetInt32(0);
                            ids.Add(cid);
                            
                        }

                        myReader.Close();

                        foreach(int cid in ids)
                        {
                            string enroll = @"insert into StudentsCourses values ( " + id + "," + cid + ")";
                            Console.WriteLine(enroll);
                            SqlCommand cmd2 = new SqlCommand(enroll, myCon);
                            cmd2.ExecuteNonQuery();
                        }

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

        [HttpDelete("{id}")]

        public JsonResult Delete(int id)
        {
            string query = @"delete from Students where userID=" + id;


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


            return new JsonResult("Deleted succesfully!");
        }

        // get /api/student
        [HttpGet]
        public IActionResult GetStudent()
        {
            string userID = Authentication.AccountController.getUserIDFromRequest(HttpContext.Request);

            if (userID == null)
            {
                return BadRequest("Invalid Token");
            }


            SqlDataReader myReader;

            DataTable tbl = new DataTable();

            string sqlDataSource = _configuration.GetConnectionString("AcademicInfo");;

            try
            {
                using (SqlConnection myCon = new SqlConnection(sqlDataSource))
                {
                    myCon.Open();
                    using (SqlCommand cmd = new SqlCommand(@"select * from Students where userID = " + userID + ";", myCon)
                    {

                    })
                    {
                        myReader = cmd.ExecuteReader();

                        if (myReader.HasRows == true)
                        {
                            tbl.Load(myReader);

                            return Ok(tbl);
                        }
                        else
                        {
                            return NotFound();
                        }
                        myReader.Close();
                        myCon.Close();
                    }

                }
            }

            catch (Exception ex) {

             return new JsonResult(ex.Message);

            }
        }

        [HttpGet("get_Courses")]

        public IActionResult get_Courses()
        {

            string userID = Authentication.AccountController.getUserIDFromRequest(HttpContext.Request);

            if (userID == null)
            {
                return BadRequest("Invalid Token");
            }


            string query = "select * from Courses where courseID in (select courseID from StudentsCourses where studentID ="+ userID +")";

            

            SqlDataReader myReader;

            DataTable tbl=new DataTable();

            string con_string = _configuration.GetConnectionString("AcademicInfo");

            try
            {
                using (SqlConnection myCon = new SqlConnection(con_string))
                {
                    myCon.Open();
                    using (SqlCommand cmd = new SqlCommand(query, myCon))
                    {
                        myReader = cmd.ExecuteReader();

                        if (myReader.HasRows == false)
                            return BadRequest("There are no courses for the current student!");

                        tbl.Load(myReader);

                        tbl.Columns["courseType"].MaxLength = 500;

                        foreach(DataRow dr in tbl.Rows)
                        {
                         
                            if (dr["courseType"].Equals("m"))
                                dr["courseType"] = "mandatory";
                            else
                                dr["courseType"] = "optional";

                     
                        }

                        myReader.Close();
                        myCon.Close();
                    }
                }
            }

            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }

          
           

            try
            {
                using (SqlConnection myCon = new SqlConnection(con_string))
                {
                    myCon.Open();
                        
                    tbl.Columns.Add("TeacherName");

                    foreach(DataRow dr in tbl.Rows)
                    {
                        string q="select Name from Teachers where userID=" + Convert.ToInt32(dr["teacherID"]);

                        using(SqlCommand cmd = new SqlCommand(q, myCon))
                        {
                            myReader=cmd.ExecuteReader();

                            myReader.Read();

                            dr["TeacherName"]=myReader.GetString(0);
                        }

                        myReader.Close();
                    }

                    myReader.Close();
                    myCon.Close();
                  }
                }
      

            catch(Exception ex)
            { return new JsonResult(ex.Message);}

          
            

            return new JsonResult(tbl);
        }

        [HttpGet("getEnrolledOptionals")]
        public IActionResult getEnrolledOptionals()
        {
            string userID = Authentication.AccountController.getUserIDFromRequest(HttpContext.Request);

            if (userID == null)
            {
                return BadRequest("Invalid Token");
            }


            string query = "select * from Courses where courseID in (select courseID from StudentsOptionals where studentID =" + userID + ") ";



            SqlDataReader myReader;

            DataTable tbl = new DataTable();

            string con_string = _configuration.GetConnectionString("AcademicInfo");

            try
            {
                using (SqlConnection myCon = new SqlConnection(con_string))
                {
                    myCon.Open();
                    using (SqlCommand cmd = new SqlCommand(query, myCon))
                    {
                        myReader = cmd.ExecuteReader();

                        if (myReader.HasRows == false)
                            return BadRequest("There are no courses for the current student!");

                        tbl.Load(myReader);

                        tbl.Columns["courseType"].MaxLength = 500;

                        foreach (DataRow dr in tbl.Rows)
                        {

                            if (dr["courseType"].Equals("m"))
                                dr["courseType"] = "mandatory";
                            else
                                dr["courseType"] = "optional";


                        }

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
                        string q = "select Name from Teachers where userID=" + Convert.ToInt32(dr["teacherID"]);

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
            { return new JsonResult(ex.Message); }



            return new JsonResult(tbl);

        }




        [HttpGet("get_Optionals")]

        public IActionResult get_Optionals()
        {

            string query = "select * from courses where courseType='o'";

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

                        tbl.Columns["courseType"].MaxLength = 500;

                        foreach(DataRow dr in tbl.Rows)
                        {
                         
                            if (dr["courseType"].Equals("m"))
                                dr["courseType"] = "mandatory";
                            else
                                dr["courseType"] = "optional";

                            ls.Add(Convert.ToInt32(dr["teacherID"]));
                        }

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
                        string q = "select Name from Teachers where userID=" + Convert.ToInt32(dr["teacherID"]);

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


    }
}

            

     