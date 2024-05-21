import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { AuthenticationService } from '../../../authentication/services/authentication.service';
import { LocationService } from '../../../other-services/location.service';
import { Report } from '../../models/report.model';
import { ReportService } from '../../services/report.service';
import { SeverityLevel } from '../../enums/severity';
import { User } from '../../../profile/models/user.model';
import { ReportStatus } from '../../enums/status';
import { NavigationEnd, Router } from '@angular/router';
import { UserService } from '../../../profile/services/user.service';
import { Observable, catchError, filter, forkJoin, map, switchMap } from 'rxjs';
import { ReportStatistics } from '../../models/report-statistics.model';
import { ViewportScroller } from '@angular/common';
import JSZip from 'jszip';
import { IconService } from '../../../other-services/icon.service';

@Component({
  selector: 'app-report-management',
  templateUrl: './report-management.component.html',
  styleUrl: './report-management.component.scss',
})
export class ReportManagementComponent implements OnInit {
  isDropdownOpen: boolean = false;
  stats = {
    totalReports: 0,
    newReports: 0,
    casesInProgress: 0,
    resolvedReports: 0,
    veryLowSeverity: 0,
    lowSeverity: 0,
    mediumSeverity: 0,
    highSeverity: 0,
    veryHighSeverity: 0,
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
  filtersApplied = false;

  selectedStartDate: string = '';
  selectedEndDate: string = '';
  userNameSearch: string = '';
  today: Date = new Date();

  cityId: string | null = this.authenticationService.getCityId();

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

  statusOptionUI = [
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

  isAuthenticated() {
    return this.authenticationService.getAuthStatus();
  }

  isNotUser(){
    return this.authenticationService.isNotUser();
  }

  onLogout() {
    this.authenticationService.logout();
  }

  toggleDropdown(): void {
    this.isDropdownOpen = !this.isDropdownOpen;
  }

  loadStatistics(cityId: number): void {
    this.isLoading = true; // Set loading state to true at the start of the operation
    this.reportService.loadStatistics(cityId).subscribe({
      next: (statistics: ReportStatistics) => {
        this.stats = statistics; // Update the stats object with the received data
        this.animateStatistics();
        this.isLoading = false; // Reset loading state after all data has been received and processed
      },
      error: (error) => {
        console.error('Error loading statistics:', error);
        this.isLoading = false;
      },
    });
  }

  private animateStatistics() {
    const keys = Object.keys(this.stats) as (keyof typeof this.stats)[]; // Type assertion here
    keys.forEach((key) => {
      this.animateNumber(key, this.stats[key], 2000);
    });
  }

  loadUserData() {
    this.isLoading = true;
    const userId = this.authenticationService.getUserId(); // Obtain dynamic userId

    if (userId === null) {
      console.error('Invalid user ID');
      this.isLoading = false;
      return;
    }
    this.userService
      .getUserById(userId)
      .pipe(
        switchMap((userData: User) => {
          this.user = userData;
          // În paralel, obține numele județului și orașului folosind ID-urile
          return forkJoin({
            cityName: this.locationService.getCityById(userData.cityId),
          });
        })
      )
      .subscribe({
        next: (result) => {
          const cityIdNumber =
            this.cityId !== null ? parseInt(this.cityId, 10) : null;

          // Check if cityIdNumber is valid before making the API call
          if (cityIdNumber === null || isNaN(cityIdNumber)) {
            console.error('Invalid city ID');
            this.isLoading = false;
            return;
          }
          this.cityName = result.cityName.name;
          this.isLoading = false;
          this.loadStatistics(cityIdNumber);
        },
        error: (error) => {
          console.error('Error fetching location data:', error);
        },
      });
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
    this.filtersApplied = true;
    const cityIdNumber =
      this.cityId !== null ? parseInt(this.cityId, 10) : null;

    // Check if cityIdNumber is valid before making the API call
    if (cityIdNumber === null || isNaN(cityIdNumber)) {
      console.error('Invalid city ID');
      this.isLoading = false;
      return;
    }

    this.reportService
      .getFilteredReportsByCityId(
        cityIdNumber,
        this.selectedStatus,
        this.selectedSeverity,
        this.selectedStartDate,
        this.selectedEndDate,
        this.userNameSearch
      )
      .pipe(
        catchError((error) => {
          console.error('Error processing report details:', error);
          this.isLoading = false;
          return []; // sau returnează un observabil gol pentru a evita crash-ul aplicației
        })
      )
      .subscribe((reports) => {
        if (reports.length === 0) {
          console.error('No reports fetched');
          this.isLoading = false;
          return;
        }

        // Procesăm detaliile raportului
        this.processReportDetails(reports);
      });
  }

  private async processReportDetails(reports: any[]): Promise<void> {
    for (const report of reports) {
      console.log(report);
      try {
        const details = await forkJoin({
          county: this.locationService.getCountyById(report.countyId),
          city: this.locationService.getCityById(report.cityId),
          user: this.userService.getUserById(report.userId), // Adăugăm obținerea numelui utilizatorului aici
          hasImages: this.reportService.checkIfReportHasImages(report.id), // Adăugăm verificarea imaginilor aici
        }).toPromise();

        if (details) {
          report.county = details.county?.name;
          report.city = details.city?.name;
          report.username = details.user?.userName; // Stocăm numele utilizatorului
          report.hasImages = details.hasImages; // Stocăm rezultatul verificării imaginilor

          if (report.hasImages) {
            report.imageUrls = await this.getReportImages(report.id);
          } else {
            report.imageUrls = []; // Dacă nu sunt imagini, folosim un array gol
          }
        }

        report.currentIndex = 0;
        report.statusText = this.getStatusText(report.status); // Mapează statusul numeric la text
      } catch (error) {
        console.error('Error loading report details:', error);
      }
    }

    this.reports = reports;
    this.isLoading = false;
  }

  // Metodă pentru a mapează statusul numeric la text
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

  

  updateStatus(report: any) {
    if (report.status !== null) {
      console.log(report.status);
      this.reportService.updateReportStatus(report.id, report.status).subscribe({
        next: response => {
          console.log('Status updated successfully', response);
          // Opțional: Actualizează `report.statusText` sau alte acțiuni necesare după update
        },
        error: error => console.error('Failed to update status', error)
      });
    }
  }

  getStatusClass(statusText: string): string {
    switch (statusText) {
      case 'Nou':
        return 'status-new';  // Clasa pentru statusul "Nou"
      case 'În Progres':
        return 'status-in-progress';  // Clasa pentru statusul "În Progres"
      case 'Rezolvat':
        return 'status-resolved';  // Clasa pentru statusul "Rezolvat"
      default:
        return 'status-unknown';  // Clasa implicită pentru orice altă valoare
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
