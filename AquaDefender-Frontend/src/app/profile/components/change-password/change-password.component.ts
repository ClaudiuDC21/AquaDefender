import { Component, OnInit } from '@angular/core';
import { AuthenticationService } from '../../../authentication/services/authentication.service';
import { Observable, filter, forkJoin, map, switchMap } from 'rxjs';
import { ReportService } from '../../../report/services/report.service';
import { User } from '../../models/user.model';
import { UserService } from '../../services/user.service';
import { LocationService } from '../../../utils/location.service';
import { NavigationEnd, Router } from '@angular/router';
import { ReportStatus } from '../../../report/enums/status';
import { SeverityLevel } from '../../../report/enums/severity';
import { Report } from '../../../report/models/report.model';
import { ViewportScroller } from '@angular/common';

@Component({
  selector: 'app-change-password',
  templateUrl: './change-password.component.html',
  styleUrl: './change-password.component.scss',
})
export class ChangePasswordComponent implements OnInit {
  isDropdownOpen: boolean = false;
  isLoading: boolean = false;
  user = {
    oldPassword: '',
    newPassword: '',
    confirmNewPassword: '',
  };

  constructor(
    private authenticationService: AuthenticationService,
    private userService: UserService,
    private router: Router,
    private viewportScroller: ViewportScroller
  ) {
    this.router.events.pipe(
      filter(event => event instanceof NavigationEnd)
    ).subscribe(() => {
      this.viewportScroller.scrollToPosition([0,0]);
    });
  }

  ngOnInit(): void {}

  get isAuthenticated() {
    return this.authenticationService.getAuthStatus();
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
      alert('Please fill in all fields.');
      return;
    }

    if (this.user.newPassword !== this.user.confirmNewPassword) {
      alert('New password and confirmation password do not match.');
      return;
    }

    // Assuming 'userId' is stored and retrieved securely, e.g., from a user service or auth service
    const userId = 15 // Or however you obtain the user's ID
    this.userService
      .updatePassword(userId, this.user.oldPassword, this.user.newPassword)
      .subscribe({
        next: (response) => {
          alert('Password successfully updated.');
          // Additional logic after successful password update, such as redirecting
          // this.router.navigate(['/profile']);
        },
        error: (error) => {
          console.error('Error updating password:', error);
          alert('Failed to update password.'); // You might want to show a more user-friendly message
        },
      });
  }
}
