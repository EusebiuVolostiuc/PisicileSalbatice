import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-staff-profile',
  templateUrl: './staff-profile.component.html',
  styleUrls: ['./staff-profile.component.css']
})
export class StaffProfileComponent implements OnInit {

  showFiller = false;
  constructor(private router: Router) { 

  }

  ngOnInit(): void {
  }

  load_manage_students(){
    this.router.navigateByUrl('manage-students-component');
  }

}
