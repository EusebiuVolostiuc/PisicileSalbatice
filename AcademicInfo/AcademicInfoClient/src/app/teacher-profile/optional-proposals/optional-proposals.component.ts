import { Component, OnInit } from '@angular/core';
import {animate, state, style, transition, trigger} from "@angular/animations";
import {HttpClient, HttpHeaders} from "@angular/common/http";
import {map} from "rxjs";
import {FormControl, FormGroup, Validators} from "@angular/forms";
interface Course {
  courseID: number,
  department:string,
  year:number,
  semester:number,
  credits:number,
  TeacherName: string,
  CourseName: string
  maxStudents:number;
}

@Component({
  selector: 'app-optional-proposals',
  templateUrl: './optional-proposals.component.html',
  styleUrls: ['./optional-proposals.component.css'],
  animations: [
    trigger('detailExpand', [
      state('collapsed', style({height: '0px', minHeight: '0'})),
      state('expanded', style({height: '*'})),
      transition('expanded <=> collapsed', animate('225ms cubic-bezier(0.4, 0.0, 0.2, 1)')),
    ]),
  ]
})
export class OptionalProposalsComponent implements OnInit {
  optionals: Course[];
  columns: string[] = ['department', 'year','semester','credits','CourseName','maxStudents','TeacherName'];
  expandedElement: Course | null;
  optionalForm: FormGroup;

  constructor(private http:HttpClient) {
    this.optionalForm = new FormGroup({
      maxStudents: new FormControl('', [Validators.required])
    })
  }

  ngOnInit(): void {
    const token = localStorage.getItem('token');
    const tokenise = "Bearer " + token;
    const headers = new HttpHeaders().set("Authorization", tokenise);
    const httpOptions = {
      headers: headers
    };
    this.http.get('https://localhost:4200/api/teacher/get_Optionals', httpOptions)
      .subscribe(response => {
        var courses_ = Object.values(response)
        let postArr: any[];
        postArr = [];
        courses_.forEach(element => postArr.push(element));
        this.optionals = postArr;
        console.log(this.optionals);
      })
  }

  submitOptional(CourseName: any) {
    const token = localStorage.getItem('token');
    const tokenise = "Bearer " + token;
    const headers = new HttpHeaders().set("Authorization", tokenise);
    const httpOptions = {
      headers: headers
    };
    var no =this.optionalForm.controls['maxStudents'].value
    const url = 'https://localhost:4200/api/teacher/setMaxNumberOfStud/'+no+"/"+CourseName
    this.http.put(url,httpOptions).subscribe(response=>{
      console.log(response);
    })
  }


}
