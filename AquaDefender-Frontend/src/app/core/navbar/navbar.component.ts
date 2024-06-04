import { Component, HostListener } from '@angular/core';
import { IconService } from '../../utils/services/icon.service';
import { AuthenticationService } from '../../authentication/services/authentication.service';
import { ViewportScroller } from '@angular/common';
import { NavigationEnd, Router } from '@angular/router';
import { filter } from 'rxjs';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.scss',
})
export class NavbarComponent {
  isDropdownOpen: boolean = false;
  showResponsiveDropdown: boolean = false;

  constructor(
    public iconService: IconService,
    private authenticationService: AuthenticationService,
    private router: Router,
    private viewportScroller: ViewportScroller
  ) {
    this.router.events
      .pipe(filter((event) => event instanceof NavigationEnd))
      .subscribe(() => {
        this.viewportScroller.scrollToPosition([0, 0]);
        this.closeAllDropdowns();
      });
    this.checkWindowSize();
  }

  @HostListener('window:resize')
  onResize() {
    this.checkWindowSize();
  }

  private checkWindowSize() {
    if (window.innerWidth >= 1000) {
      this.showResponsiveDropdown = false;
    }
  }

  isAuthenticated(): boolean {
    return this.authenticationService.getAuthStatus();
  }

  onLogout() {
    this.authenticationService.logout();
  }

  toggleDropdown(): void {
    this.isDropdownOpen = !this.isDropdownOpen;
  }

  enableResposiveDropdown(): void {
    this.showResponsiveDropdown = !this.showResponsiveDropdown;
  }

  getCityId() {
    return this.authenticationService.getCityId() ?? 0;
  }
  
  getUserId() {
    return this.authenticationService.getUserId() ?? 0;
  }

  private closeAllDropdowns(): void {
    this.showResponsiveDropdown = false;
    this.isDropdownOpen = false; // Opțional, în funcție de necesități
  }
}
