namespace AcademicInfoServer.Models
{
    public class Course
    {

        public int courseId { get; set; } = -1;


        public string department { get; set; } = "";

        public int year { get; set; } = -1;

        public int semester { get; set; } = -1;

        public int credits { get; set; } = -1;

        public char courseType { get; set; } = '\0';

        public string CourseName { get; set; } = "";


    }
}
