import {Component, OnInit, TemplateRef, ViewChild} from '@angular/core';
import {HttpClient, HttpHeaders} from "@angular/common/http";
import {MatDialog} from "@angular/material/dialog";

interface Course {
  courseID: number,
  name:string
  department:string,
  year:number,
  semester:number,
  credits:number,
  courseType:string,
  TeacherName: string,
  CourseName: string
}

interface Grade {
  studentID: number,
  courseID: number,
  value: number,
  weight: number,
}

@Component({
  selector: 'app-courses-tab',
  templateUrl: './courses-tab.component.html',
  styleUrls: ['./courses-tab.component.css']
})
export class CoursesTabComponent implements OnInit {
  @ViewChild('modalGrades', { static: true }) modalGrades: TemplateRef<any>;

  columns = [
    {
      columnDef: 'CourseName',
      header: 'Course',
      cell: (element: Course) => `${element.CourseName}`,
    },
    {
      columnDef: 'department',
      header: 'Department',
      cell: (element: Course) => `${element.department}`,
    },
    {
      columnDef: 'year',
      header: 'Year',
      cell: (element: Course) => `${element.year}`,
    },
    {
      columnDef: 'semester',
      header: 'Semester',
      cell: (element: Course) => `${element.semester}`,
    },
    {
      columnDef: 'credits',
      header: 'Credits',
      cell: (element: Course) => `${element.credits}`,
    },
    {
      columnDef: 'courseType',
      header: 'Course Type',
      cell: (element: Course) => `${element.courseType}`,
    },
    {
      columnDef: 'TeacherName',
      header: 'Teacher',
      cell: (element: Course) => `${element.TeacherName}`,
    },
  ];

  gradeColumns = [
    {
      columnDef: 'value',
      header: 'Value',
      cell: (element: Grade) => `${element.value}`,
    },
    {
      columnDef: 'weight',
      header: 'Weight',
      cell: (element: Grade) => `${element.weight}`,
    }
  ];

  displayedColumns = this.columns.map(c => c.columnDef);
  courses: Course[]
  grades: Grade[]
  displayGrades: Grade[]
  constructor(private http: HttpClient,
              private dialogBox: MatDialog) { }

  ngOnInit(): void {
    var token = localStorage.getItem('token');

    var tokenise = "Bearer " + token;


    var headers = new HttpHeaders().set("Authorization", tokenise);

    const httpOptions = {
      headers: headers
    }
    this.http.get('https://localhost:4200/api/student/get_Courses',httpOptions)
      .subscribe(response => {


        var courses_ = Object.values(response)

        let postArr: any[];
        postArr = [];
        courses_.forEach(element => postArr.push(element));
        this.courses=postArr;
        console.log(this.courses);
      })

    this.http.get('https://localhost:4200/api/student/get_Grades',httpOptions)
      .subscribe(response => {


        var grades_ = Object.values(response)

        let postArr: any[];
        postArr = [];
        grades_.forEach(element => postArr.push(element));
        this.grades=postArr;
        console.log(this.grades);
      })
  }
  sendCellData(data: any): void {
    // @ts-ignore
    const courseId = this.courses.find(course => {return course.CourseName === data}) === undefined ? -1 : this.courses.find(course => {return course.CourseName === data}).courseID
    const grades = this.grades.filter(grade => {return grade.courseID === courseId});
    this.displayGrades = grades;
    if (grades.length > 0) {
      this.dialogBox.open(this.modalGrades);
    }
  }

}


