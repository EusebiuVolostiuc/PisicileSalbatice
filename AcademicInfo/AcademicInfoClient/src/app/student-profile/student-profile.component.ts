import { Component, OnInit } from '@angular/core';
import {FormControl, FormGroup, Validators} from "@angular/forms";
import {HttpClient, HttpHeaders} from "@angular/common/http";
import {Router} from "@angular/router";
import {parseJson} from "@angular/cli/utilities/json-file";
export interface Student{
  name: String;
  department: String;
  year:String;
  group:String;
}
@Component({
  selector: 'app-student-profile',
  templateUrl: './student-profile.component.html',
  styleUrls: ['./student-profile.component.css']
})


export class StudentProfileComponent implements OnInit {
  public addStudentForm: FormGroup
  name: String;
  department: String;
  year: String;
  group: String;
  hello: number;
  coursesTab: number;
  optionalTab: number;
  updatePfinfo:number;
  student:Student;


  constructor(private http: HttpClient,private router:Router) {
  }

  ngOnInit(): void {
    var token = localStorage.getItem('token');

    var tokenise = "Bearer " + token;


    var headers = new HttpHeaders().set("Authorization", tokenise);

    const httpOptions = {
      headers: headers
    };

    this.http.get('https://localhost:4200/api/student',httpOptions)
      .subscribe(response => {
        var student = Object.values(response)[0];
        console.log(student);
        this.name= student["Name"];
        this.department=student["department"];
        this.year=student["year"];
        this.group=student["groupp"];
      })

    this.hello=1;
    this.coursesTab=0;
    this.optionalTab=0;
    this.updatePfinfo=0;
    this.student={
      "name":this.name,
      "department":this.department,
      "year":this.year,
      "group":this.group
    }
  }

  addStudent() {
    const studentData = {
      userId : this.addStudentForm.value.userId,
      department : this.addStudentForm.value.department,
      name : this.addStudentForm.value.name,
      year : this.addStudentForm.value.year,
      group: this.addStudentForm.value.group
    }
    this.http.post('http://localhost:4200/api/student', studentData)
      .subscribe(response => {
        console.log('post response ', response);
      })
  }

  logout() {
    localStorage.setItem("token","");
    this.router.navigateByUrl("");
  }

  loadCoursesTab() {
    this.hello=0;
    this.optionalTab=0;
    this.updatePfinfo=0;
    this.coursesTab=1;
  }

  loadOptionalsTab() {
    this.hello=0;
    this.coursesTab=0;
    this.updatePfinfo=0;
    this.optionalTab=1;
  }

  updateProfileInfo() {
    this.hello=0;
    this.coursesTab=0;
    this.optionalTab=0;
    this.updatePfinfo=1;
  }
}
