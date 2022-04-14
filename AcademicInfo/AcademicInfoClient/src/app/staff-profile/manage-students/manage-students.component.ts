import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
export interface Student {
  position: number;
  name: string;
  year: number;
  group: number;
}

const ELEMENT_DATA: Student[] = [
  {position: 1, name: 'St1', year: 2, group: 925},
  {position: 2, name: 'st2', year: 1, group: 911},
  {position: 3, name: 'st3', year: 3, group: 935},
  {position: 4, name: 'st4', year: 1, group: 917},

];

@Component({
  selector: 'app-manage-students',
  templateUrl: './manage-students.component.html',
  styleUrls: ['./manage-students.component.css']
})
export class ManageStudentsComponent implements OnInit {

  displayedColumns: string[] = ['position', 'name', 'year', 'group'];
  dataSource = ELEMENT_DATA;

  constructor(private router: Router) { }

  ngOnInit(): void {
  }

  load_student_form(){
    this.router.navigateByUrl('student-form-component');
  }

}
