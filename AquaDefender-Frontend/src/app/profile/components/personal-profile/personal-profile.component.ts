import { Component, OnInit } from '@angular/core';
import { AuthenticationService } from '../../../authentication/services/authentication.service';
import {
  Observable,
  catchError,
  filter,
  forkJoin,
  map,
  of,
  switchMap,
} from 'rxjs';
import { ReportService } from '../../../report/services/report.service';
import { User } from '../../models/user.model';
import { UserService } from '../../services/user.service';
import { LocationService } from '../../../utils/services/location.service';
import { ActivatedRoute, NavigationEnd, NavigationExtras, Router } from '@angular/router';
import { ReportStatus } from '../../../report/enums/status';
import { SeverityLevel } from '../../../report/enums/severity';
import { Report } from '../../../report/models/report.model';
import { ViewportScroller } from '@angular/common';
import { IconService } from '../../../utils/services/icon.service';
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
  filtersApplied = false;

  alertErrorMessages: string[] = [];
  alertSuccessMessages: string[] = [];

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
    private route: ActivatedRoute,
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
    
    this.route.queryParams.subscribe((params) => {
      const message = params['message'];
      if (message && !this.alertSuccessMessages.includes(message)) {
        this.alertSuccessMessages.push(message);
        this.router.navigate([], {
          queryParams: { message: null },
          queryParamsHandling: 'merge',
        });
      }
    });
  }

  get isAuthenticated() {
    return this.authenticationService.getAuthStatus();
  }

  getUserId() {
    return this.authenticationService.getUserId() ?? 0;
  }

  onLogout() {
    this.authenticationService.logout();
  }

  toggleDropdown(): void {
    this.isDropdownOpen = !this.isDropdownOpen;
  }

  removeAlert(index: number): void {
    this.alertErrorMessages.splice(index, 1); // Îndepărtează mesajul de eroare la indexul specificat
  }

  removeSuccessAlert(index: number): void {
    this.alertSuccessMessages.splice(index, 1); // Îndepărtează mesajul de eroare la indexul specificat
  }

  openDeleteConfirmation(): void {
    this.showDeleteConfirmation = true;
  }

  closePopup(): void {
    this.showDeleteConfirmation = false;
  }

  confirmDelete(): void {
    this.isLoading = true;
    const userId = this.getUserId();
    this.userService.deleteUser(userId).subscribe({
      next: (response) => {
        this.authenticationService.logout();
        this.isLoading = false;
        const navigationExtras: NavigationExtras = {
          queryParams: {
            message: 'Profilul dumneavoastră fost șters cu succes.',
          },
        };
        this.router.navigate(['/home'], navigationExtras);
      },
      error: (error) => {
        this.isLoading = false;
        const errorMessage = 'A apărut o eroare la ștergerea user-ului.';
        console.error('There was an error deleting the user', error);
        this.alertErrorMessages.push(errorMessage);
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
        error: (error) => {
          console.error('Error getting total reports:', error);
          this.alertErrorMessages.push(
            'A apărut o eroare la preluarea numărului total de rapoarte.'
          );
        },
      });

      this.reportService.getNewReportsByUserId(userId).subscribe({
        next: (newReports) => {
          this.animateNumber('newReports', newReports, 2000);
        },
        error: (error) => {
          console.error('Error getting new reports:', error);
          this.alertErrorMessages.push(
            'A apărut o eroare la preluarea numărului de rapoarte noi.'
          );
        },
      });

      this.reportService.getInProgressReportsByUserId(userId).subscribe({
        next: (casesInProgress) => {
          this.animateNumber('casesInProgress', casesInProgress, 2000);
        },
        error: (error) => {
          console.error('Error getting in-progress reports:', error);
          this.alertErrorMessages.push(
            'A apărut o eroare la preluarea numărului de rapoarte în curs de desfășurare.'
          );
        },
      });

      this.reportService.getResolvedReportsByUserId(userId).subscribe({
        next: (resolvedReports) => {
          this.animateNumber('resolvedReports', resolvedReports, 2000);
        },
        error: (error) => {
          console.error('Error getting resolved reports:', error);
          this.alertErrorMessages.push(
            'A apărut o eroare la preluarea numărului de rapoarte rezolvate.'
          );
        },
      });
    }
  }

  async loadUserData() {
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
            next: async (result) => {
              // Update the names based on the results
              this.countyName = result.countyName.name;
              this.cityName = result.cityName.name;
              this.user.profilePictureUrl = await this.getProfilePicture(userId);
              console.log(this.user.profilePictureUrl);
              this.user.currentIndex = 0;
              this.isLoading = false;
              this.loadStatistics();
            },
            error: (error) => {
              console.error('Error fetching location data:', error);
              this.alertErrorMessages.push(
                'A apărut o eroare la preluarea datelor de locație.'
              );
              this.isLoading = false;
            },
          });
      } else {
        console.error('User ID not found in local storage');
        this.alertErrorMessages.push(
          'ID-ul utilizatorului nu a fost găsit în stocarea locală.'
        );
        this.isLoading = false;
      }
    } else {
      console.error('User is not authenticated');
      this.alertErrorMessages.push('Utilizatorul nu este autentificat.');
      this.isLoading = false;
    }
  }

  async getProfilePicture(userId: number) {
    try {
      const response = await this.userService.getUserProfileImage(userId)
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
          this.alertErrorMessages.push('Nu au fost găsite imagini în fișierul ZIP.');
        }
      } else {
        console.error('Unexpected response format:', response);
        this.alertErrorMessages.push('Formatul răspunsului este neașteptat.');
      }
    } catch (error) {
      console.error('Error fetching images:', error);
      this.alertErrorMessages.push('A apărut o eroare la preluarea imaginilor.');
    }
  
    console.log('Returning default paths.');
    this.alertErrorMessages.push('Se returnează căile implicite ale imaginilor.');
    return ['path/to/default/image.jpg'];
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
    const userId = this.authenticationService.getUserId(); // Replace this with the actual authenticated user's ID

    if (!userId) {
      const errorMessage = 'ID-ul utilizatorului este invalid.';
      console.error(errorMessage);
      this.alertErrorMessages.push(errorMessage);
      this.isLoading = false;
      return;
    }

    this.reportService
      .getFilteredReportsbyUserId(
        userId,
        this.selectedStatus,
        this.selectedSeverity
      )
      .pipe(
        catchError((error) => {
          const errorMessage =
            'Eroare la procesarea detaliilor raportului: ' + error.message;
          console.error(errorMessage);
          this.alertErrorMessages.push(errorMessage);
          this.isLoading = false;
          return of([]); // Return an empty observable to avoid crashing the application
        })
      )
      .subscribe((reports) => {
        this.isLoading = false;
        this.reports = reports; // Update the reports property with the fetched data

        if (reports.length === 0) {
          const errorMessage =
            'Nu au fost găsite rapoarte cu filtrele adăugate.';
          console.error(errorMessage);
          this.alertErrorMessages.push(errorMessage);
          return;
        }

        // Process report details
        this.processReportDetails(reports);
        this.alertSuccessMessages.push('Filtrele au fost aplicate cu succes.');
        this.isLoading = false;
      });
  }

  private async processReportDetails(reports: any[]): Promise<void> {
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
            console.log(report.imageUrls);
          } else {
            report.imageUrls = []; // Ensure it's an empty array if no images are found
          }
        }

        report.currentIndex = 0;
        report.statusText = this.getStatusText(report.status);
        report.severityText = this.getSeverityText(report.severity);
      } catch (error: any) {
        console.error('Error loading report details:', error);
        this.alertErrorMessages.push(
          'Eroare la încărcarea detaliilor raportului: ' + error.message
        );
        report.imageUrls = []; // Ensure it's initialized even if there's an error
        report.hasImages = false; // Ensure hasImages is initialized even if there's an error
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
          console.error('No images found in the ZIP file.');
          this.alertErrorMessages.push('Nu au fost găsite imagini în fișierul ZIP.');
        }
      } else {
        console.error('Unexpected response format:', response);
        this.alertErrorMessages.push('Formatul răspunsului este neașteptat.');
      }
    } catch (error) {
      console.error('Error fetching images:', error);
      this.alertErrorMessages.push('A apărut o eroare la preluarea imaginilor.');
    }
  
    console.log('Returning default paths.');
    this.alertErrorMessages.push('Se returnează căile implicite ale imaginilor.');
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
