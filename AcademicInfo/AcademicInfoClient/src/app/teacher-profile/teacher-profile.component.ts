import { Component, OnInit } from '@angular/core';
import {Router} from "@angular/router";
import {HttpClient, HttpHeaders, HttpParams} from "@angular/common/http";

@Component({
  selector: 'app-teacher-profile',
  templateUrl: './teacher-profile.component.html',
  styleUrls: ['./teacher-profile.component.css']
})
export class TeacherProfileComponent implements OnInit {
  name: any;
  department:any;
  propose: number;
  hello: number;
  updatePfinfo: number;
  gradeStudent: number;
  seeOptionalProposals:number;
  chief:number;

  constructor(private router:Router,private http: HttpClient) {

  }

  ngOnInit(): void {
    this.hello=1;
    this.propose=0;
    this.updatePfinfo=0;
    this.gradeStudent=0;
    this.seeOptionalProposals=0;
    var token = localStorage.getItem('token');

    var tokenise = "Bearer " + token;


    var headers = new HttpHeaders().set("Authorization", tokenise);

    const httpOptions = {
      headers: headers
    };
    this.http.get('https://localhost:4200/api/teacher/get_Teacher',httpOptions)
      .subscribe(response => {
        var teacher = Object.values(response)[0];
        console.log(teacher);
        this.name=teacher["Name"]
        this.department=teacher["department"]
        if(teacher["type"]=="chief")
          this.chief=1;
        else
          this.chief=0
      })


  }

  logout() {
    this.router.navigateByUrl("");
    localStorage.setItem("token","");
  }

  loadOptionalForm() {
    this.hello=0;
    this.updatePfinfo=0;
    this.gradeStudent=0;
    this.propose=1;
  }

  updateProfileInfo() {
    this.hello=0;
    this.propose=0;
    this.gradeStudent=0;
    this.seeOptionalProposals=0;
    this.updatePfinfo=1;
  }

  loadGradeStudent() {
    this.hello=0;
    this.propose=0;
    this.updatePfinfo=0;
    this.seeOptionalProposals=0;
    this.gradeStudent=1;
  }

  loadOptionalProposals() {
    this.hello=0;
    this.propose=0;
    this.updatePfinfo=0;
    this.gradeStudent=0;
    this.seeOptionalProposals=1;

  }
}
