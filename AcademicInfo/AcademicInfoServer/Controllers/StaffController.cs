using AcademicInfoServer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;

namespace AcademicInfoServer.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "staff")]

    public class StaffController : ControllerBase
    {

        private readonly IConfiguration _configuration;
        private static Random random = new Random();
        private readonly IWebHostEnvironment _env;

        public StaffController(IConfiguration configuration,IWebHostEnvironment env)
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
                var file = req.Files[0];
                String[] extension = file.FileName.Split('.');
                String filename = userID + "." + extension[extension.Length - 1];
                var physicalPath = _env.ContentRootPath + "/Photos/Staff/" + filename;

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

        private static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }


        private string add_User(dynamic u)
        {
            string name_ = u.name.Replace(" ", String.Empty);
            string type_=u.type.Replace(" ",String.Empty);

            string userName = name_ + "@"+ type_ + ".academicinfo.ro";
            string password= RandomString(10);

            
                MD5 md5Hash= MD5.Create();
                // Byte array representation of source string
                var sourceBytes = Encoding.UTF8.GetBytes(password);

                // Generate hash value(Byte Array) for input data
                var hashBytes = md5Hash.ComputeHash(sourceBytes);

                // Convert hash byte array to string
                var hash = BitConverter.ToString(hashBytes).Replace("-", string.Empty);

                
            

            string query = @"insert into Users (userName,password,accountType) values ('" + userName + "','" + hash + "','" + u.type + "')";

            DataTable tbl = new DataTable();

            string sqlDataSource = _configuration.GetConnectionString("AcademicInfo");

            SqlDataReader myReader;


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

            var newAccount = new
            {
                Message = "User Authenticated Successfully!",
                userN = userName,
                passwd = password
               
            };

            return JsonConvert.SerializeObject(newAccount);

           
        }
        [HttpGet("get_Students")]
        public JsonResult Get(string? year = "%", string? group = "%", int? minAvg = 0, int? maxAvg = 10)
        {
            string query = @"select *, (select cast(avg(cast(value as decimal(10,2))) as decimal(10,2)) from grades group by studentID having studentID = userID) as average 
                            from students
                            where year like '" + year + "' and groupp like '" + group + "' order by average desc";



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

            var filteredRows = tbl.Select("average is null or (average>=" + minAvg + " and average <= " + maxAvg + ")");

            if (filteredRows.Count() == 0)
                return new JsonResult("[]");

            return new JsonResult(filteredRows.CopyToDataTable());

        }



        [HttpPost("add_Student")]
        public IActionResult add_Student(Student u)
        {

            dynamic newUser;
            try
            {
                dynamic user = new
                {

                    name = u.name,
                    type = "student"
   

                };

  
                newUser = JsonConvert.DeserializeObject(add_User(user));
            }

            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }
            
            string query = @"select accountId from Users where userName= '" + newUser.userN + "'";

            Console.WriteLine(query);

            DataTable tbl = new DataTable();

            string sqlDataSource = _configuration.GetConnectionString("AcademicInfo");

            SqlDataReader myReader;

            int id=-1;

            try
            {
                using (SqlConnection myCon = new SqlConnection(sqlDataSource))
                {
                    myCon.Open();
                    using (SqlCommand cmd = new SqlCommand(query, myCon))
                    {

                        myReader = cmd.ExecuteReader();
                        myReader.Read();

                        id = (int)myReader["accountId"];

                        myCon.Close();
                    }

                }
            }



            catch(Exception ex)
            {
                return new JsonResult(ex.Message);
            }

            

          

            string query2 = @"insert into Students values (" + id + ",'" + u.name + "','" + u.department + "'," + u.year + "," + u.group + ")";

            try
            {
                using (SqlConnection myCon = new SqlConnection(sqlDataSource))
                {
                    myCon.Open();
                    using (SqlCommand cmd = new SqlCommand(query2, myCon))
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

            string enroll_student = @"select courseID from courses where department='" + u.department + "' and " + "year=" + u.year + " and courseType='m'";

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

                        while (myReader.Read())
                        {
                            int cid = myReader.GetInt32(0);
                            ids.Add(cid);

                        }

                        myReader.Close();

                        foreach (int cid in ids)
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

            //return new JsonResult("Added succesfully!\n");
            return Ok(newUser);
        }

        [HttpPost("add_Teacher")]
        public IActionResult add_Teacher(Teacher u)
        {

            dynamic newUser;
            try
            {
                dynamic user = new
                {

                    name = u.Name,
                    type = "teacher"


                };


                newUser = JsonConvert.DeserializeObject(add_User(user));
            }

            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }

            string query = @"select accountId from Users where userName= '" + newUser.userN + "'";

            Console.WriteLine(query);

            DataTable tbl = new DataTable();

            string sqlDataSource = _configuration.GetConnectionString("AcademicInfo");

            SqlDataReader myReader;

            int id = -1;

            try
            {
                using (SqlConnection myCon = new SqlConnection(sqlDataSource))
                {
                    myCon.Open();
                    using (SqlCommand cmd = new SqlCommand(query, myCon))
                    {

                        myReader = cmd.ExecuteReader();
                        myReader.Read();

                        id = (int)myReader["accountId"];

                        myCon.Close();
                    }

                }
            }

            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }





            string query2 = @"insert into Teachers values (" + id + ",'" + u.Name + "','" + u.department + "','" + u.type + "')";

            try
            {
                using (SqlConnection myCon = new SqlConnection(sqlDataSource))
                {
                    myCon.Open();
                    using (SqlCommand cmd = new SqlCommand(query2, myCon))
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


            //return new JsonResult("Added succesfully!\n");
            return Ok(newUser);
        }


      
        [HttpPost]
        public IActionResult Post(Staff s)
        {

            dynamic user = new
            {

                name = s.name,
                type = "staff"


            };

            var newUser = JsonConvert.DeserializeObject(add_User(user));

            string query = @"select accountId from Users where userName= '" + newUser.userN + "'";

            Console.WriteLine(query);

            DataTable tbl = new DataTable();

            string sqlDataSource = _configuration.GetConnectionString("AcademicInfo");

            SqlDataReader myReader;

            int id = -1;

            try
            {
                using (SqlConnection myCon = new SqlConnection(sqlDataSource))
                {
                    myCon.Open();
                    using (SqlCommand cmd = new SqlCommand(query, myCon))
                    {

                        myReader = cmd.ExecuteReader();
                        myReader.Read();

                        id = (int)myReader["accountId"];

                        myCon.Close();
                    }

                }
            }

            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }






            string query2 = @"insert into Staff values (" + id + ",'" + s.name +"')" ;


      

            try
            {
                using (SqlConnection myCon = new SqlConnection(sqlDataSource))
                {
                    myCon.Open();
                    using (SqlCommand cmd = new SqlCommand(query2, myCon))
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



            return Ok(newUser);

        }



        [HttpPut]
        public IActionResult Put(Staff s)
        {
            string userID = Authentication.AccountController.getUserIDFromRequest(HttpContext.Request);

            if (userID == null)
            {
                return BadRequest("Invalid Token");
            }

            int id = Convert.ToInt32(userID);


            string query = @"update Staff set Name='" + s.name + "'" + " where userID=" + id;


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

            catch(Exception ex)
            {
                return new JsonResult(ex.Message);
            }

           

            return new JsonResult("Updated succesfully!");

        }

        [HttpDelete("{id}")]

        public JsonResult Delete(int id)
        {
            string query = @"delete from Staff where userID=" + id;


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

        [HttpGet("get_Staff")]

        public IActionResult get_Staff()
        {

            string userID = Authentication.AccountController.getUserIDFromRequest(HttpContext.Request);

            if (userID == null)
            {
                return BadRequest("Invalid Token");
            }

            int id = Convert.ToInt32(userID);


            string q = "select * from staff where userID=" + id;

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






    }

}

