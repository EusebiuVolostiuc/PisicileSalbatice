namespace AcademicInfoServer.Models
{
    public class Student
    {
        int Id { get; set; }

        string Department { get; set; }

        int Year { get; set; }

        int GroupNo { get; set; }

        public Student(int id, string department, int year, int groupNo)
        {
            Id = id;
            Department = department;
            Year = year;
            GroupNo = groupNo;
        }
    }
}
