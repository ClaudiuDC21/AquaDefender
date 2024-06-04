import { Component, OnInit } from '@angular/core';
import { AuthenticationService } from '../../../authentication/services/authentication.service';
import { Observable, filter, forkJoin, map, switchMap } from 'rxjs';
import { ReportService } from '../../../report/services/report.service';
import { User } from '../../models/user.model';
import { UserService } from '../../services/user.service';
import { LocationService } from '../../../utils/services/location.service';
import { NavigationEnd, NavigationExtras, Router } from '@angular/router';
import { ReportStatus } from '../../../report/enums/status';
import { SeverityLevel } from '../../../report/enums/severity';
import { Report } from '../../../report/models/report.model';
import { ViewportScroller } from '@angular/common';
import { IconService } from '../../../utils/services/icon.service';

@Component({
  selector: 'app-change-password',
  templateUrl: './change-password.component.html',
  styleUrl: './change-password.component.scss',
})
export class ChangePasswordComponent implements OnInit {
  isDropdownOpen: boolean = false;
  isLoading: boolean = false;

  alertErrorMessages: string[] = [];
  alertSuccessMessages: string[] = [];
  user = {
    oldPassword: '',
    newPassword: '',
    confirmNewPassword: '',
  };

  constructor(
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

  ngOnInit(): void {}

  getisAuthenticated() {
    return this.authenticationService.getAuthStatus();
  }

  getUserId() {
    return this.authenticationService.getUserId();
  }

  removeAlert(index: number): void {
    this.alertErrorMessages.splice(index, 1); // Îndepărtează mesajul de eroare la indexul specificat
  }

  removeSuccessAlert(index: number): void {
    this.alertSuccessMessages.splice(index, 1); // Îndepărtează mesajul de eroare la indexul specificat
  }

  newPasswordsMatch(): boolean {
    return this.user.newPassword === this.user.confirmNewPassword;
  }

  onLogout() {
    this.authenticationService.logout();
  }

  toggleDropdown(): void {
    this.isDropdownOpen = !this.isDropdownOpen;
  }

  changePassword(): void {
    console.log('Submitting:', this.user);

    if (
      !this.user.oldPassword ||
      !this.user.newPassword ||
      !this.user.confirmNewPassword
    ) {
      const errorMessage = 'Vă rugăm să completați toate câmpurile.';
      console.error(errorMessage);
      this.alertErrorMessages.push(errorMessage);
      return;
    }

    if (this.user.newPassword !== this.user.confirmNewPassword) {
      const errorMessage =
        'Noua parolă și confirmarea parolei nu se potrivesc.';
      console.error(errorMessage);
      this.alertErrorMessages.push(errorMessage);
      return;
    }

    // Assuming 'userId' is stored and retrieved securely, e.g., from a user service or auth service
    const userId = this.getUserId(); // Or however you obtain the user's ID

    console.log('Old Password:', this.user.oldPassword);
    console.log('New Password:', this.user.newPassword);
    console.log('Confirm New Password:', this.user.confirmNewPassword);
    if (userId) {
      this.userService
        .updatePassword(userId, this.user.oldPassword, this.user.newPassword)
        .subscribe({
          next: (response) => {
            const successMessage = 'Parola a fost actualizată cu succes.';
            console.log(successMessage);
            this.alertSuccessMessages.push(successMessage);

            // Redirect to personal profile page with success message
            const navigationExtras: NavigationExtras = {
              queryParams: { message: successMessage },
            };
            this.router.navigate(
              ['/personal-profile', userId, 'info'],
              navigationExtras
            );
          },
          error: (error) => {
            const errorMessage = 'Actualizarea parolei a eșuat: ' + error.error;
            console.error(errorMessage, error);
            this.alertErrorMessages.push(errorMessage);
          },
        });
    }
  }
}
