import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import {HttpClient, HttpHeaders} from "@angular/common/http";

@Component({
  selector: 'app-staff-profile',
  templateUrl: './staff-profile.component.html',
  styleUrls: ['./staff-profile.component.css']
})
export class StaffProfileComponent implements OnInit {

  name: any;
  id:any;
  hello: number;
  manage: number;
  form: number;
  updatePfinfo: number;
  constructor(private router:Router,private http: HttpClient) {

  }

  ngOnInit(): void {
    this.hello=1;
    this.manage=0;
    this.form=0;
    this.updatePfinfo=0;
    var token = localStorage.getItem('token');

    var tokenise = "Bearer " + token;


    var headers = new HttpHeaders().set("Authorization", tokenise);

    const httpOptions = {
      headers: headers
    };

    this.http.get('https://localhost:4200/api/staff/get_Staff',httpOptions)
      .subscribe(response => {
        var staff = Object.values(response)[0];
        console.log(staff);
        this.name= staff["Name"];
        this.id=staff["userID"];
      })
    this.name="name"
  }

  load_manage_students(){
    this.hello=0;
    this.form=0;
    this.updatePfinfo=0;
    this.manage=1;
    //this.router.navigateByUrl('manage-students-component');
  }

  logout() {
    localStorage.setItem("token", "");
    this.router.navigateByUrl("");
  }

  load_student_form() {
    this.hello=0;
    this.manage=0;
    this.updatePfinfo=0;
      this.form=1
  }

  updateProfileInfo() {
    this.hello=0
    this.manage=0
    this.form=0
    this.updatePfinfo=1

  }
}
