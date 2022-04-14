namespace AcademicInfoServer.Authentication
{
    public interface IJwtAuthenticationManager
    {
        string Authenticate(string userID, string password);
    }
}
