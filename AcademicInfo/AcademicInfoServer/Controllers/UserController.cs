using AcademicInfoServer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace AcademicInfoServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly IConfiguration _configuration;

        public UserController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public JsonResult Get()
        {
            string query = @"select * from Users";



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

           
            return new JsonResult(tbl);

        }


        [HttpPost]
        public JsonResult Post(User u)
        {


            string query = @"insert into Users (userName,password,accountType) values ('" + u.UserName + "','" + u.password + "','" + u.accountType + "')";

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

           

            return new JsonResult("Added succesfully!");

        }



        [HttpPut]
        public JsonResult Put(User u)
        {


            string query = @"update Users set userName='" + u.UserName + "',password='" + u.password + "'," + "accountType='" + u.accountType  + "' where accountID=" + u.accountId;


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
            string query = @"delete from Users where accountID=" + id;


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






    }

}

