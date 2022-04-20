import { Component, OnInit } from '@angular/core';
import {FormControl, FormGroup, Validators} from "@angular/forms";
import {HttpClient} from "@angular/common/http";

@Component({
  selector: 'app-student-profile',
  templateUrl: './student-profile.component.html',
  styleUrls: ['./student-profile.component.css']
})
export class StudentProfileComponent implements OnInit {
  public addStudentForm: FormGroup


  constructor(private http: HttpClient) { }

  ngOnInit(): void {
    this.addStudentForm = new FormGroup({
      userId : new FormControl('', [Validators.required]),
      department : new FormControl('', [Validators.required]),
      name : new FormControl('', [Validators.required]),
      year : new FormControl('', [Validators.required]),
      group: new FormControl('', [Validators.required])
    })
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
}
