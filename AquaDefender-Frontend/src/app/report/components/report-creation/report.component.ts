import { HttpClient } from '@angular/common/http';
import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { AuthenticationService } from '../../../authentication/services/authentication.service';
import * as L from 'leaflet';
import { LocationService } from '../../../utils/services/location.service';
import { Report } from '../../models/report.model';
import { ReportService } from '../../services/report.service';
import { ViewportScroller } from '@angular/common';
import {
  ActivatedRoute,
  NavigationEnd,
  NavigationExtras,
  Router,
} from '@angular/router';
import { filter, forkJoin } from 'rxjs';
import { AbstractControl, FormArray, FormGroup, NgForm } from '@angular/forms';
import { IconService } from '../../../utils/services/icon.service';
import { EmailService } from '../../../utils/services/email.service';
import { UserService } from '../../../profile/services/user.service';

@Component({
  selector: 'app-report',
  templateUrl: './report.component.html',
  styleUrl: './report.component.scss',
})
export class ReportComponent implements OnInit {
  isDropdownOpen: boolean = false;
  showInstructions: boolean = false;
  formSubmissionAttempt: boolean = false;
  report: Report = {
    id: 0, // sau poți omite acest rând dacă dorești, deoarece este opțional
    title: '',
    description: '',
    reportDate: undefined, // sau poți folosi `new Date()` dacă vrei să inițializezi cu data curentă
    userId: 0,
    username: '',
    county: '',
    city: '',
    locationDetails: '',
    latitude: 0,
    longitude: 0,
    isAnonymous: false,
    status: 0,
    statusText: '',
    severity: -1,
    severityText: '',
    imageFiles: [],
    imageUrls: [],
    currentIndex: 0,
    hasImages: false,
  };
  counties: any[] = []; // To store counties
  cities: any[] = [];
  imagePreviews: string[] = [];

  isLoading = false;
  alertErrorMessages: string[] = [];
  alertSuccessMessages: string[] = [];
  alertInfoMessages: string[] = [];
  alertWarningMessages: string[] = [];

  private map: any;
  private currentMarker: any;

