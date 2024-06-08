import { Component, OnInit } from '@angular/core';
import { AuthenticationService } from '../../../authentication/services/authentication.service';
import { LocationService } from '../../../utils/services/location.service';
import { User } from '../../models/user.model';
import { UserService } from '../../services/user.service';
import { NavigationEnd, NavigationExtras, Router } from '@angular/router';
import { ViewportScroller } from '@angular/common';
import { filter } from 'rxjs';
import { IconService } from '../../../utils/services/icon.service';

@Component({
  selector: 'app-edit-profile',
  templateUrl: './edit-profile.component.html',
  styleUrl: './edit-profile.component.scss',
})
export class EditProfileComponent implements OnInit {
  minDate: string = '';
  maxDate: string = '';
  isLoading = false;
  profileImagePreview: string | null = null;
  profileImageFile: File | null = null;

  counties: any[] = [];
  cities: any[] = [];
  countyName: string = '';
  cityName: string = '';
  user: User = new User();

  alertErrorMessages: string[] = [];
  alertSuccessMessages: string[] = [];

  constructor(
    private locationService: LocationService,
    private authenticationService: AuthenticationService,
    private userService: UserService,
    private router: Router,
    private viewportScroller: ViewportScroller,
    public iconService: IconService
  ) {
    this.router.events
      .pipe(filter((event) => event instanceof NavigationEnd))
      .subscribe(() => {
        this.viewportScroller.scrollToPosition([0, 0]);
      });
  }

  get isAuthenticated() {
    return this.authenticationService.getAuthStatus();
  }

  isCityHallEmployee(): boolean {
    return this.authenticationService.isCityHallEmployee();
  }

  removeAlert(index: number): void {
    this.alertErrorMessages.splice(index, 1);
  }

  removeSuccessAlert(index: number): void {
    this.alertSuccessMessages.splice(index, 1);
  }

  ngOnInit(): void {
    this.calculateDateLimits();
    this.loadCounties();
  }

  loadCounties(): void {
    this.isLoading = true;
    this.locationService.getAllCounties().subscribe({
      next: (data) => {
        this.counties = data;
        this.isLoading = false;
      },
      error: (error) => {
        const errorMessage =
          'A apărut o eroare la încărcarea județelor: ' +
          (error.message || error.error);
        console.error(errorMessage, error);
        this.alertErrorMessages.push(errorMessage);
        if (error.error) {
          this.alertErrorMessages.push(error.error);
        }
        this.isLoading = false;
      },
    });
  }

  onCountyChange(): void {
    this.cityName = '';
    if (this.countyName) {
      const countyId = +this.countyName;
      this.locationService.getAllCitiesByCountyId(countyId).subscribe({
        next: (data) => {
          this.cities = data;
        },
        error: (error) => {
          const errorMessage =
            'A apărut o eroare la încărcarea localităților pentru județul selectat: ' +
            (error.message || error);
          console.error(errorMessage, error.error);
          this.alertErrorMessages.push(errorMessage);
          if (error.error) {
            this.alertErrorMessages.push(error.error);
          }
        },
      });
    }
  }

