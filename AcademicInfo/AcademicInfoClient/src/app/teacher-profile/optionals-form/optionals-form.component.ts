import { Component, OnInit } from '@angular/core';
import {FormControl, FormGroup, Validators} from "@angular/forms";

@Component({
  selector: 'app-optionals-form',
  templateUrl: './optionals-form.component.html',
  styleUrls: ['./optionals-form.component.css']
})
export class OptionalsFormComponent implements OnInit {
  optionalForm: FormGroup;

  constructor() { }

  ngOnInit(): void {
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
      name: this.optionalForm.value.name,
      year: this.optionalForm.value.year,
      semester: this.optionalForm.value.semester,
      credits: this.optionalForm.value.credits,
    }
   if(optionalData.name == "" || optionalData.year == "" || optionalData.semester == "" || optionalData.credits == "")
     alert("invalid data")

    else if(optionalData.credits>7 || optionalData.credits<1){
      alert("invalid credits")
   }
    else {

   }

  }
}
