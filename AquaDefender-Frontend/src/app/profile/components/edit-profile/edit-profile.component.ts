import { Component, OnInit } from '@angular/core';
import { AuthenticationService } from '../../../authentication/services/authentication.service';
import { LocationService } from '../../../utils/location.service';
import { User } from '../../models/user.model';
import { UserService } from '../../services/user.service';
import { NavigationEnd, Router } from '@angular/router';
import { ViewportScroller } from '@angular/common';
import { filter } from 'rxjs';
import { IconService } from '../../../utils/icon.service';

@Component({
  selector: 'app-edit-profile',
  templateUrl: './edit-profile.component.html',
  styleUrl: './edit-profile.component.scss',
})
export class EditProfileComponent implements OnInit {
  isDropdownOpen: boolean = false;
  minDate: string = ''; // pentru data nașterii - 100 ani
  maxDate: string = '';
  isLoading = false;
  profileImagePreview: string | null = null;
  profileImageFile: File | null = null;

  counties: any[] = []; // To store counties
  cities: any[] = [];
  countyName: string = '';
  cityName: string = '';
  user: User = new User();

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

  onLogout() {
    this.authenticationService.logout();
  }

  toggleDropdown(): void {
    this.isDropdownOpen = !this.isDropdownOpen;
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
        console.error('There was an error fetching the counties:', error);
      },
    });
  }

  onCountyChange(): void {
    this.cityName = ''; // Reset city selection
    if (this.countyName) {
      const countyId = +this.countyName;
      this.locationService.getAllCitiesByCountyId(countyId).subscribe({
        next: (data) => {
          this.cities = data;
        },
        error: (error) => {
          console.error(
            'There was an error fetching the cities for the selected county:',
            error
          );
        },
      });
    }
  }

  updateProfile(): void {
    console.log(this.user);
    const userId = this.authenticationService.getUserId();
    if (userId === null) {
      console.error('Invalid user ID');
      this.isLoading = false;
      return;
    }
    this.isLoading = true;
    const formData = new FormData();

    // Adăugăm fiecare câmp în formData numai dacă există o valoare validă (diferită de null și undefined)
    if (this.user.userName) {
      formData.append('UserName', this.user.userName);
    }
    if (this.user.phoneNumber) {
      formData.append('PhoneNumber', this.user.phoneNumber);
    }
    if (this.user.birthDate) {
      // Convert string to Date object
      const birthDate = new Date(this.user.birthDate);

      // Check if the date conversion is valid
      if (!isNaN(birthDate.getTime())) {
        formData.append('BirthDate', birthDate.toISOString());
      } else {
        console.error('Invalid date');
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
      },
      error: (error) => {
        console.error('Failed to update profile', error);
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
      this.profileImagePreview = URL.createObjectURL(file);
      this.profileImageFile = file; // Actualizează proprietatea cu fișierul nou
    }
    // Nu este necesar să setezi isLoading aici decât dacă procesezi imaginea
  }

  removeProfileImage(): void {
    // Dacă există un preview, revocă URL-ul blob pentru a elibera memoria
    if (this.profileImagePreview) {
      URL.revokeObjectURL(this.profileImagePreview);
    }
    this.profileImageFile = null; // Resetează și proprietatea fișierului
  }

  ngOnDestroy(): void {
    // Eliberează memoria dacă există un URL blob creat pentru preview-ul imaginii de profil
    if (this.profileImagePreview) {
      URL.revokeObjectURL(this.profileImagePreview);
    }
  }

  // handleImageUpload(event: any): void {
  //   const fileList: FileList | null = event.target.files;

  //   if (fileList) {
  //     this.report.imageFiles = Array.from(fileList);
  //   }
  // }

  // removeImage(index: number): void {
  //   this.report.imageFiles.splice(index, 1);
  // }

  getPreviewImage(imageFile: File): string {
    return URL.createObjectURL(imageFile);
  }
}
