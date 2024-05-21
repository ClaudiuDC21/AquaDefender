import { Component, NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './authentication/components/login/login.component';
import { HomeComponent } from './home/home.component';
import { SignupComponent } from './authentication/components/signup/signup.component';
import { ReportComponent } from './report/components/report-creation/report.component';
import { WaterValuesComponent } from './water-values/water-values.component';
import { PersonalProfileComponent } from './profile/components/personal-profile/personal-profile.component';
import { EditProfileComponent } from './profile/components/edit-profile/edit-profile.component';
import { ReportManagementComponent } from './report/components/report-management/report-management.component';
import { ChangePasswordComponent } from './profile/components/change-password/change-password.component';
import { NavbarComponent } from './core/navbar/navbar.component';
const routes: Routes = [
  { path: '', redirectTo: 'home', pathMatch: 'full' },
  { path: 'home', component: HomeComponent },
  { path: 'login', component: LoginComponent },
  { path: 'signup', component: SignupComponent },
  { path: 'report-a-problem', component: ReportComponent },
  { path: 'report-management/:cityId/details', component: ReportManagementComponent },
  { path: 'water-values/:userId/info', component: WaterValuesComponent },
  { path: 'personal-profile/:userId/info', component: PersonalProfileComponent},
  { path: 'edit-profile/:userId', component: EditProfileComponent},
  { path: 'change-password/:userId', component: ChangePasswordComponent},
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
