import { Component, OnInit } from '@angular/core';

interface Course {
  name:string
  department:string,
  year:number,
  semester:number,
  credits:number,
  courseType:string,
  teacher:string
}
const ELEMENT_DATA: Course[] = [
  {name: "Course1", department: 'Dep', year:1,semester:1,credits:5,courseType:"mandatory",teacher:"teacher name"},
  {name: "Course1", department: 'Dep', year:1,semester:1,credits:5,courseType:"mandatory",teacher:"teacher name"},
  {name: "Course1", department: 'Dep', year:1,semester:1,credits:5,courseType:"mandatory",teacher:"teacher name"},
  {name: "Course1", department: 'Dep', year:1,semester:1,credits:5,courseType:"mandatory",teacher:"teacher name"}
];

@Component({
  selector: 'app-courses-tab',
  templateUrl: './courses-tab.component.html',
  styleUrls: ['./courses-tab.component.css']
})
export class CoursesTabComponent implements OnInit {
  columns = [
    {
      columnDef: 'name',
      header: 'Course',
      cell: (element: Course) => `${element.name}`,
    },
    {
      columnDef: 'department',
      header: 'Department',
      cell: (element: Course) => `${element.department}`,
    },
    {
      columnDef: 'year',
      header: 'Year',
      cell: (element: Course) => `${element.year}`,
    },
    {
      columnDef: 'semester',
      header: 'Semester',
      cell: (element: Course) => `${element.semester}`,
    },
    {
      columnDef: 'credits',
      header: 'Credits',
      cell: (element: Course) => `${element.credits}`,
    },
    {
      columnDef: 'courseType',
      header: 'Course Type',
      cell: (element: Course) => `${element.courseType}`,
    },
    {
      columnDef: 'teacher',
      header: 'Teacher',
      cell: (element: Course) => `${element.teacher}`,
    },
  ];
  dataSource = ELEMENT_DATA;
  displayedColumns = this.columns.map(c => c.columnDef);

  constructor() { }

  ngOnInit(): void {
  }

}
