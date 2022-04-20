using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace AcademicInfoServer.Authentication
{
    public interface IJwtAuthenticationManager
    {
        //string Authenticate(string userID, string password);
        JwtSecurityToken Authenticate(string userID, string password);
    }
}
