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

        public StudentController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("get_Grades")]


        

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

            List<int> ls=new List<int>();

            foreach(DataRow dr in tbl.Rows)
            {
                ls.Add(Convert.ToInt32(dr["teacherID"]));
            }

            string q2 = "select Name from teachers where userID in (";

            for(int i=0;i<ls.Count;i++)
            {
                q2+=ls[i];
                q2 += ",";
            }

            StringBuilder sb=new StringBuilder(q2);

            sb[sb.Length-1]=')';

            q2=sb.ToString();

            Console.WriteLine(q2);

            try
            {
                using (SqlConnection myCon = new SqlConnection(con_string))
                {
                    myCon.Open();
                    using (SqlCommand cmd = new SqlCommand(q2, myCon))
                    {
                        myReader = cmd.ExecuteReader();

                       DataTable dt2=new DataTable();

                        dt2.Load(myReader);

                        tbl.Columns.Add("TeacherName");

                        int i = 0;
                        foreach(DataRow dr in tbl.Rows)
                        {
                            dr["TeacherName"] = dt2.Rows[i]["Name"];

                            if(i<dt2.Rows.Count-1)
                                i++;
                        }

                        myReader.Close();
                        myCon.Close();
                    }
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


            string query = "select * from Courses where courseID in (select courseID from StudentsCourses where studentID =" + userID + ") and courseType='o'";



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

            List<int> ls = new List<int>();

            foreach (DataRow dr in tbl.Rows)
            {
                ls.Add(Convert.ToInt32(dr["teacherID"]));
            }

            string q2 = "select Name from teachers where userID in (";

            for (int i = 0; i < ls.Count; i++)
            {
                q2 += ls[i];
                q2 += ",";
            }

            StringBuilder sb = new StringBuilder(q2);

            sb[sb.Length - 1] = ')';

            q2 = sb.ToString();

            Console.WriteLine(q2);

            try
            {
                using (SqlConnection myCon = new SqlConnection(con_string))
                {
                    myCon.Open();
                    using (SqlCommand cmd = new SqlCommand(q2, myCon))
                    {
                        myReader = cmd.ExecuteReader();

                        DataTable dt2 = new DataTable();

                        dt2.Load(myReader);

                        tbl.Columns.Add("TeacherName");

                        int i = 0;
                        foreach (DataRow dr in tbl.Rows)
                        {
                            dr["TeacherName"] = dt2.Rows[i]["Name"];

                            if (i < dt2.Rows.Count - 1)
                                i++;
                        }

                        myReader.Close();
                        myCon.Close();
                    }
                }
            }

            catch (Exception ex)
            { return BadRequest(ex.Message); }




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


            string q2 = "select Name from teachers where userID in (";

            for (int i = 0; i < ls.Count; i++)
            {
                q2 += ls[i];
                q2 += ",";
            }

            StringBuilder sb = new StringBuilder(q2);

            sb[sb.Length - 1] = ')';

            q2 = sb.ToString();

            Console.WriteLine(q2);

            try
            {
                using (SqlConnection myCon = new SqlConnection(con_string))
                {
                    myCon.Open();
                    using (SqlCommand cmd = new SqlCommand(q2, myCon))
                    {
                        myReader = cmd.ExecuteReader();

                        DataTable dt2 = new DataTable();

                        dt2.Load(myReader);

                        tbl.Columns.Add("TeacherName");

                        int i = 0;
                        foreach (DataRow dr in tbl.Rows)
                        {
                            dr["TeacherName"] = dt2.Rows[i]["Name"];

                            if (i < dt2.Rows.Count - 1)
                                i++;
                        }

                        myReader.Close();
                        myCon.Close();
                    }
                }
            }

            catch (Exception ex)
            { return BadRequest(ex.Message); }




            return BadRequest(tbl);
        }


    }
}

            

     