import { Component, ElementRef, HostListener, OnInit } from '@angular/core';
import { AuthenticationService } from '../authentication/services/authentication.service';
import { Report } from '../report/models/report.model';
import { ReportService } from '../report/services/report.service';
import { LocationService } from '../utils/location.service';
import { Observable, catchError, filter, forkJoin, map, switchMap } from 'rxjs';
import JSZip from 'jszip';
import { IconService } from '../utils/icon.service';
import { ActivatedRoute, NavigationEnd, Router } from '@angular/router';
import { ViewportScroller } from '@angular/common';
import { User } from '../profile/models/user.model';
import { UserService } from '../profile/services/user.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss',
})
export class HomeComponent implements OnInit {
  isLoading = false;
  isDropdownOpen: boolean = false;
  showResponsiveDropdown: boolean = false;
  users: any;
  cityName: string = '';
  reports: Report[] = [];
  currentIndex: number = 0;
  reportImages: string[] = [
    'https://www.americanrivers.org/wp-content/uploads/2022/08/Untitled-design-43-2-1024x576.png',
    'https://images.nationalgeographic.org/image/upload/t_edhub_resource_key_image/v1638882947/EducationHub/photos/tourists-at-victoria-falls.jpg',
  ];

  alertErrorMessages: string[] = [];
  alertSuccessMessages: string[] = [];
  alertInfoMessages: string[] = [];
  alertWarningMessages: string[] = [];

  constructor(
    public iconService: IconService,
    private authenticationService: AuthenticationService,
    private reportService: ReportService,
    private locationService: LocationService,
    private router: Router,
    private userService: UserService,
    private viewportScroller: ViewportScroller,
    private route: ActivatedRoute
  ) {
    this.router.events
      .pipe(filter((event) => event instanceof NavigationEnd))
      .subscribe(() => {
        this.viewportScroller.scrollToPosition([0, 0]);
        this.closeAllDropdowns();
      });
  }

  ngOnInit(): void {
    this.loadReports();
    this.route.queryParams.subscribe((params) => {
      const message = params['message'];
      const reportCount = params['reportCount']; // Obține numărul de rapoarte noi

      if (message && !this.alertSuccessMessages.includes(message)) {
        this.alertSuccessMessages.push(message);
        this.router.navigate([], {
          queryParams: { message: null },
          queryParamsHandling: 'merge',
        });
      }

      if (reportCount) {
        this.alertInfoMessages.push(
          `Sunt ${reportCount} rapoarte noi în localitatea dumneavoastră.`
        );
        this.router.navigate([], {
          queryParams: { reportCount: null },
          queryParamsHandling: 'merge',
        });
      }
    });
  }

  isAuthenticated(): boolean {
    return this.authenticationService.getAuthStatus();
  }

  getCityId() {
    return this.authenticationService.getCityId() ?? 0;
  }

  getUserId() {
    return this.authenticationService.getUserId() ?? 0;
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

  onLogout() {
    this.authenticationService.logout();
  }

  toggleDropdown(): void {
    this.isDropdownOpen = !this.isDropdownOpen;
  }

  enableResposiveDropdown(): void {
    this.showResponsiveDropdown = !this.showResponsiveDropdown;
  }

  private closeAllDropdowns(): void {
    this.showResponsiveDropdown = false;
    this.isDropdownOpen = false; // Opțional, în funcție de necesități
  }

  private async loadReports(): Promise<void> {
    this.isLoading = true;

    forkJoin({
      newReports: this.reportService.getRandomNewReports(),
      inProgressReports: this.reportService.getRandomInProgressReports(),
      completedReports: this.reportService.getRandomCompletedReports(),
    })
      .pipe(
        catchError((error) => {
          console.error('Error loading reports', error);
          return []; // sau returnează un observabil gol pentru a evita crash-ul aplicației
        })
      )
      .subscribe((results) => {
        const allReports = [
          ...results.newReports,
          ...results.inProgressReports,
          ...results.completedReports,
        ];

        if (allReports.length === 0) {
          const errorMessage = 'Nu s-au preluat rapoarte';
          console.error(errorMessage);
          this.alertErrorMessages.push(errorMessage);
          this.isLoading = false;
          return;
        }

        this.processReportDetails(allReports);
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
        report.statusText = this.getStatusText(report.status);
        report.severityText = this.getSeverityText(report.severity); // Map status numeric to text
      } catch (error) {
        const errorMessage =
          'Eroare la încărcarea detaliilor raportului: ' + error;
        console.error(errorMessage);
        this.alertErrorMessages.push(errorMessage);
      }
    }

    this.reports = reports;
    this.isLoading = false;
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
      const errorMessage = 'Eroare la preluarea imaginilor: ' + error;
      console.error(errorMessage);
      this.alertErrorMessages.push(errorMessage);
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

  getStatusClass(statusText: string): string {
    switch (statusText) {
      case 'Nou':
        return 'status-new';
      case 'În Progres':
        return 'status-in-progress'; 
      case 'Rezolvat':
        return 'status-resolved'; 
      default:
        return 'status-unknown'; 
    }
  }
}
