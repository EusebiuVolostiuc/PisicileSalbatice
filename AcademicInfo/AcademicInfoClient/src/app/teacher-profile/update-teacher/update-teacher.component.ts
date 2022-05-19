import { Component, OnInit } from '@angular/core';
import {TeacherProfileComponent} from "../teacher-profile.component";
import {HttpClient, HttpHeaders} from "@angular/common/http";

@Component({
  selector: 'app-update-teacher',
  templateUrl: './update-teacher.component.html',
  styleUrls: ['./update-teacher.component.css']
})
export class UpdateTeacherComponent implements OnInit {
  teacher: any;

  constructor(private t:TeacherProfileComponent,private http: HttpClient) { }

  ngOnInit(): void {
    this.teacher = {
      "name": this.t.name,
      "department":this.t.department
    }
  }

  updateTeacher() {
    const teacherData = {
      name : this.teacher.name,
      department : this.teacher.department
    }

    var token = localStorage.getItem('token');

    var tokenise = "Bearer " + token;


    var headers = new HttpHeaders().set("Authorization", tokenise);

    const httpOptions = {
      headers: headers
    };

    this.http.put('https://localhost:4200/api/teacher',teacherData,httpOptions)
      .subscribe(response => {
        console.log(response);
        location.reload();
      })
  }

  revertChanges() {
    this.teacher = {
      "name": this.t.name,
      "department":this.t.department
    }
  }
}
