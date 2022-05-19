namespace AcademicInfoServer.Models
{
    public class ProposedOptional
    {

        public int teacherID { get; set; }=-1;
        public String department { get; set; } = "";

        public int year { get; set; }=-1;

        public int semester { get; set; } = -1;
        public int credits { get; set; } = -1;

        public String CourseName { get; set; } = "";

    }
}
