import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { AuthenticationService } from '../../../authentication/services/authentication.service';
import { LocationService } from '../../../utils/services/location.service';
import { Report } from '../../models/report.model';
import { ReportService } from '../../services/report.service';
import { SeverityLevel } from '../../enums/severity';
import { User } from '../../../profile/models/user.model';
import { ReportStatus } from '../../enums/status';
import { NavigationEnd, Router } from '@angular/router';
import { UserService } from '../../../profile/services/user.service';
import {
  Observable,
  catchError,
  filter,
  forkJoin,
  map,
  of,
  switchMap,
} from 'rxjs';
import { ReportStatistics } from '../../models/report-statistics.model';
import { ViewportScroller } from '@angular/common';
import JSZip from 'jszip';
import { IconService } from '../../../utils/services/icon.service';

@Component({
  selector: 'app-report-management',
  templateUrl: './report-management.component.html',
  styleUrl: './report-management.component.scss',
})
export class ReportManagementComponent implements OnInit {
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
  currentReportId: number | null = null;

  severityOptions = Object.values(SeverityLevel);
  statusOptions = Object.values(ReportStatus);

  selectedSeverity: SeverityLevel = SeverityLevel.All;
  selectedStatus: ReportStatus = ReportStatus.All;

  showDeleteConfirmation: boolean = false;
  statusChangeMessage: string = '';
  isLoading = false;
  filtersApplied = false;

  selectedStartDate: string = '';
  selectedEndDate: string = '';
  userNameSearch: string = '';
  today: Date = new Date();

  alertErrorMessages: string[] = [];
  alertSuccessMessages: string[] = [];

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

  isNotUser() {
    return this.authenticationService.isNotUser();
  }

  removeAlert(index: number): void {
    this.alertErrorMessages.splice(index, 1);
  }

  removeSuccessAlert(index: number): void {
    this.alertSuccessMessages.splice(index, 1);
  }

  loadStatistics(cityId: number): void {
    this.isLoading = true;
    this.reportService.loadStatistics(cityId).subscribe({
      next: (statistics: ReportStatistics) => {
        this.stats = statistics;
        this.animateStatistics();
        this.isLoading = false;
      },
      error: (error) => {
        console.error('Error loading statistics:', error);
        this.isLoading = false;
      },
    });
  }

  private animateStatistics() {
    const keys = Object.keys(this.stats) as (keyof typeof this.stats)[];
    keys.forEach((key) => {
      this.animateNumber(key, this.stats[key], 2000);
    });
  }

  loadUserData() {
    this.isLoading = true;
    const userId = this.authenticationService.getUserId();

    if (userId === null) {
      console.error('Invalid user ID');
      if (this.isAuthenticated())
        this.alertErrorMessages.push('ID-ul utilizatorului este invalid.');
      this.isLoading = false;
      return;
    }

    this.userService
      .getUserById(userId)
      .pipe(
        switchMap((userData: User) => {
          this.user = userData;
          return forkJoin({
            cityName: this.locationService.getCityById(userData.cityId),
          });
        }),
        catchError((error) => {
          console.error('Error fetching user data:', error);
          this.alertErrorMessages.push(
            'Eroare la obținerea datelor utilizatorului: ' + error.message
          );
          this.isLoading = false;
          return of(null);
        })
      )
      .subscribe({
        next: (result) => {
          if (!result) {
            return;
          }
          const cityIdNumber =
            this.cityId !== null ? parseInt(this.cityId, 10) : null;

          if (cityIdNumber === null || isNaN(cityIdNumber)) {
            console.error('Invalid city ID');
            this.alertErrorMessages.push('ID-ul orașului este invalid.');
            this.isLoading = false;
            return;
          }
          this.cityName = result.cityName.name;
          this.isLoading = false;
          this.loadStatistics(cityIdNumber);
        },
        error: (error) => {
          console.error('Error fetching location data:', error);
          this.alertErrorMessages.push(
            'Eroare la obținerea datelor locației: ' + error.message
          );
          this.isLoading = false;
        },
      });
  }

  calculateAge(birthDate: Date): number {
    if (!birthDate) return 0;

    const today = new Date();
    const birthDateObj = new Date(birthDate);
    let age = today.getFullYear() - birthDateObj.getFullYear();
    const m = today.getMonth() - birthDateObj.getMonth();

    if (m < 0 || (m === 0 && today.getDate() < birthDateObj.getDate())) {
      age--;
    }

    return age;
  }

