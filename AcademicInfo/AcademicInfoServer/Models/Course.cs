namespace AcademicInfoServer.Models
{
    public class Course
    {

        public int courseId { get; set; }

        public int teacherId { get; set; }

        public string department { get; set; }

        public int year { get; set; }

        public int semester { get; set; }

        public int credits { get; set; }

        public char courseType { get; set; }


    }
}
