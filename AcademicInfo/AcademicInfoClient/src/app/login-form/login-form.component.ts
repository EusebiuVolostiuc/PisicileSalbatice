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
  public http: HttpClient

  constructor(private router:Router) { }

  ngOnInit(): void {
    this.loginForm = new FormGroup({
      username : new FormControl('', [Validators.required]),
      password : new FormControl('', [Validators.required])
    })
  }


  // authenticateUser() {
  // this.http.post('http://localhost/')
  // }
  authenticateUser() {
    this.router.navigateByUrl('staff-component')
  }
}
