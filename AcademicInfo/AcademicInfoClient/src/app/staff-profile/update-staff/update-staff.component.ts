import { Component, OnInit } from '@angular/core';
import {TeacherProfileComponent} from "../../teacher-profile/teacher-profile.component";
import {HttpClient, HttpHeaders} from "@angular/common/http";
import {StaffProfileComponent} from "../staff-profile.component";

@Component({
  selector: 'app-update-staff',
  templateUrl: './update-staff.component.html',
  styleUrls: ['./update-staff.component.css']
})
export class UpdateStaffComponent implements OnInit {
  staff: any;

  constructor(private s:StaffProfileComponent,private http: HttpClient) {

  }

  ngOnInit(): void {
    this.staff={
      "name":this.s.name
    }
  }

  updateStaff() {
    const staffData = {
      name : this.staff.name
    }

    var token = localStorage.getItem('token');

    var tokenise = "Bearer " + token;


    var headers = new HttpHeaders().set("Authorization", tokenise);

    const httpOptions = {
      headers: headers
    };

    this.http.put('https://localhost:4200/api/staff',staffData,httpOptions)
      .subscribe(response => {
        console.log(response);
        location.reload();
      })

  }

  revertChanges() {
    this.staff={
      "name":this.s.name
    }
  }
}
