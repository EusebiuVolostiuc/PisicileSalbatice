import {Component, Inject, Injectable, OnInit} from '@angular/core';
import {Student, StudentProfileComponent} from '../student-profile.component';
import {HttpClient, HttpHeaders} from "@angular/common/http";
import {Router} from "@angular/router";

@Component({
  selector: 'app-update-profile',
  templateUrl: './update-profile.component.html',
  styleUrls: ['./update-profile.component.css']
})

export class UpdateProfileComponent implements OnInit {
  student:Student

  constructor(private comp:StudentProfileComponent,private http: HttpClient,private router:Router) {
    this.student = {
      "name":comp.name,
      "department":comp.department,
      "year":comp.year,
      "group":comp.group
    }
  }

  ngOnInit(): void {

  }

  revertChanes() {
    this.student = {
      "name":this.comp.name,
      "department":this.comp.department,
      "year":this.comp.year,
      "group":this.comp.group
    }
  }

  updateStudent() {
    /*
    /api/student cu http put
     */
    const studentData = {

      department : this.student.department,
      name : this.student.name,
      year : this.student.year,
      group: this.student.group
    }

    var token = localStorage.getItem('token');

    var tokenise = "Bearer " + token;


    var headers = new HttpHeaders().set("Authorization", tokenise);

    const httpOptions = {
      headers: headers
    };

    this.http.put('https://localhost:4200/api/student',studentData,httpOptions)
      .subscribe(response => {
        console.log(response);
        location.reload();
      })



  }
}
