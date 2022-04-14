using Microsoft.IdentityModel.Tokens;

namespace AcademicInfoServer.Authentication
{
    public interface IJwtAuthenticationManager
    {
        //string Authenticate(string userID, string password);
        SecurityToken Authenticate(string userID, string password);
    }
}