  updateProfile(): void {
    console.log(this.user);
    const userId = this.authenticationService.getUserId();
    if (userId === null) {
      const errorMessage = 'ID-ul utilizatorului este invalid.';
      console.error(errorMessage);
      this.alertErrorMessages.push(errorMessage);
      this.isLoading = false;
      return;
    }

    this.isLoading = true;
    const formData = new FormData();

    if (this.user.userName) {
      formData.append('UserName', this.user.userName);
    }
    if (this.user.phoneNumber) {
      formData.append('PhoneNumber', this.user.phoneNumber);
    }
    if (this.user.birthDate) {
      const birthDate = new Date(this.user.birthDate);

      if (
        !isNaN(birthDate.getTime()) &&
        birthDate.toISOString().slice(0, 10) !==
          new Date().toISOString().slice(0, 10)
      ) {
        formData.append('BirthDate', birthDate.toISOString());
      } else if (isNaN(birthDate.getTime())) {
        const errorMessage = 'Data de naștere este invalidă.';
        console.error(errorMessage);
        this.alertErrorMessages.push(errorMessage);
      }
    }
    if (this.countyName) {
      formData.append('CountyId', this.countyName);
    }
    if (this.cityName) {
      formData.append('CityId', this.cityName);
    }

    if (this.profileImageFile) {
      formData.append('ProfilePicture', this.profileImageFile);
    }

    this.userService.updateUser(userId, formData).subscribe({
      next: () => {
        console.log('Profile updated successfully!', formData);

        if (this.user.userName) {
          this.authenticationService.removeUserName();
          this.authenticationService.setUserName(this.user.userName);
        }
        if (this.cityName) {
          this.authenticationService.removeCityId();
          this.authenticationService.setCityId(this.cityName);
        }

        this.isLoading = false;
        const successMessage = 'Profilul a fost actualizat cu succes.';
        this.alertSuccessMessages.push(successMessage);

        const navigationExtras: NavigationExtras = {
          queryParams: { message: successMessage },
        };
        this.router.navigate(
          ['/personal-profile', userId, 'info'],
          navigationExtras
        );
      },
      error: (error) => {
        const errorMessage = 'Actualizarea profilului a eșuat: ' + error.error;
        console.error( error.error);
        this.alertErrorMessages.push(errorMessage);

        this.isLoading = false;
      },
    });
  }

  private calculateDateLimits(): void {
    this.isLoading = true;
    const today = new Date();
    const hundredYearsAgo = new Date(
      today.getFullYear() - 100,
      today.getMonth(),
      today.getDate()
    );
    const fiveYearsAgo = new Date(
      today.getFullYear() - 5,
      today.getMonth(),
      today.getDate()
    );

    this.minDate = hundredYearsAgo.toISOString().split('T')[0];
    this.maxDate = fiveYearsAgo.toISOString().split('T')[0];
    this.isLoading = false;
  }

  handleProfileImageUpload(event: Event): void {
    const element = event.target as HTMLInputElement;
    const file = element.files ? element.files[0] : null;

    if (file) {
      try {
        if (this.profileImagePreview) {
          URL.revokeObjectURL(this.profileImagePreview);
        }

        this.profileImagePreview = URL.createObjectURL(file);
        this.profileImageFile = file;
      } catch (error: any) {
        const errorMessage =
          'Eroare la încărcarea imaginii de profil: ' +
          (error.message || error);
        console.error(errorMessage);
        this.alertErrorMessages.push(errorMessage);
      }
    }
  }

  removeProfileImage(): void {
    try {
      if (this.profileImagePreview) {
        URL.revokeObjectURL(this.profileImagePreview);
      }
      this.profileImageFile = null;
    } catch (error: any) {
      const errorMessage =
        'Eroare la eliminarea imaginii de profil: ' + (error.message || error);
      console.error(errorMessage, error.error);
      this.alertErrorMessages.push(errorMessage);
    }
  }

  ngOnDestroy(): void {
    try {
      if (this.profileImagePreview) {
        URL.revokeObjectURL(this.profileImagePreview);
      }
    } catch (error: any) {
      const errorMessage =
        'Eroare la eliberarea resurselor imaginii de profil: ' +
        (error.message || error.error);
      console.error(errorMessage, error.error);
      this.alertErrorMessages.push(errorMessage);
    }
  }

  getPreviewImage(imageFile: File): string {
    try {
      return URL.createObjectURL(imageFile);
    } catch (error: any) {
      const errorMessage =
        'Eroare la obținerea previzualizării imaginii: ' +
        (error.message || error.error);
      console.error(errorMessage, error.error);
      this.alertErrorMessages.push(errorMessage);
      return 'path/to/default/image.jpg'; 
    }
  }
}
