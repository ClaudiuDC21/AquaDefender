import { Component, OnInit } from '@angular/core';
import { NavigationExtras, Router } from '@angular/router';
import { AuthenticationService } from '../../services/authentication.service';
import { LocationService } from '../../../utils/location.service';
import { IconService } from '../../../utils/icon.service';

@Component({
  selector: 'app-signup',
  templateUrl: './signup.component.html',
  styleUrl: './signup.component.scss',
})
export class SignupComponent implements OnInit {
  username: string = '';
  email: string = '';
  phone: string = '';
  county: string = '';
  city: string = '';
  password: string = '';
  confirmPassword: string = '';
  errorMessage: string = '';
  counties: any[] = []; // To store counties
  cities: any[] = [];
  selectedCountyIndex: number | null = null; // Holds the selected index
  isLoading = false;

  alertErrorMessages: string[] = [];
  alertSuccessMessages: string[] = [];
  alertInfoMessages: string[] = [];
  alertWarningMessages: string[] = [];

  constructor(
    private router: Router,
    private authenticationService: AuthenticationService,
    private locationService: LocationService,
    public iconService: IconService
  ) {}

  ngOnInit(): void {
    this.loadCounties();
    setTimeout(() => {
      this.alertWarningMessages.push('Nu uita să apeși pe butonul de Înregistrare după ce ai introdus toate datele corect');
    }, 25000);
  }

  closeForm() {
    this.router.navigate(['/home']);
  }
  
  passwordsMatch(): boolean {
    return this.password === this.confirmPassword;
  }

  removeAlert(index: number): void {
    this.alertErrorMessages.splice(index, 1); // Îndepărtează mesajul de eroare la indexul specificat
  }

  removeSuccessAlert(index: number): void {
    this.alertSuccessMessages.splice(index, 1); // Îndepărtează mesajul de eroare la indexul specificat
  }

  removeInfoAlert(index: number): void {
    this.alertInfoMessages.splice(index, 1); // Remove the info message at the specified index
  }
  
  removeWarningAlert(index: number): void {
    this.alertWarningMessages.splice(index, 1); // Remove the warning message at the specified index
  }

  loadCounties(): void {
    this.isLoading = true;
    this.locationService.getAllCounties().subscribe({
      next: (data) => {
        this.counties = data;
        this.isLoading = false;
      },
      error: (error) => {
        const errorMessage = 'A apărut o eroare la preluarea județelor: ' + error;
        console.error(errorMessage);
        this.alertErrorMessages.push(errorMessage);
        this.isLoading = false;
      }
    });
  }
  
  onCountyChange(): void {
    this.city = ''; // Resetează selecția orașului
    if (this.county) {
      const countyId = +this.county;
      this.locationService.getAllCitiesByCountyId(countyId).subscribe({
        next: (data) => {
          this.cities = data;
        },
        error: (error) => {
          const errorMessage = 'A apărut o eroare la preluarea orașelor pentru județul selectat: ' + error;
          console.error(errorMessage);
          this.alertErrorMessages.push(errorMessage);
        }
      });
    }
  }

  signup() {
    this.isLoading = true;
    if (!this.passwordsMatch()) {
      const errorMessage = 'Parolele nu se potrivesc. Vă rugăm să încercați din nou.';
      this.errorMessage = errorMessage;
      this.alertErrorMessages.push(errorMessage);
      this.isLoading = false; // Resetează isLoading când parolele nu se potrivesc
      return;
    }
    

    const user = {
      UserName: this.username,
      Email: this.email,
      PhoneNumber: this.phone,
      County: this.county,
      City: this.city,
      Password: this.password,
    };

    this.authenticationService.register(user).subscribe(
      (response) => {
        console.log('Registration successful');
        this.isLoading = false;
        const navigationExtras: NavigationExtras = {
          queryParams: { message: 'Înregistrare reușită!' }
        };
        this.router.navigate(['/home'], navigationExtras);
      },
      (error) => {
        this.isLoading = false;
        const errorMessage = 'Înregistrarea a eșuat. Vă rugăm să verificați datele introduse și să încercați din nou.';
        console.error('Înregistrarea a eșuat:', error.error);
        this.errorMessage = errorMessage;
        this.alertErrorMessages.push(errorMessage);
      }
      
    );
  }

  onOutsideClick() {
    this.router.navigate(['/home']);
  }
}
