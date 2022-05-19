import { Component, OnInit } from '@angular/core';
import {HttpClient, HttpHeaders} from "@angular/common/http";
import {map} from "rxjs";
import {animate, state, style, transition, trigger} from '@angular/animations';
import {FormControl, FormGroup, Validators} from "@angular/forms";
import {parseJson} from "@angular/cli/utilities/json-file";


interface Course {
  courseID: number,
  name:string
  department:string,
  year:number,
  semester:number,
  credits:number,
  courseType:string,
  TeacherName: string,
  CourseName: string
}
interface Student {
  userID:number,
  Name:string,
  department:string,
  year:number,
  groupp:number
}
@Component({
  selector: 'app-grade-student',
  templateUrl: './grade-student.component.html',
  styleUrls: ['./grade-student.component.css'],
  animations: [
    trigger('detailExpand', [
      state('collapsed', style({height: '0px', minHeight: '0'})),
      state('expanded', style({height: '*'})),
      transition('expanded <=> collapsed', animate('225ms cubic-bezier(0.4, 0.0, 0.2, 1)')),
    ]),
  ],
})
export class GradeStudentComponent implements OnInit {
  gradeForm: FormGroup
  courseSelected: number;
  courseID:number;
  courses: Course[];
  students: Student[];
  invalidGrade: number;
  displayedStudentColumns: string[] = ['id', 'Name', 'Year', 'Group','Department'];



  constructor(private http: HttpClient) {
    this.courseSelected=-1;
    this.gradeForm = new FormGroup({
      grade: new FormControl('', [Validators.required]),
      weight: new FormControl('', [Validators.required])
    })
  }
  expandedElement: Student | null;

  ngOnInit(): void {
    const token = localStorage.getItem('token');
    const tokenise = "Bearer " + token;
    const headers = new HttpHeaders().set("Authorization", tokenise);
    const httpOptions = {
      headers: headers
    };
    this.http.get('https://localhost:4200/api/teacher/get_Courses',httpOptions).pipe(map(responseData => {
      const postArr = []
      for (const key in responseData) {
        if (responseData.hasOwnProperty(key)) {
          postArr.push((responseData as any)[key])
        }
      }
      return postArr;
    })).subscribe(posts => this.courses = posts)
    this.invalidGrade=0;
  }

  loadStudents() {
    const token = localStorage.getItem('token');

    const tokenise = "Bearer " + token;


    const headers = new HttpHeaders().set("Authorization", tokenise);

    const httpOptions = {
      headers: headers
    };
    this.courseSelected=this.courseID;
    const url = 'https://localhost:4200/api/teacher/getByCourseID/' + this.courseSelected;
    console.log(url);

    this.http.get(url,httpOptions).pipe(map(responseData => {
      const postArr = []
      for (const key in responseData) {
        if (responseData.hasOwnProperty(key)) {
          postArr.push((responseData as any)[key])
        }
      }
      return postArr;
    })).subscribe(posts => this.students = posts)
    this.courseSelected=1;
  }

  gradeStudent(userID: any) {

    const gradeData={
      studentID: userID,
      courseID: this.courseID,
      value:this.gradeForm.controls['grade'].value,
      weight:this.gradeForm.controls['weight'].value
    }
    const token = localStorage.getItem('token');
    const tokenise = "Bearer " + token;
    const headers = new HttpHeaders().set("Authorization", tokenise);
    const httpOptions = {
      headers: headers
    };
    this.http.post('https://localhost:4200/api/teacher/grade_Student',gradeData,httpOptions).subscribe(response =>{
      console.log(response);
      if(response=="Student Graded!")
        this.courseSelected=-1;
      this.gradeForm.controls['grade'].setValue("")
      weight:this.gradeForm.controls['weight'].setValue("")
    })

  }


  changeCourse(value: any) {
    this.courseID=value;
  }
}
