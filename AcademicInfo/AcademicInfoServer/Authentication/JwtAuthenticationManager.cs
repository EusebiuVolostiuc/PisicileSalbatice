using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace AcademicInfoServer.Authentication
{
    public class JwtAuthenticationManager : IJwtAuthenticationManager
    {

        private readonly string key;

        public JwtAuthenticationManager(string key)
        {
            this.key = key;
        }

        public JwtSecurityToken Authenticate(string userID, string password)
        {

            MD5 md5Hash = MD5.Create();
            // Byte array representation of source string
            var sourceBytes = Encoding.UTF8.GetBytes(password);

            // Generate hash value(Byte Array) for input data
            var hashBytes = md5Hash.ComputeHash(sourceBytes);

            // Convert hash byte array to string
            var hash = BitConverter.ToString(hashBytes).Replace("-", string.Empty);

            string query = @"select accountType, accountId from Users where userName = '" + userID + "' and password = '" + hash + "'";


            DataTable tbl = new DataTable();

            string sqlDataSource = "Data Source=.;Initial Catalog=AcademicInfo;Integrated Security = true;";

            SqlDataReader myReader;

            bool found = false;

            string userType = "";
            string userCode = "";

            try
            {
                using (SqlConnection myCon = new SqlConnection(sqlDataSource))
                {
                    myCon.Open();
                    using (SqlCommand cmd = new SqlCommand(query, myCon))
                    {
                        myReader = cmd.ExecuteReader();

                        if (myReader.HasRows == true)
                        {
                            found = true;
                            myReader.Read();
                            userType = myReader.GetString(0);
                            userCode = myReader.GetValue(1).ToString();
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
                    new Claim(ClaimTypes.Name, userCode)
                    ,new Claim(ClaimTypes.Role, userType)
                    }),
                    Expires = DateTime.UtcNow.AddMinutes(30),
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
