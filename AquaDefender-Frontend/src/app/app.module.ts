import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HttpClientModule } from '@angular/common/http';
import { LoginComponent } from './authentication/components/login/login.component';
import { FormsModule } from '@angular/forms';
import { SignupComponent } from './authentication/components/signup/signup.component';
import { ReportComponent } from './report/components/report-creation/report.component';
import { WaterValuesComponent } from './water-values/water-values.component';
import { PersonalProfileComponent } from './profile/components/personal-profile/personal-profile.component';
import { EditProfileComponent } from './profile/components/edit-profile/edit-profile.component';
import { LoadingComponent } from './shared/loading/loading.component';
import { AlertErrorComponent } from './shared/alert-error/alert-error.component';
import { AlertSuccessComponent } from './shared/alert-success/alert-success.component';
import { AlertInfoComponent } from './shared/alert-info/alert-info.component';
import { AlertWarningComponent } from './shared/alert-warning/alert-warning.component';
import { HomeComponent } from './home/home.component';
import { ReportManagementComponent } from './report/components/report-management/report-management.component';
import { ChangePasswordComponent } from './profile/components/change-password/change-password.component';
import { ScrollDirective } from './shared/directives/scroll.directive';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { CustomNumberCoordonatePipe } from './shared/pipes/custom-number-coordonate.pipe';
import { NavbarComponent } from './core/navbar/navbar.component';
import { FooterComponent } from './core/footer/footer.component';
import { DatePipe } from '@angular/common';

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    HomeComponent,
    SignupComponent,
    ReportComponent,
    WaterValuesComponent,
    PersonalProfileComponent,
    EditProfileComponent,
    LoadingComponent,
    AlertErrorComponent,
    AlertSuccessComponent,
    AlertInfoComponent,
    AlertWarningComponent,
    ReportManagementComponent,
    ChangePasswordComponent,
    ScrollDirective,
    CustomNumberCoordonatePipe,
    NavbarComponent,
    FooterComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule,
    FontAwesomeModule,
  ],
  providers: [DatePipe],
  bootstrap: [AppComponent],
})
export class AppModule {}
