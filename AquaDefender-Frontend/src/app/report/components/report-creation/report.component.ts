import { HttpClient } from '@angular/common/http';
import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { AuthenticationService } from '../../../authentication/services/authentication.service';
import * as L from 'leaflet';
import { LocationService } from '../../../other-services/location.service';
import { Report } from '../../models/report.model';
import { ReportService } from '../../services/report.service';
import { ViewportScroller } from '@angular/common';
import { NavigationEnd, Router } from '@angular/router';
import { filter } from 'rxjs';
import { AbstractControl, FormArray, FormGroup, NgForm } from '@angular/forms';
import { IconService } from '../../../other-services/icon.service';

@Component({
  selector: 'app-report',
  templateUrl: './report.component.html',
  styleUrl: './report.component.scss',
})
export class ReportComponent implements OnInit {
  isDropdownOpen: boolean = false;
  showInstructions: boolean = false;
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
    }, 2000); // Delay of 2 seconds (2000 milliseconds)
  }
  
  constructor(
    private cdr: ChangeDetectorRef,
    private authenticationService: AuthenticationService,
    private locationService: LocationService,
    private reportService: ReportService,
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
    this.alertErrorMessages.splice(index, 1); // Îndepărtează mesajul de eroare la indexul specificat
  }

  removeInfoAlert(index: number): void {
    this.alertInfoMessages.splice(index, 1); // Remove the info message at the specified index
  }

  removeWarningAlert(index: number): void {
    this.alertWarningMessages.splice(index, 1); // Remove the warning message at the specified index
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

  
  createReport(reportForm: NgForm) {
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
      newReport.imageFiles = this.report.imageFiles; // Presupunem că acesta este un array de File

      // Conversia datelor raportului într-un obiect FormData pentru încărcare
      const formData = this.createFormData(newReport);

      this.reportService.createReportWithImages(formData).subscribe(
        (response) => {
          console.log('Report creation successful', response);
          this.isLoading = false;
          this.alertSuccessMessages.push('Problema a fost raportata cu succes');
        },
        (error) => {
          console.error('Report creation failed:', error.error);
          const errors = error.error.errors;

          for (const key in errors) {
            if (errors.hasOwnProperty(key)) {
              this.alertErrorMessages = this.alertErrorMessages.concat(
                errors[key]
              );
            }
          }
        }
      );
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

    // Asigură-te că userId este setat corespunzător înainte de acest punct
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

    // Marchează pentru verificare după schimbările efectuate în array-uri
    this.cdr.markForCheck();
  }

  ngOnDestroy(): void {
    this.imagePreviews.forEach((url) => URL.revokeObjectURL(url));
  }

  // handleImageUpload(event: any): void {
  //   const fileList: FileList | null = event.target.files;

  //   if (fileList) {
  //     this.report.imageFiles = Array.from(fileList);
  //   }
  // }

  // removeImage(index: number): void {
  //   this.report.imageFiles.splice(index, 1);
  // }

  getPreviewImage(imageFile: File): string {
    return URL.createObjectURL(imageFile);
  }
}
