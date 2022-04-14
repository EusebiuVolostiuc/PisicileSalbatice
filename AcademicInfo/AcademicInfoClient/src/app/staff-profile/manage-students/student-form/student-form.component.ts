import { Component, OnInit } from '@angular/core';
import {FormControl, FormGroup, Validators} from "@angular/forms";
@Component({
  selector: 'app-student-form',
  templateUrl: './student-form.component.html',
  styleUrls: ['./student-form.component.css']
})
export class StudentFormComponent implements OnInit {
  public studentForm: FormGroup
  

  constructor() { }

  ngOnInit(): void {
    this.studentForm = new FormGroup({
      Name : new FormControl('', [Validators.required]),
      Department : new FormControl('', [Validators.required]),
      Year : new FormControl('', [Validators.required]),
      Group : new FormControl('', [Validators.required])
  })
}
addStudent() {
  alert('e ok!');
}

}
