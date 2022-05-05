import { Component, OnInit } from '@angular/core';
import {FormControl, FormGroup, Validators} from "@angular/forms";
import {HttpClient, HttpErrorResponse} from "@angular/common/http";
import {Router} from "@angular/router";

@Component({
  selector: 'app-login-form',
  templateUrl: './login-form.component.html',
  styleUrls: ['./login-form.component.css']
})
export class LoginFormComponent implements OnInit {
  iconImg:string = "src\\assets\\img\\img.png"
  public loginForm: FormGroup

  public invalidLogin: number = 0
  public closedServer: number = 0

  constructor(private router: Router, private http: HttpClient) { }

  ngOnInit(): void {
    this.loginForm = new FormGroup({
      username : new FormControl('', [Validators.required]),
      password: new FormControl('', [
        Validators.required,
        Validators.minLength(4),
      ])
    })
  }


  authenticateUser() {

    const loginData = {
      userId: this.loginForm.value.username,
      password: this.loginForm.value.password
    }

    this.http.post('https://localhost:4200/api/authenticate', loginData)
      .subscribe(response => {
        var res = Object.values(response)
        console.log(res);
        localStorage.setItem('token', res[3]);
        localStorage.setItem('account', res[1]);
        this.invalidLogin = 0;
        this.closedServer = 0;
        if (res[2] == "staff")
          this.router.navigateByUrl('staff-component')
        else if (res[2] == "student")
          this.router.navigateByUrl('student-component')
        else if (res[2] == "teacher")
          this.router.navigateByUrl('teacher-component')
      },
        error => {
          if (error.status == 401) {
            this.invalidLogin = 1;
            this.closedServer = 0;
          }
          if (error.status == 504)
            this.closedServer = 1;
        });
   }
}
