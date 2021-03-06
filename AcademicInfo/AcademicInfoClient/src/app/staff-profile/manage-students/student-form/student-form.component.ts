import { Component, OnInit } from '@angular/core';
import {FormControl, FormGroup, Validators} from "@angular/forms";
import {HttpClient} from "@angular/common/http";
import {Router} from "@angular/router";
@Component({
  selector: 'app-student-form',
  templateUrl: './student-form.component.html',
  styleUrls: ['./student-form.component.css']
})
export class StudentFormComponent implements OnInit {
  public studentForm: FormGroup


  constructor(private http: HttpClient, private router:Router) { }

  ngOnInit(): void {
    this.studentForm = new FormGroup({
      UserID: new FormControl('', [Validators.required]),
      Name : new FormControl('', [Validators.required]),
      Department : new FormControl('', [Validators.required]),
      Year : new FormControl('', [Validators.required]),
      Group : new FormControl('', [Validators.required])
  })
}
  addStudent() {
    const studentData = {
      userId : this.studentForm.value.UserID,
      department : this.studentForm.value.Department,
      name : this.studentForm.value.Name,
      year : this.studentForm.value.Year,
      group: this.studentForm.value.Group
    }
    this.http.post('https://localhost:4200/api/student', studentData)
      .subscribe(response => {
        console.log('post response ', response);
      })
    this.router.navigateByUrl('manage-students-component')
  }



}
