
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import {HttpClient, HttpHeaders} from "@angular/common/http";
import {map} from "rxjs";

interface Student {
  userID:number,
  Name:string,
  department:string,
  year:number,
  groupp: number,
  average: number
}

@Component({
  selector: 'app-manage-students',
  templateUrl: './manage-students.component.html',
  styleUrls: ['./manage-students.component.css']
})
export class ManageStudentsComponent implements OnInit {

  displayedColumns: string[] = ['position', 'name', 'year', 'group','average'];
  dataSource = [];
  students: Student[]
  tableStudents: Student[]

  groupFilter = '';
  averageMinFilter = '';
  averageMaxFilter = '';
  yearFilter = '';


  constructor(private router: Router, private http: HttpClient) { }

  ngOnInit(): void {
    this.loadStudents();
  }

  load_student_form(){
    this.router.navigateByUrl('student-form-component');
  }



  loadStudents() {

    var token = localStorage.getItem('token');

    var tokenise = "Bearer " + token;

    var headers = new HttpHeaders().set("Authorization", tokenise);

    const httpOptions = {
      headers: headers
    };

    this.http.get('https://localhost:4200/api/staff/get_Students', httpOptions).pipe(map(responseData => {
      const postArr = []
      for (const key in responseData) {
        if (responseData.hasOwnProperty(key)) {
          postArr.push((responseData as any)[key])
        }
      }
      return postArr;
    })).subscribe(posts => {
      this.students = posts
      this.tableStudents = this.students;
    })
  }

  applyFilters() {
    let groupFilter = parseInt(this.groupFilter);
    let averageMinFilter = parseFloat(this.averageMinFilter);
    let averageMaxFilter = parseFloat(this.averageMaxFilter);
    let yearFilter = parseInt(this.yearFilter);

    let filterGroup = this.groupFilter !== '' ? ((student: Student) => {return student.groupp === groupFilter}) :
      ((student: Student) => {return true})

    let filterAverageMin = this.averageMinFilter !== '' ? ((student: Student) => {return student.average >= averageMinFilter}) :
      ((student: Student) => {return true})

    let filterAverageMax = this.averageMaxFilter !== '' ? ((student: Student) => {return student.average <= averageMaxFilter}) :
      ((student: Student) => {return true})

    let filterYear = this.yearFilter !== '' ? ((student: Student) => {return student.year == yearFilter}) :
      ((student: Student) => {return true})

    this.tableStudents = this.students.filter(student => {
      return filterGroup(student) && filterAverageMin(student) && filterAverageMax(student) && filterYear(student);
    });

  }

}

