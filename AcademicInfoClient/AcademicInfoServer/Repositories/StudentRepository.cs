using AcademicInfoServer.Models;

namespace AcademicInfoServer.Repositories
{
    public class StudentRepository
    {
        private readonly List<Student> _studentRepository = new List<Student>();

        StudentRepository()
        {
            _studentRepository.Append(new Student(1, "CS", 2, 927));
            _studentRepository.Append(new Student(2, "CS", 3, 937));
            _studentRepository.Append(new Student(3, "CS", 1, 917));
        }

        public void Add(Student student)
        {
            _studentRepository.Append(student);
        }

        public Student[] GetStudents()
        {
            return _studentRepository.ToArray();
        }
    }
}
