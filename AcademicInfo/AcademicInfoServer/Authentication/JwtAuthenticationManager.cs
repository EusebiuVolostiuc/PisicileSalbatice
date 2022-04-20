using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AcademicInfoServer.Authentication
{
    public class JwtAuthenticationManager : IJwtAuthenticationManager
    {
        //modify to get data from db
        private readonly IDictionary<string, string> users = new Dictionary<string, string>()
        {
            {"test1","password1"}, {"test2","password2"}
        };

        private readonly string key;

        public JwtAuthenticationManager(string key)
        {
            this.key = key;
        }

        public JwtSecurityToken Authenticate(string userID, string password)
        {
            //modify if to work with db

            string query = @"select userName, accountType from Users where userName = '" + userID + "' and password = '" + password + "'";


            DataTable tbl = new DataTable();

            string sqlDataSource = "Data Source=.;Initial Catalog=AcademicInfo;Integrated Security = true;";

            SqlDataReader myReader;

            bool found = false;

            string userType = "";

            try
            {
                using (SqlConnection myCon = new SqlConnection(sqlDataSource))
                {
                    myCon.Open();
                    using (SqlCommand cmd = new SqlCommand(query, myCon))
                    {
                        myReader = cmd.ExecuteReader();

                        Console.WriteLine(myReader.HasRows);
                        if (myReader.HasRows == true)
                        {
                            found = true;
                            myReader.Read();
                            userType = myReader.GetString(1);
                        }
                          
                        myReader.Close();
                        myCon.Close();
                    }

                }
            }

            catch (Exception ex)
            {
                return null;
            }

            if (found == true)
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var Key = Encoding.ASCII.GetBytes(key);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                    new Claim(ClaimTypes.Name, userID)
                    ,new Claim(ClaimTypes.Role, userType)
                    }),
                    Expires = DateTime.UtcNow.AddHours(1),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Key), SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateJwtSecurityToken(tokenDescriptor);

                //return tokenHandler.WriteToken(token);

                return token;
            }

            else 
                return null;
        }
    }
}