  ngOnInit(): void {
    this.loadCounties();

    setTimeout(() => {
      this.map = L.map('map').setView([46.0, 24.4856], 7);

      L.tileLayer(
        'https://api.maptiler.com/maps/streets-v2/{z}/{x}/{y}.png?key=ZbxnTyrFHviL2DiDto0g',
        {
          tileSize: 512,
          zoomOffset: -1,
        }
      ).addTo(this.map);

      const popup = L.popup();

      this.map.on('click', (e: L.LeafletMouseEvent) => {
        if (this.currentMarker) {
          this.map.removeLayer(this.currentMarker);
        }

        this.report.latitude = e.latlng.lat;
        this.report.longitude = e.latlng.lng;

        this.currentMarker = L.marker(e.latlng).addTo(this.map);

        popup
          .setLatLng(e.latlng)
          .setContent(
            'Ați apăsat pe hartă la coordonatele: Latitudine ' +
              e.latlng.lat.toFixed(9) +
              ' și Longitudine ' +
              e.latlng.lng.toFixed(9)
          )
          .openOn(this.map);

        // Asociem popup-ul cu marker-ul
        this.currentMarker.bindPopup(popup);
      });
    }, 1000); // Delay of 2 seconds (2000 milliseconds)
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

  constructor(
    private cdr: ChangeDetectorRef,
    private authenticationService: AuthenticationService,
    private locationService: LocationService,
    private reportService: ReportService,
    private emailService: EmailService,
    private userService: UserService,
    private router: Router,
    private route: ActivatedRoute,
    private viewportScroller: ViewportScroller,
    public iconService: IconService
  ) {
    this.router.events
      .pipe(filter((event) => event instanceof NavigationEnd))
      .subscribe(() => {
        this.viewportScroller.scrollToPosition([0, 0]);
      });
  }

  isNotUser() {
    return this.authenticationService.isNotUser();
  }

  isAuthenticated() {
    return this.authenticationService.getAuthStatus();
  }

  toggleInstructions() {
    this.showInstructions = !this.showInstructions;
  }

  removeAlert(index: number): void {
    this.alertErrorMessages.splice(index, 1); // Îndepărtează mesajul de eroare la indexul specificat
  }

  removeSuccessAlert(index: number): void {
    this.alertSuccessMessages.splice(index, 1); // Îndepărtează mesajul de eroare la indexul specificat
  }

  loadCounties(): void {
    this.isLoading = true;
    this.locationService.getAllCounties().subscribe({
      next: (data) => {
        console.log(data);
        this.counties = data;
        this.isLoading = false;
      },
      error: (error) => {
        console.error('There was an error fetching the counties:', error);
        this.alertErrorMessages.push(error.error);
      },
    });
  }

  onCountyChange(): void {
    this.report.city = ''; // Reset city selection
    if (this.report.county) {
      const countyId = +this.report.county;
      this.locationService.getAllCitiesByCountyId(countyId).subscribe({
        next: (data) => {
          this.cities = data;
        },
        error: (error) => {
          console.error(
            'There was an error fetching the cities for the selected county:',
            error
          );
          this.alertErrorMessages.push(error.error);
        },
      });
    }
  }

  onLogout() {
    this.authenticationService.logout();
  }

  toggleDropdown(): void {
    this.isDropdownOpen = !this.isDropdownOpen;
  }

  markFormControlsAsTouched(form: NgForm) {
    Object.keys(form.controls).forEach((field) => {
      const control = form.controls[field];
      control.markAsTouched({ onlySelf: true });
    });
  }

  createReport(reportForm: NgForm) {
    this.formSubmissionAttempt = true; // Set the form submission attempt flag to true

    if (this.report.severity === -1) {
      // Manually mark the severity field as invalid
      const severityControl = reportForm.controls['severityField'];
      if (severityControl) {
        severityControl.setErrors({ invalidSeverity: true });
      }
    }

    if (reportForm.invalid || this.report.severity === -1) {
      this.markFormControlsAsTouched(reportForm);
      console.log('Form is invalid!');
      this.isLoading = false;
      return;
    }

    this.isLoading = true;
    let userId = this.authenticationService.getUserId();
    if (userId) {
      const newReport = new Report();
      newReport.title = this.report.title;
      newReport.description = this.report.description;
      newReport.county = this.report.county;
      newReport.city = this.report.city;
      newReport.locationDetails = this.report.locationDetails;
      newReport.userId = userId;
      newReport.latitude = this.report.latitude;
      newReport.longitude = this.report.longitude;
      newReport.isAnonymous = this.report.isAnonymous;
      newReport.status = this.report.status;
      newReport.severity = this.report.severity;
      newReport.imageFiles = this.report.imageFiles;

      console.log(newReport);
      const formData = this.createFormData(newReport);

      this.reportService.createReportWithImages(formData).subscribe({
        next: () => {
          this.sendEmailNotification(newReport); // Send email after successful report creation
        },
        error: (error) => {
          this.isLoading = false;
          console.error('Report creation failed:', error.error);
          const errors = error.error.errors;
          for (const key in errors) {
            if (errors.hasOwnProperty(key)) {
              this.alertErrorMessages = this.alertErrorMessages.concat(
                errors[key]
              );
            }
          }
        },
        complete: () => {
          this.isLoading = false;

          const successMessage = 'Problema a fost raportata cu succes';
          this.alertSuccessMessages.push(successMessage);

          // Redirect to report-a-problem page with success message
          const navigationExtras: NavigationExtras = {
            queryParams: { message: successMessage },
          };
          this.router.navigate(['/home'], navigationExtras);
        },
      });
    } else {
      this.isLoading = false;
    }
  }

  private async sendEmailNotification(report: Report) {
    const { userName, cityName, countyName } =
      await this.getUserCityCountyDetails(
        report.userId,
        +report.city,
        +report.county
      );

    this.locationService.getCityEmailById(+report.city).subscribe(
      (response: any) => {
        const cityEmail = response.email;
        this.sendEmail(report, cityEmail, userName, cityName, countyName);

        if (report.severity == 4) {
          // 4 is the highest severity level (Critic)
          this.locationService.getCountyEmailById(+report.county).subscribe(
            (response: any) => {
              const countyEmail = response.email;
              this.sendEmail(
                report,
                countyEmail,
                userName,
                cityName,
                countyName
              );
            },
            (error) => {
              const errorMessage =
                'Nu s-a putut prelua adresa de email a județului: ' +
                error.error;
              console.error(errorMessage);
              this.alertErrorMessages.push(errorMessage);
            }
          );
        }
      },
      (error) => {
        const errorMessage =
          'Nu s-a putut prelua adresa de email a orașului: ' + error.error;
        console.error(errorMessage);
        this.alertErrorMessages.push(errorMessage);
      }
    );
  }

  private sendEmail(
    report: Report,
    recipient: string,
    userName: string,
    cityName: string,
    countyName: string
  ) {
    const severityText = this.getSeverityText(report.severity);
    const userNameText = report.isAnonymous
      ? 'un utilizator anonim'
      : `utilizatorul <strong>${userName}</strong>`;

    const email = {
      to: recipient,
      subject:
        'O nouă problemă legată de apă raportată pe platforma Aqua Defender',
      body: `
        <p>Bună ziua,</p>
    
        <p>A fost raportată o nouă problemă pe platforma Aqua Defender de către ${userNameText}.</p>
    
        <p>Titlul acesteia este <strong>${report.title}</strong>. Problema a fost descrisă astfel: <strong>${report.description}</strong>.</p>
    
        <p>Problema a avut loc în județul <strong>${countyName}</strong>, localitatea <strong>${cityName}</strong>, având coordonatele latitudine <strong>${report.latitude}</strong> și longitudine <strong>${report.longitude}</strong>.</p>
    
        <p>Detalii suplimentare despre locație: <strong>${report.locationDetails}</strong>.</p>
    
        <p>Gradul de severitate al problemei este <strong>${severityText}</strong>.</p>
    
        <p>Vă rugăm să accesați platforma noastră pentru mai multe detalii, pentru a putea rezolva problema și pentru a ține oamenii actualizați cu statusul acesteia.</p>
    
        <p>Cu respect,</p>
        <p>Echipa Aqua Defender</p>
      `,
    };

    this.emailService.sendEmail(email).subscribe(
      (response) => {
        console.log(`Email trimis cu succes la ${recipient}`);
      },
      (error) => {
        const errorMessage =
          `Trimiterea emailului către ${recipient} a eșuat: ` + error.error;
        console.error(errorMessage);
        this.alertErrorMessages.push(errorMessage);
      }
    );
  }

  private async getUserCityCountyDetails(
    userId: number,
    cityId: number,
    countyId: number
  ) {
    try {
      const details = await forkJoin({
        user: this.userService.getUserById(userId),
        city: this.locationService.getCityById(cityId),
        county: this.locationService.getCountyById(countyId),
      }).toPromise();

      if (!details) {
        throw new Error('Detaliile sunt nedefinite');
      }

      return {
        userName: details.user?.userName ?? 'Utilizator necunoscut',
        cityName: details.city?.name ?? 'Oraș necunoscut',
        countyName: details.county?.name ?? 'Județ necunoscut',
      };
    } catch (error: any) {
      const errorMessage =
        'Eroare la preluarea detaliilor utilizatorului, orașului sau județului: ' +
        error.error;
      console.error(errorMessage);
      this.alertErrorMessages.push(errorMessage);
      return {
        userName: 'Utilizator necunoscut',
        cityName: 'Oraș necunoscut',
        countyName: 'Județ necunoscut',
      };
    }
  }

  private getSeverityText(severity: number): string {
    if (severity == 0) {
      return 'minor';
    } else if (severity == 1) {
      return 'moderat';
    } else if (severity == 2) {
      return 'serios';
    } else if (severity == 3) {
      return 'sever';
    } else if (severity == 4) {
      return 'critic';
    } else {
      return 'necunoscut';
    }
  }

  // Funcția care converteste un obiect Report în FormData
  createFormData(report: Report): FormData {
    const formData = new FormData();
    formData.append('Title', report.title);
    formData.append('Description', report.description);
    formData.append('CountyId', report.county);
    formData.append('CityId', report.city);
    formData.append('LocationDetails', report.locationDetails || '');
    formData.append('Latitude', report.latitude.toString());
    formData.append('Longitude', report.longitude.toString());
    formData.append('IsAnonymous', report.isAnonymous.toString());
    formData.append('Status', report.status.toString());
    formData.append('Severity', report.severity.toString());

    formData.append('UserId', report.userId.toString());

    // Adăugarea fișierelor imagine
    if (report.imageFiles) {
      for (const imageFile of this.report.imageFiles) {
        formData.append('Images', imageFile);
      }
    }

    return formData;
  }

  handleImageUpload(event: Event): void {
    this.isLoading = true;

    const element = event.target as HTMLInputElement;
    const newFiles = element.files;

    if (newFiles) {
      // Adaugă fiecare fișier nou la lista de fișiere și creează un URL blob pentru previzualizare
      for (let file of Array.from(newFiles)) {
        const blobUrl = URL.createObjectURL(file);
        this.imagePreviews.push(blobUrl); // Adaugă URL-ul blob pentru previzualizare
        this.report.imageFiles.push(file);
      }
      // În caz că Angular nu detectează schimbările, marchează pentru verificare
      this.cdr.markForCheck();
    }
    this.isLoading = false;
  }

  removeImage(index: number): void {
    // Elimină fișierul și URL-ul blob corespunzător
    this.report.imageFiles.splice(index, 1);
    URL.revokeObjectURL(this.imagePreviews[index]); // Curăță memoria revocând URL-ul blob
    this.imagePreviews.splice(index, 1);

    this.cdr.markForCheck();
  }

  ngOnDestroy(): void {
    this.imagePreviews.forEach((url) => URL.revokeObjectURL(url));
  }

  getPreviewImage(imageFile: File): string {
    return URL.createObjectURL(imageFile);
  }
}
