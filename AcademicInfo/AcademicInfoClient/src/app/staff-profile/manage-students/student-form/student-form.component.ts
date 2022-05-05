import { Component, OnInit } from '@angular/core';
import {AbstractControl, FormControl, FormGroup, ValidationErrors, ValidatorFn, Validators} from "@angular/forms";
import {HttpClient, HttpHeaders} from "@angular/common/http";
import {Router} from "@angular/router";
import { concat } from 'rxjs';
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
      Name: new FormControl('', [Validators.required, this.customValidator(/[^a-zA-Z]/i)]),
      Department: new FormControl('', [Validators.required, this.customValidator(/[^ 0 - 9]/i)]),
      Year: new FormControl('', [Validators.required, this.customValidator(/[^1 - 3]/i)]),
      Group: new FormControl('', [Validators.required, this.customValidator(/(^9[1 - 3][1 - 7])/i)])
    })
  }
  customYearValidator(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      var rex = /^[0 - 9]/i;
      return (rex.test(control.value) || control.value < 1 || control.value > 3) ? control.value : null;
    }
  }
  customValidator(nameRe: RegExp): ValidatorFn {
  return (control: AbstractControl): ValidationErrors | null => {
    const forbidden = nameRe.test(control.value);
    return forbidden ? { forbiddenName: { value: control.value } } : null;
    };
}
  addStudent() {
    const studentData = {

      department : this.studentForm.value.Department,
      name : this.studentForm.value.Name,
      year : this.studentForm.value.Year,
      group: this.studentForm.value.Group
    }

    var token = localStorage.getItem('token');

    var tokenise = "Bearer " + token;


    var headers = new HttpHeaders().set("Authorization", tokenise);

    const httpOptions = {
      headers: headers
    };

    this.http.post('https://localhost:4200/api/staff/add_Student', studentData, httpOptions
    )
      .subscribe(response => {
        console.log('post response ', response);
        var res = Object.values(response);
        if(res[0]=="User Authenticated Successfully!")
          alert("Student added!")
        else
        {
          alert("Invalid student")
        }
      })
   // this.router.navigateByUrl('manage-students-component')
  }



}