  applyFilters() {
    this.isLoading = true;
    this.filtersApplied = true;

    if (
      this.selectedEndDate &&
      this.selectedStartDate &&
      this.selectedEndDate < this.selectedStartDate
    ) {
      const errorMessage =
        'Data de sfârșit nu poate fi mai mică decât data de început.';
      console.error(errorMessage);
      this.alertErrorMessages.push(errorMessage);
      this.isLoading = false;
      return;
    }

    const cityIdNumber =
      this.cityId !== null ? parseInt(this.cityId, 10) : null;

    if (cityIdNumber === null || isNaN(cityIdNumber)) {
      const errorMessage = 'ID-ul orașului este invalid.';
      console.error(errorMessage);
      this.alertErrorMessages.push(errorMessage);
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
          const errorMessage =
            'Eroare la procesarea detaliilor raportului: ' + error.message;
          console.error(errorMessage);
          this.alertErrorMessages.push(errorMessage);
          this.isLoading = false;
          return of([]);
        })
      )
      .subscribe((reports) => {
        if (this.userNameSearch) {
          reports = reports.filter((report) => !report.isAnonymous);
        }

        this.reports = reports;

        if (reports.length === 0) {
          const errorMessage =
            'Nu au fost găsite rapoarte cu filtrele adăugate.';
          console.error(errorMessage);
          this.alertErrorMessages.push(errorMessage);
          this.isLoading = false;
          return;
        }

        this.processReportDetails(reports);
        this.alertSuccessMessages.push('Filtrele au fost aplicate cu succes.');
        this.isLoading = false;
      });
  }

  private async processReportDetails(reports: any[]): Promise<void> {
    this.isLoading = true;
    for (const report of reports) {
      console.log(report);
      try {
        const details = await forkJoin({
          county: this.locationService.getCountyById(report.countyId),
          city: this.locationService.getCityById(report.cityId),
          user: this.userService.getUserById(report.userId),
          hasImages: this.reportService.checkIfReportHasImages(report.id),
        }).toPromise();

        if (details) {
          report.county = details.county?.name;
          report.city = details.city?.name;
          report.username = details.user?.userName;
          report.hasImages = details.hasImages;

          if (report.hasImages) {
            report.imageUrls = await this.getReportImages(report.id);
          } else {
            report.imageUrls = [];
          }
        }

        report.currentIndex = 0;
        report.statusText = this.getStatusText(report.status);
        report.severityText = this.getSeverityText(report.severity);
        this.isLoading = false;
      } catch (error) {
        console.error('Error loading report details:', error);
        this.isLoading = false;
      }
    }

    this.reports = reports;
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
          const noImagesError = 'Nu au fost găsite imagini în fișierul ZIP.';
          console.error(noImagesError);
          this.alertErrorMessages.push(noImagesError);
        }
      } else {
        const unexpectedResponseError =
          'Format de răspuns neașteptat: ' + response;
        console.error(unexpectedResponseError);
        this.alertErrorMessages.push(unexpectedResponseError);
      }
    } catch (error: any) {
      const fetchImagesError =
        'Eroare la obținerea imaginilor: ' + error.message;
      console.error(fetchImagesError);
      this.alertErrorMessages.push(fetchImagesError);
    }

    const defaultPathMessage = 'Întoarcere la căile implicite.';
    console.log(defaultPathMessage);
    this.alertErrorMessages.push(defaultPathMessage);
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
      const oldStatusText = this.getStatusText(report.status);

      console.log(report.status);
      this.currentReportId = report.id;
      this.reportService
        .updateReportStatus(report.id, report.status)
        .subscribe({
          next: (response) => {
            console.log('Status updated successfully', response);
            const newStatusText = this.getStatusText(report.status);
            this.statusChangeMessage = `Statusul acestui raport a fost schimbat în "${newStatusText}".\nPentru a putea vedea modificările va trebui să aplicați din nou filtrele dorite!`;
          },
          error: (error) => {
            console.error('Failed to update status', error);
            const errorMessage =
              'Eroare la actualizarea statusului raportului: ' + error.message;
            this.alertErrorMessages.push(errorMessage);
            this.currentReportId = null;
          },
        });
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

  private animateValue(
    current: number,
    target: number,
    duration: number
  ): Observable<number> {
    const totalFrames = (duration / 1000) * 60;
    const increment = (target - current) / totalFrames;
    let frame = 0;

    return new Observable<number>((observer) => {
      const intervalId = setInterval(() => {
        frame++;
        const value = current + increment * frame;

        if (frame >= totalFrames) {
          observer.next(target);
          observer.complete();
          clearInterval(intervalId);
        } else {
          observer.next(value);
        }
      }, 1000 / 60);

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
        this.stats[property] = target;
      }
    });
  }
}
