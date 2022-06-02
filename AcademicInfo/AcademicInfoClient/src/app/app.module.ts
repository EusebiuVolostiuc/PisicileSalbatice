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
import {MatTabsModule} from '@angular/material/tabs';

import { AppComponent } from './app.component';
import { LoginFormComponent } from './login-form/login-form.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { StudentProfileComponent } from './student-profile/student-profile.component';
import { StaffProfileComponent } from './staff-profile/staff-profile.component';
import { ManageStudentsComponent } from './staff-profile/manage-students/manage-students.component';
import { StudentFormComponent } from './staff-profile/manage-students/student-form/student-form.component';
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import { TeacherProfileComponent } from './teacher-profile/teacher-profile.component';
import { CoursesTabComponent } from './student-profile/courses-tab/courses-tab.component';
import { OptionalsTabComponent } from './student-profile/optionals-tab/optionals-tab.component';
import { OptionalsFormComponent } from './teacher-profile/optionals-form/optionals-form.component';
import {MatSelectModule} from "@angular/material/select";
import {MatListModule} from "@angular/material/list";
import {MatDialogModule, MatDialogRef} from "@angular/material/dialog";
import { UpdateProfileComponent } from './student-profile/update-profile/update-profile.component';
import { UpdateTeacherComponent } from './teacher-profile/update-teacher/update-teacher.component';
import { UpdateStaffComponent } from './staff-profile/update-staff/update-staff.component';
import { GradeStudentComponent } from './teacher-profile/grade-student/grade-student.component';
import { OptionalProposalsComponent } from './teacher-profile/optional-proposals/optional-proposals.component';


const routes: Routes = [
  {path:'', component:LoginFormComponent},
  { path: 'staff-component', component: StaffProfileComponent },
  { path: 'manage-students-component', component: ManageStudentsComponent },
  { path: 'student-form-component', component: StudentFormComponent },
  {path:'student-component',component:StudentProfileComponent},
  {path:'teacher-component',component:TeacherProfileComponent}
];
@NgModule({
  declarations: [
    AppComponent,
    LoginFormComponent,
    StudentProfileComponent,
    StaffProfileComponent,
    ManageStudentsComponent,
    StudentFormComponent,
    TeacherProfileComponent,
    CoursesTabComponent,
    OptionalsTabComponent,
    OptionalsFormComponent,
    UpdateProfileComponent,
    UpdateTeacherComponent,
    UpdateStaffComponent,
    GradeStudentComponent,
    OptionalProposalsComponent
  ],
    imports: [
        RouterModule.forRoot(routes), BrowserModule, HttpClientModule, BrowserAnimationsModule, FlexLayoutModule,
        MatFormFieldModule, MatInputModule, MatButtonModule, MatCardModule,
        MatToolbarModule, MatTableModule, MatSidenavModule, ReactiveFormsModule, MatTabsModule, MatSelectModule, MatDialogModule, MatListModule, FormsModule
    ],
  providers: [
    {
      provide: MatDialogRef,
      useValue: {}
    }
  ],
  bootstrap: [AppComponent],
  exports: [RouterModule]
})
export class AppModule { }
