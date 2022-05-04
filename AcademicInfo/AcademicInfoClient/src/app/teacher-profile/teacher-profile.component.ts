import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-teacher-profile',
  templateUrl: './teacher-profile.component.html',
  styleUrls: ['./teacher-profile.component.css']
})
export class TeacherProfileComponent implements OnInit {
  name: any;

  constructor() { }

  ngOnInit(): void {
    this.name=localStorage.getItem("account");
  }

}
