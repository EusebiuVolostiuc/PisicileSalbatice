﻿using AcademicInfoServer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data;
using System.Data.SqlClient;

namespace AcademicInfoServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StaffController : ControllerBase
    {

        private readonly IConfiguration _configuration;
        private static Random random = new Random();

        public StaffController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private string add_User(Student u)
        {

            string userName = u.name + "@academicinfo";
            string password= RandomString(10);
            string query = @"insert into Users (userName,password,accountType) values ('" + userName + "','" + password + "','" + "student" + "')";

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

        [HttpPost("add_Student")]
        public IActionResult add_Student(Student u)
        {

            dynamic newUser;
            try
            {
                newUser = JsonConvert.DeserializeObject(add_User(u));
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





            //return new JsonResult("Added succesfully!\n");
            return Ok(newUser);
        }

        [HttpGet]
        public JsonResult Get()
        {
            string query = @"select * from Staff";


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



            return new JsonResult(tbl);

        }


        [HttpPost]
        public JsonResult Post(Staff s)
        {


            string query = @"insert into Staff values (" + s.userId + ",'" + s.name +"')";


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
        public JsonResult Put(Staff s)
        {


            string query = @"update Staff set Name='" + s.name + "'" + " where userID=" + s.userId;


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






    }

}

