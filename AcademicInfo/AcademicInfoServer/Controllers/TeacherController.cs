using AcademicInfoServer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace AcademicInfoServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles="teacher")]
    public class TeacherController : ControllerBase

    {
        private IConfiguration _configuration;
        public TeacherController(IConfiguration config)
        {
            _configuration = config;
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

            try
            {
                using(SqlConnection conn=new SqlConnection(myConn))
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

                        int no_proposed=myReader.GetInt32(0);

                        if (no_proposed > 1)
                            return new JsonResult("The teacher already proposed 2 optionals!");

                        no_proposed++;

                        myReader.Close();

                        string q2 = "update Teachers set proposed=" + no_proposed + " where userId=" + id;

                        SqlCommand cmd2 = new SqlCommand(q2, conn);

                        cmd2.ExecuteNonQuery();


                    }

                    conn.Close();
                }
            }

            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }


            string query = "insert into ProposedOptionals values ( " + id + ",'" + cs.department + "'," + cs.year + "," + cs.semester + "," + cs.credits + ",'" + cs.CourseName + "')";

            
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
    }
}
