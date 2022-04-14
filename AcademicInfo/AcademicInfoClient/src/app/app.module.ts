import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { Routes, RouterModule } from '@angular/router';
import { FlexLayoutModule } from '@angular/flex-layout';
import {MatFormFieldModule} from '@angular/material/form-field';
import {MatInputModule} from '@angular/material/input';
import {MatButtonModule} from '@angular/material/button';
import {MatCardModule} from '@angular/material/card';
import {MatToolbarModule} from '@angular/material/toolbar';
import {MatTableModule} from '@angular/material/table';
import {MatSidenavModule} from '@angular/material/sidenav';

import { AppComponent } from './app.component';
import { LoginFormComponent } from './login-form/login-form.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { StudentProfileComponent } from './student-profile/student-profile.component';
import { StaffProfileComponent } from './staff-profile/staff-profile.component';
import { ManageStudentsComponent } from './staff-profile/manage-students/manage-students.component';
import { StudentFormComponent } from './staff-profile/manage-students/student-form/student-form.component';


const routes: Routes = [
  {path:'', component:StudentFormComponent}
];
@NgModule({
  declarations: [
    AppComponent,
    LoginFormComponent,
    StudentProfileComponent,
    StaffProfileComponent,
    ManageStudentsComponent,
    StudentFormComponent
  ],
  imports: [
    RouterModule.forRoot(routes), BrowserModule, HttpClientModule, BrowserAnimationsModule,FlexLayoutModule,
    MatFormFieldModule,MatInputModule,MatButtonModule,MatCardModule,
    MatToolbarModule,MatTableModule,MatSidenavModule
  ],
  providers: [],
  bootstrap: [AppComponent],
  exports: [RouterModule]
})
export class AppModule { }
