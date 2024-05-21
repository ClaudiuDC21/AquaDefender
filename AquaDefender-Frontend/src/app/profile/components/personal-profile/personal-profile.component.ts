import { Component, OnInit } from '@angular/core';
import { AuthenticationService } from '../../../authentication/services/authentication.service';
import { Observable, catchError, filter, forkJoin, map, switchMap } from 'rxjs';
import { ReportService } from '../../../report/services/report.service';
import { User } from '../../models/user.model';
import { UserService } from '../../services/user.service';
import { LocationService } from '../../../other-services/location.service';
import { NavigationEnd, Router } from '@angular/router';
import { ReportStatus } from '../../../report/enums/status';
import { SeverityLevel } from '../../../report/enums/severity';
import { Report } from '../../../report/models/report.model';
import { ViewportScroller } from '@angular/common';
import { IconService } from '../../../other-services/icon.service';
import JSZip from 'jszip';

@Component({
  selector: 'app-personal-profile',
  templateUrl: './personal-profile.component.html',
  styleUrls: ['./personal-profile.component.scss'],
})
export class PersonalProfileComponent implements OnInit {
  isDropdownOpen: boolean = false;
  stats = {
    totalReports: 0,
    newReports: 0,
    casesInProgress: 0,
    resolvedReports: 0,
  };
  user: User = new User();
  reports: Report[] = [];
  countyName: string = '';
  cityName: string = '';

  severityOptions = Object.values(SeverityLevel);
  statusOptions = Object.values(ReportStatus);

  selectedSeverity: SeverityLevel = SeverityLevel.All;
  selectedStatus: ReportStatus = ReportStatus.All;

  showDeleteConfirmation: boolean = false;
  isLoading = false;

  severityOptionsUI = [
    { display: 'Toate', value: SeverityLevel.All },
    { display: 'Minor', value: SeverityLevel.Minor },
    { display: 'Moderat', value: SeverityLevel.Moderate },
    { display: 'Serios', value: SeverityLevel.Serious },
    { display: 'Sever', value: SeverityLevel.Severe },
    { display: 'Critic', value: SeverityLevel.Critical },
  ];

  statusOptionsUI = [
    { display: 'Toate', value: ReportStatus.All },
    { display: 'Nou', value: ReportStatus.New },
    { display: 'În Progres', value: ReportStatus.InProgress },
    { display: 'Rezolvat', value: ReportStatus.Resolved },
  ];

