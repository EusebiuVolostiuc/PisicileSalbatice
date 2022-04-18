import { Component, OnInit } from '@angular/core';
import {FormControl, FormGroup, Validators} from "@angular/forms";
import {HttpClient} from "@angular/common/http";
import {Router} from "@angular/router";

@Component({
  selector: 'app-login-form',
  templateUrl: './login-form.component.html',
  styleUrls: ['./login-form.component.css']
})
export class LoginFormComponent implements OnInit {
  iconImg:string = "src\\assets\\img\\img.png"
  public loginForm: FormGroup
 
  constructor(private router: Router, private http: HttpClient) { }

  ngOnInit(): void {
    this.loginForm = new FormGroup({
      username : new FormControl('', [Validators.required]),
      password : new FormControl('', [Validators.required])
    })
  }


  authenticateUser() {

    const loginData = {
      userId: this.loginForm.value.username,
      password: this.loginForm.value.password
    }

    this.http.post('https://localhost:4200/api/authenticate', loginData)
      .subscribe(response => {
        console.log('post response ', response);
      })
   }
  //authenticateUser() {
  //    this.router.navigateByUrl('staff-component');
  //}
}
