import { Component, OnInit } from '@angular/core';
import {FormControl, FormGroup, Validators} from "@angular/forms";
import {Router} from "@angular/router";
import {HttpClient, HttpHeaders} from "@angular/common/http";

@Component({
  selector: 'app-optionals-form',
  templateUrl: './optionals-form.component.html',
  styleUrls: ['./optionals-form.component.css']
})
export class OptionalsFormComponent implements OnInit {
  optionalForm: FormGroup;
  optionals:any[]

  constructor(private router: Router, private http: HttpClient) { }

  ngOnInit(): void {
    var token = localStorage.getItem('token');

    var tokenise = "Bearer " + token;


    var headers = new HttpHeaders().set("Authorization", tokenise);

    const httpOptions = {
      headers: headers
    };

    this.http.get('https://localhost:4200/api/teacher/get_Proposed', httpOptions
    )
      .subscribe(response => {

        var res = Object.values(response);
        this.optionals=res;
        this.optionals.forEach(element => console.log(element));
      })


    this.optionalForm = new FormGroup({
      name : new FormControl('', [Validators.required]),
      year: new FormControl('', [Validators.required]),
      semester : new FormControl('', [Validators.required]),
      credits: new FormControl('', [
        Validators.required
      ])


  })
  }

  proposeOptional() {
    const optionalData = {
      CourseName: this.optionalForm.value.name,
      year: this.optionalForm.value.year,
      semester: this.optionalForm.value.semester,
      credits: this.optionalForm.value.credits,
    }
   if(optionalData.CourseName == "" || optionalData.year == "" || optionalData.semester == "" || optionalData.credits == "")
     alert("invalid data")

    else if(optionalData.credits>7 || optionalData.credits<1){
      alert("invalid credits")
   }
    else {

     var token = localStorage.getItem('token');

     var tokenise = "Bearer " + token;


     var headers = new HttpHeaders().set("Authorization", tokenise);

     const httpOptions = {
       headers: headers
     };

     this.http.post('https://localhost:4200/api/teacher/propose', optionalData, httpOptions
     )
       .subscribe(response => {
          alert(response)
         
         this.http.get('https://localhost:4200/api/teacher/get_Proposed', httpOptions
         )
           .subscribe(response => {

             var res = Object.values(response);
             this.optionals=res;
             this.optionals.forEach(element => console.log(element));
           })
       })

   }

  }
}