  constructor(
    private authenticationService: AuthenticationService,
    private reportService: ReportService,
    private userService: UserService,
    private locationService: LocationService,
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

  ngOnInit(): void {
    this.loadUserData();
  }

  get isAuthenticated() {
    return this.authenticationService.getAuthStatus();
  }

  getUserId(){
    return this.authenticationService.getUserId() ?? 0;
  }

  onLogout() {
    this.authenticationService.logout();
  }

  toggleDropdown(): void {
    this.isDropdownOpen = !this.isDropdownOpen;
  }

  openDeleteConfirmation(): void {
    this.showDeleteConfirmation = true;
  }

  closePopup(): void {
    this.showDeleteConfirmation = false;
  }

  confirmDelete(): void {
    this.isLoading = true;
    const userId = 4;
    this.userService.deleteUser(userId).subscribe({
      next: (response) => {
        // Logica pentru când ștergerea a avut succes
        this.authenticationService.logout();
        this.isLoading = false;
        this.router.navigate(['/']); // Redirect to home page
        console.log('User-ul cu id-ul' + userId + 'a fpst sters cu succes.');
      },
      error: (error) => {
        // Logica pentru când ștergerea a eșuat
        console.error('There was an error deleting the user', error);
      },
    });
  }

  loadStatistics(): void {
    this.isLoading = false;
    const userId = this.authenticationService.getUserId(); // Înlocuiește cu ID-ul utilizatorului actual
    if (userId) {
      this.reportService.getTotalReportsByUserId(userId).subscribe({
        next: (totalReports) => {
          this.animateNumber('totalReports', totalReports, 2000);
        },
        // handle error
      });

      this.reportService.getNewReportsByUserId(userId).subscribe({
        next: (newReports) => {
          this.animateNumber('newReports', newReports, 2000);
        },
        // handle error
      });

      this.reportService.getInProgressReportsByUserId(userId).subscribe({
        next: (casesInProgress) => {
          this.animateNumber('casesInProgress', casesInProgress, 2000);
        },
        // handle error
      });

      this.reportService.getResolvedReportsByUserId(userId).subscribe({
        next: (resolvedReports) => {
          this.animateNumber('resolvedReports', resolvedReports, 2000);
        },
        // handle error
      });
    }
  }

  loadUserData() {
    this.isLoading = true;
    // Check if the user is authenticated and retrieve userId from local storage
    if (this.isAuthenticated) {
      const userId = this.authenticationService.getUserId(); // Get userId from local storage
      if (userId) {
        this.userService
          .getUserById(userId)
          .pipe(
            switchMap((userData: User) => {
              this.user = userData;
              return forkJoin({
                countyName: this.locationService.getCountyById(
                  userData.countyId
                ),
                cityName: this.locationService.getCityById(userData.cityId),
              });
            })
          )
          .subscribe({
            next: (result) => {
              // Update the names based on the results
              this.countyName = result.countyName.name;
              this.cityName = result.cityName.name;
              this.isLoading = false;
              this.loadStatistics();
            },
            error: (error) => {
              console.error('Error fetching location data:', error);
              this.isLoading = false;
            },
          });
      } else {
        console.error('User ID not found in local storage');
        this.isLoading = false;
      }
    }
  }

  calculateAge(birthDate: Date): number {
    if (!birthDate) return 0;

    const today = new Date();
    const birthDateObj = new Date(birthDate);
    let age = today.getFullYear() - birthDateObj.getFullYear();
    const m = today.getMonth() - birthDateObj.getMonth();

    // Ajustează vârsta dacă ziua de naștere nu a trecut încă în anul curent
    if (m < 0 || (m === 0 && today.getDate() < birthDateObj.getDate())) {
      age--;
    }

    return age;
  }

  applyFilters() {
    this.isLoading = true;
    const userId = this.authenticationService.getUserId(); // Replace this with the actual authenticated user's ID
    if (userId) {
      this.reportService
        .getFilteredReportsbyUserId(
          userId,
          this.selectedStatus,
          this.selectedSeverity
        )
        .pipe(
          catchError((error) => {
            console.error('Error processing report details:', error);
            this.isLoading = false;
            return []; // Return an empty array to avoid crashing the application
          })
        )
        .subscribe((reports) => {
          if (reports.length === 0) {
            console.error('No reports fetched');
            this.isLoading = false;
            return;
          }

          // Process report details
          this.processReportDetails(reports);
        });
    }
  }

  private async processReportDetails(reports: any[]): Promise<void> {
    for (const report of reports) {
      console.log(report);
      try {
        const details = await forkJoin({
          county: this.locationService.getCountyById(report.countyId),
          city: this.locationService.getCityById(report.cityId),
          user: this.userService.getUserById(report.userId), // Get the username
          hasImages: this.reportService.checkIfReportHasImages(report.id), // Check if the report has images
        }).toPromise();

        if (details) {
          report.county = details.county?.name;
          report.city = details.city?.name;
          report.username = details.user?.userName; // Store the username
          report.hasImages = details.hasImages; // Store the image check result

          if (report.hasImages) {
            report.imageUrls = await this.getReportImages(report.id);
          } else {
            report.imageUrls = []; // Use an empty array if no images
          }
        }

        report.currentIndex = 0;
        report.statusText = this.getStatusText(report.status);
        report.severityText = this.getSeverityText(report.severity); // Map status numeric to text
      } catch (error) {
        console.error('Error loading report details:', error);
      }
    }

    this.reports = reports;
    this.isLoading = false;
  }

  getStatusText(status: number): string {
    switch (status) {
      case 0:
        return 'Nou';
      case 1:
        return 'În Progres';
      case 2:
        return 'Rezolvat';
      default:
        return 'Necunoscut';
    }
  }

  getSeverityText(severity: number): string {
    switch (severity) {
      case 0:
        return 'minor';
      case 1:
        return 'moderat';
      case 2:
        return 'serios';
      case 3:
        return 'sever';
      case 4:
        return 'critic';
      default:
        return 'necunoscut';
    }
  }

  async getReportImages(reportId: number) {
    try {
      const response = await this.reportService
        .getImagesByReportId(reportId)
        .toPromise();

      if (response instanceof Blob) {
        const zip = new JSZip();
        const zipData = await zip.loadAsync(response);

        const imageFiles = Object.values(zipData.files);
        const imageUrls: string[] = [];

        for (const file of imageFiles) {
          const imageUrl = URL.createObjectURL(await file.async('blob'));
          imageUrls.push(imageUrl);
        }

        if (imageUrls.length > 0) {
          return imageUrls;
        } else {
          console.error('No images found in the ZIP file.');
        }
      } else {
        console.error('Unexpected response format:', response);
      }
    } catch (error) {
      console.error('Error fetching images:', error);
    }

    console.log('Returning default paths.');
    return ['path/to/default/image.jpg'];
  }

  nextImage(report: any) {
    if (report.imageUrls && report.imageUrls.length > 0) {
      report.currentIndex = (report.currentIndex + 1) % report.imageUrls.length;
    }
  }

  prevImage(report: any) {
    if (report.imageUrls && report.imageUrls.length > 0) {
      report.currentIndex =
        (report.currentIndex - 1 + report.imageUrls.length) %
        report.imageUrls.length;
    }
  }

  getStatusClass(statusText: string): string {
    switch (statusText) {
      case 'Nou':
        return 'status-new'; // Clasa pentru statusul "Nou"
      case 'În Progres':
        return 'status-in-progress'; // Clasa pentru statusul "În Progres"
      case 'Rezolvat':
        return 'status-resolved'; // Clasa pentru statusul "Rezolvat"
      default:
        return 'status-unknown'; // Clasa implicită pentru orice altă valoare
    }
  }

  private animateValue(
    current: number,
    target: number,
    duration: number
  ): Observable<number> {
    const totalFrames = (duration / 1000) * 60; // De exemplu, pentru 60 de FPS
    const increment = (target - current) / totalFrames;
    let frame = 0;

    return new Observable<number>((observer) => {
      const intervalId = setInterval(() => {
        frame++;
        const value = current + increment * frame;

        if (frame >= totalFrames) {
          observer.next(target); // Asigură-te că ultima valoare este exact ținta
          observer.complete();
          clearInterval(intervalId);
        } else {
          observer.next(value);
        }
      }, 1000 / 60); // Aproximativ 60 de FPS

      return () => clearInterval(intervalId);
    });
  }

  animateNumber(
    property: keyof typeof this.stats,
    target: number,
    duration: number
  ): void {
    this.animateValue(0, target, duration).subscribe((value: number) => {
      this.stats[property] = Math.floor(value);
      if (this.stats[property] === target) {
        this.stats[property] = target; // pentru a asigura că se oprește la target
      }
    });
  }
}
