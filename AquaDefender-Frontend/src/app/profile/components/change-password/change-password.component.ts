import { Component, OnInit } from '@angular/core';
import { AuthenticationService } from '../../../authentication/services/authentication.service';
import { filter } from 'rxjs';
import { UserService } from '../../services/user.service';
import { NavigationEnd, NavigationExtras, Router } from '@angular/router';
import { ViewportScroller } from '@angular/common';
import { IconService } from '../../../utils/services/icon.service';

@Component({
  selector: 'app-change-password',
  templateUrl: './change-password.component.html',
  styleUrl: './change-password.component.scss',
})
export class ChangePasswordComponent {
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


  getisAuthenticated() {
    return this.authenticationService.getAuthStatus();
  }

  getUserId() {
    return this.authenticationService.getUserId();
  }

  removeAlert(index: number): void {
    this.alertErrorMessages.splice(index, 1);
  }

  removeSuccessAlert(index: number): void {
    this.alertSuccessMessages.splice(index, 1);
  }

  newPasswordsMatch(): boolean {
    return this.user.newPassword === this.user.confirmNewPassword;
  }

  changePassword(): void {
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

    const userId = this.getUserId();
    this.isLoading = true;
    if (userId) {
      this.userService
        .updatePassword(userId, this.user.oldPassword, this.user.newPassword)
        .subscribe({
          next: (response) => {
            const successMessage = 'Parola a fost actualizată cu succes.';
            console.log(successMessage);
            this.alertSuccessMessages.push(successMessage);
            this.isLoading = false;
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
            this.isLoading = false;
          },
        });
    }
  }
}
