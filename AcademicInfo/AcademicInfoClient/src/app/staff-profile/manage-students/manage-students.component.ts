
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import {HttpClient, HttpHeaders} from "@angular/common/http";
import {map} from "rxjs";

interface Student {
  userID:number,
  Name:string,
  department:string,
  year:number,
  groupp:number
}

@Component({
  selector: 'app-manage-students',
  templateUrl: './manage-students.component.html',
  styleUrls: ['./manage-students.component.css']
})
export class ManageStudentsComponent implements OnInit {

  displayedColumns: string[] = ['position', 'name', 'year', 'group'];
  dataSource = [];
  students: Student[]


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
    })).subscribe(posts => this.students = posts)
  }

}

