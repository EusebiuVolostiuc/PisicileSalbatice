import { Component, OnInit } from '@angular/core';
import {Router} from "@angular/router";
import {HttpClient, HttpHeaders} from "@angular/common/http";

@Component({
  selector: 'app-teacher-profile',
  templateUrl: './teacher-profile.component.html',
  styleUrls: ['./teacher-profile.component.css']
})
export class TeacherProfileComponent implements OnInit {
  name: any;
  propose: number;
  hello: number;

  constructor(private router:Router,private http: HttpClient) {

  }

  ngOnInit(): void {
    this.hello=1;
    this.propose=0;
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
      })
  }

  logout() {
    this.router.navigateByUrl("");
    localStorage.setItem("token","");
  }

  loadOptionalForm() {
    this.hello=0;
    this.propose=1;
  }
}
