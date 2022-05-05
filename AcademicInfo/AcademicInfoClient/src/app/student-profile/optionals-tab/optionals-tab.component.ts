import { Component, OnInit } from '@angular/core';
import {HttpClient, HttpHeaders} from "@angular/common/http";

interface Course {
  name:string
  department:string,
  year:number,
  semester:number,
  credits:number,
  courseType:string,
  TeacherName: string,
  CourseName : string
}

@Component({
  selector: 'app-optionals-tab',
  templateUrl: './optionals-tab.component.html',
  styleUrls: ['./optionals-tab.component.css']
})
export class OptionalsTabComponent implements OnInit {
  columns = [
    {
      columnDef: 'name',
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
      columnDef: 'teacher',
      header: 'Teacher',
      cell: (element: Course) => `${element.TeacherName}`,
    },
  ];

  displayedColumns = this.columns.map(c => c.columnDef);
  courses: Course[]
  constructor(private http: HttpClient) { }


  ngOnInit(): void {
    var token = localStorage.getItem('token');

    var tokenise = "Bearer " + token;


    var headers = new HttpHeaders().set("Authorization", tokenise);

    const httpOptions = {
      headers: headers
    }
    this.http.get('https://localhost:4200/api/student/getEnrolledOptionals',httpOptions)
      .subscribe(response => {
        var courses_ = Object.values(response)
        let postArr: any[];
        postArr = [];
        courses_.forEach(element => postArr.push(element));
        this.courses=postArr;
        console.log(this.courses);
      })
  }

}
