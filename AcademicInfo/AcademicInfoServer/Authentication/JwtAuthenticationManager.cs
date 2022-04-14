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

        public SecurityToken Authenticate(string userID, string password)
        {
            //modify if to work with db

            string query = @"select userName,password from Users";


            DataTable tbl = new DataTable();

            string sqlDataSource = "Data Source=.;Initial Catalog=AcademicInfo;Integrated Security = true;";

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
                return null;
            }

            bool ok = false;

            foreach(DataRow dr in tbl.Rows)
            {
                string userName = dr["userName"].ToString();
                string passwd = dr["password"].ToString();

                if(userName == userID && passwd == password)
                    ok=true;

            }

            if (ok == false)
                return null;


            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.ASCII.GetBytes(key);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, userID)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            //return tokenHandler.WriteToken(token);

            return token;
 
        }
    }
}
