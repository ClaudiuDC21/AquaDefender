import { Component } from '@angular/core';
import { NavigationExtras, Router } from '@angular/router';
import { AuthenticationService } from '../../services/authentication.service';
import { IconService } from '../../../utils/services/icon.service';
import { ReportService } from '../../../report/services/report.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss',
})
export class LoginComponent {
  email: string = '';
  password: string = '';
  isLoading = false;
  showPassword: boolean = false;

  alertErrorMessages: string[] = [];

  constructor(
    private authenticationService: AuthenticationService,
    private router: Router,
    private reportService: ReportService,
    public iconService: IconService
  ) {}

  removeAlert(index: number): void {
    this.alertErrorMessages.splice(index, 1); // Îndepărtează mesajul de eroare la indexul specificat
  }

  togglePasswordVisibility() {
    this.showPassword = !this.showPassword;
  }

  closeForm() {
    this.router.navigate(['/home']); // Navighează înapoi la pagina principală sau la o altă pagină.
  }

  login() {
    this.isLoading = true;
    const user = {
      email: this.email,
      password: this.password,
    };

    this.authenticationService.login(user).subscribe(
      () => {
        // Login successful
        console.log('Login successful');

        // Obține cityId și convertește-l la număr întreg
        const cityIdStr = this.authenticationService.getCityId();
        const cityId = cityIdStr ? parseInt(cityIdStr, 10) : 0;

        if (this.authenticationService.isNotUser() && cityId && !isNaN(cityId)) {
          this.reportService.getNewReportsByCityId(cityId).subscribe(
            (reportCount: number) => {
              this.isLoading = false;
              const navigationExtras: NavigationExtras = {
                queryParams: { 
                  message: 'Autentificare reușită!', 
                  reportCount: reportCount // Adaugă numărul de rapoarte noi
                }
              };
              this.router.navigate(['/home'], navigationExtras);
            },
            (error) => {
              this.isLoading = false;
              const errorMessage = 'Autentificare eșuată: ' + error;
              console.error(errorMessage);
              this.alertErrorMessages.push(errorMessage);
              const navigationExtras: NavigationExtras = {
                queryParams: { message: 'Autentificare eșuată!' }
              };
              this.router.navigate(['/home'], navigationExtras);
            }
            
          );
        } else {
          this.isLoading = false;
          const navigationExtras: NavigationExtras = {
            queryParams: { message: 'Autentificare reușită!' }
          };
          this.router.navigate(['/home'], navigationExtras);
        }
      },
      (error: { error: any }) => {
        this.isLoading = false;
        const errorMessage = 'Autentificare eșuată: ' + error.error;
        console.error(errorMessage);
        this.alertErrorMessages.push(errorMessage);
      }
      
    );
  }
  onOutsideClick() {
    this.router.navigate(['/home']);
  }
}
