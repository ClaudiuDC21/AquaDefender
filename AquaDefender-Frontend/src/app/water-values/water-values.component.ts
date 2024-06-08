import { Component } from '@angular/core';
import { AuthenticationService } from '../authentication/services/authentication.service';
import { LocationService } from '../utils/services/location.service';
import { WaterInfoService } from './services/water-info.service';
import { WaterValuesService } from './services/water-values.service';
import { NavigationEnd, Router } from '@angular/router';
import { WaterInfo } from './models/water-info.model';
import { DatePipe, ViewportScroller } from '@angular/common';
import { filter } from 'rxjs';
import { IconService } from '../utils/services/icon.service';

@Component({
  selector: 'app-water-values',
  templateUrl: './water-values.component.html',
  styleUrl: './water-values.component.scss',
})
export class WaterValuesComponent {
  reportId: number = 0;
  counties: any[] = [];
  cities: any[] = [];
  reportDates: any[] = [];
  waterInfo: WaterInfo = {
    id: 0,
    name: '',
    county: '',
    city: '',
    dateReported: new Date(),
    additionalNotes: '',
  };
  waterValues: any[] = [];

  editMode: boolean = false;
  isAdding: boolean = false;
  isUpdating: boolean = false;
  isDeleting: boolean = false;
  isReportFetched: boolean = false;
  showDeleteConfirmation: boolean = false;
  showDate: boolean = false;
  showInstructions: boolean = false;

  isLoading = false;
  alertErrorMessages: string[] = [];
  alertSuccessMessages: string[] = [];
  alertInfoMessages: string[] = [];
  alertWarningMessages: string[] = [];

  constructor(
    private router: Router,
    private authenticationService: AuthenticationService,
    private locationService: LocationService,
    private waterInfoService: WaterInfoService,
    private waterValuesService: WaterValuesService,
    private viewportScroller: ViewportScroller,
    private datePipe: DatePipe,
    public iconService: IconService
  ) {
    this.router.events
      .pipe(filter((event) => event instanceof NavigationEnd))
      .subscribe(() => {
        this.viewportScroller.scrollToPosition([0, 0]);
      });
  }

  isAuthenticated(): boolean {
    return this.authenticationService.getAuthStatus();
  }

  isNotUser() {
    return this.authenticationService.isNotUser();
  }

  toggleInstructions() {
    this.showInstructions = !this.showInstructions;
  }

  formatDate(date: string | Date): string {
    return this.datePipe.transform(date ?? new Date(), 'dd.MM.yyyy') as string;
  }

  ngOnInit(): void {
    this.loadCounties();
  }

  removeAlert(index: number): void {
    this.alertErrorMessages.splice(index, 1);
  }

  removeSuccessAlert(index: number): void {
    this.alertSuccessMessages.splice(index, 1);
  }

  removeInfoAlert(index: number): void {
    this.alertInfoMessages.splice(index, 1);
  }

  removeWarningAlert(index: number): void {
    this.alertWarningMessages.splice(index, 1);
  }

  loadCounties(): void {
    this.isLoading = true;
    this.locationService.getAllCounties().subscribe({
      next: (data) => {
        this.counties = data;
        this.isLoading = false;
      },
      error: (error) => {
        const errorMessage =
          'A apărut o eroare la preluarea județelor: ' + error.error;
        console.error(errorMessage);
        this.isLoading = false;
        this.alertErrorMessages.push(errorMessage);
      },
    });
  }

  onCountyChange(): void {
    this.waterInfo.city = '';
    if (this.waterInfo.county) {
      const countyId = +this.waterInfo.county;
      this.locationService.getAllCitiesByCountyId(countyId).subscribe({
        next: (data) => {
          this.cities = data;
        },
        error: (error) => {
          const errorMessage =
            'A apărut o eroare la preluarea orașelor pentru județul selectat: ' +
            error.error;
          console.error(errorMessage);
          this.alertErrorMessages.push(errorMessage);
        },
      });
    }
  }

  onCityChange(): void {
    if (this.waterInfo.city && this.editMode === false) {
      const cityId = +this.waterInfo.city;
      this.waterInfoService.getAllWaterInfosByCityId(cityId).subscribe({
        next: (waterInfos) => {
          const dates = waterInfos.map((info) => info.dateReported);
          this.reportDates = [...new Set(dates)].sort(
            (a, b) => new Date(b).getTime() - new Date(a).getTime()
          );
          this.alertErrorMessages = [];
        },
        error: (error) => {
          const message =
            error.error.message ||
            'A apărut o problemă neașteptată, vă rugăm să încercați din nou.';
          this.alertErrorMessages.push(message);
          console.error(
            'A apărut o eroare la preluarea informațiilor despre apă:',
            error
          );
          this.reportDates = [];
        },
      });
    } else {
      this.reportDates = [];
      if (this.editMode === false) {
        this.alertErrorMessages.push(
          'Vă rugăm să selectați un oraș pentru a obține informații.'
        );
      }
    }
  }

  onDateChange(): void {
    this.isLoading = true;
    this.editMode = false;
    if (
      this.waterInfo.city &&
      this.waterInfo.dateReported &&
      this.editMode === false
    ) {
      this.getReportByDateAndCity();
    }
    this.isLoading = false;
  }

  getReportByDateAndCity() {
    this.isLoading = true;
    const dateReported = new Date(this.waterInfo.dateReported);
    const formattedDate = dateReported.toISOString().split('T')[0];

    this.waterInfoService
      .getReportByDateAndCity(formattedDate, +this.waterInfo.city)
      .subscribe({
        next: (report) => {
          if (report && report.id) {
            this.reportId = report.id;
            this.waterInfo.additionalNotes = report.additionalNotes || '';
            this.waterValuesService
              .getAllWaterValuesByWaterInfoId(report.id)
              .subscribe({
                next: (waterValues) => {
                  this.tableData = waterValues.map((value) => ({
                    id: value.id,
                    param: value.name,
                    value: value.maximumAllowedValue,
                    userProvidedValue: value.userProvidedValue,
                    unit: value.measurementUnit,
                    editMode: false,
                  }));
                  this.isReportFetched = true;
                  this.isLoading = false;
                },
                error: (error) => {
                  const errorMessage =
                    'A apărut o eroare la preluarea valorilor apei: ' +
                    error.error;
                  console.error(errorMessage);
                  this.isLoading = false;
                  this.alertErrorMessages.push(errorMessage);
                },
              });
          }
        },
        error: (error) => {
          const errorMessage =
            'A apărut o eroare la preluarea raportului: ' + error.error;
          console.error(errorMessage);
          this.alertErrorMessages.push(errorMessage);
        },
      });
  }

  activateDate() {
    this.showDate = true;
  }

  addReport(): void {
    setTimeout(() => {
      if (this.editMode)
        this.alertWarningMessages.push(
          'Nu uita să salvezi datele după ce ai introdus datele necesare, apăsând butonul de sub tabel.'
        );
    }, 25000);
    this.editMode = true;
    this.isAdding = true;
    this.isUpdating = false;
    this.isDeleting = false;
    this.isReportFetched = false;
    this.waterInfo.county = '';
    this.cities = [];
    this.reportDates = [];
    this.waterInfo.dateReported = new Date(
      new Date().toLocaleString('en-US', { timeZone: 'Europe/Bucharest' })
    );
    this.tableData.forEach((item) => (item.userProvidedValue = ''));
    this.waterInfo.additionalNotes = '';
  }

  modifyReport(): void {
    setTimeout(() => {
      if (this.editMode)
        this.alertWarningMessages.push(
          'Nu uita să salvezi datele după ce ai introdus datele necesare, apăsând butonul de sub tabel.'
        );
    }, 25000);
    this.editMode = !this.editMode;
    this.isAdding = false;
    this.isUpdating = true;
    this.isDeleting = false;
  }

  saveWaterInfoAndValues(): void {
    this.isLoading = true;
    this.waterInfoService.createWaterInfo(this.waterInfo).subscribe({
      next: (createdWaterInfo) => {
        console.log('WaterInfo creat cu succes', createdWaterInfo);

        const waterValuesToCreate = this.tableData.map((item) => ({
          Id: item.id,
          Name: item.param,
          MaximumAllowedValue: item.value,
          UserProvidedValue: item.userProvidedValue,
          MeasurementUnit: item.unit,
          IdWaterInfo: createdWaterInfo.id,
        }));

        waterValuesToCreate.forEach((waterValuesDto) => {
          this.waterValuesService.createWaterValues(waterValuesDto).subscribe({
            next: (createdWaterValues) => {
              console.log('WaterValues create cu succes', createdWaterValues);
            },
            error: (error) => {
              console.error('Eroare la crearea WaterValues', error);
              this.isLoading = false;
              this.alertErrorMessages.push(
                'Eroare la crearea valorilor apei: ' + error.error
              );
            },
          });
        });
        this.isLoading = false;
        this.alertSuccessMessages.push(
          'Un nou raport de apă potabilă a fost introdus!'
        );
        this.editMode = false;
      },
      error: (error) => {
        console.error('Eroare la crearea WaterInfo', error);
        this.isLoading = false;
        this.alertErrorMessages.push(
          'Eroare la crearea informațiilor despre apă: ' + error.error
        );
      },
    });
  }

  modifyWaterValues() {
    this.isLoading = true;
    this.waterInfoService
      .updateWaterInfo(this.reportId, this.waterInfo)
      .subscribe({
        next: (updatedWaterInfo) => {
          console.log('WaterInfo actualizat cu succes', updatedWaterInfo);

          this.tableData.forEach((item) => {
            const updatedWaterValues = {
              Id: item.id,
              Name: item.param,
              MaximumAllowedValue: item.value,
              UserProvidedValue: item.userProvidedValue,
              MeasurementUnit: item.unit,
            };

            this.waterValuesService
              .updateWaterValues(item.id, updatedWaterValues)
              .subscribe({
                next: (updatedWaterValues) => {
                  console.log(
                    'WaterValues actualizat cu succes',
                    updatedWaterValues
                  );
                },
                error: (error) => {
                  console.error('Eroare la actualizarea WaterValues', error);
                  this.isLoading = false;
                  this.alertErrorMessages.push(
                    'Eroare la actualizarea valorilor apei: ' + error.error
                  );
                },
              });
          });
          this.editMode = false;
          this.isLoading = false;
          this.alertSuccessMessages.push(
            'Raportul a fost modificat cu succes!'
          );
        },
        error: (error) => {
          console.error('Eroare la actualizarea WaterInfo', error);
          this.isLoading = false;
          this.alertErrorMessages.push(
            'Eroare la actualizarea informațiilor despre apă: ' + error.error
          );
        },
      });
  }

  deleteWaterInfoAndValues() {
    this.showDeleteConfirmation = true;
    this.editMode = false;
    this.isAdding = false;
    this.isUpdating = false;
    this.isDeleting = true;
  }

  closePopup() {
    this.showDeleteConfirmation = false;
    this.showDate = false;
  }

  confirmDelete() {
    this.isLoading = true;
    this.waterInfoService.deleteWaterInfo(this.reportId).subscribe({
      next: (response) => {
        location.reload();
        this.isLoading = false;
        console.log(
          'Informațiile despre apă au fost șterse cu succes',
          response
        );
        this.showDeleteConfirmation = false;
        this.alertSuccessMessages.push('Raportul a fost șters cu succes');
      },
      error: (error) => {
        this.isLoading = false;
        console.error('Eroare la ștergerea informațiilor despre apă', error);
        this.alertErrorMessages.push(
          'Eroare la ștergerea informațiilor despre apă: ' + error.error
        );
      },
    });
  }

  confirmShowDate() {
    this.isAdding = false;
    this.editMode = false;
    this.showDate = false;
    this.waterInfo.county = '';
    this.cities = [];
  }

  tableData = [
    {
      id: 0,
      param: 'pH',
      value: '≥ 6,5; ≤ 9,5 unități de pH',
      userProvidedValue: '',
      unit: 'unități de pH',
      editMode: false,
    },
    {
      id: 0,
      param: 'Conductivitate Electrica',
      value: '2.500 µS cm-1 la 20°C',
      userProvidedValue: '',
      unit: 'µS cm-1 la 20°C',
      editMode: false,
    },
    {
      id: 0,
      param: 'Totalitatea solidelor dizolvate',
      value: '1250 ppm',
      userProvidedValue: '',
      unit: 'Parti per milion',
      editMode: false,
    },
    {
      id: 0,
      param: 'Nitriți',
      value: '0,50 mg/l',
      userProvidedValue: '',
      unit: 'mg/l',
      editMode: false,
    },
    {
      id: 0,
      param: 'Nitrați',
      value: '50 mg/l',
      userProvidedValue: '',
      unit: 'mg/l',
      editMode: false,
    },
    {
      id: 0,
      param: 'Duritate totala',
      value: '≥ 5°',
      userProvidedValue: '',
      unit: 'grade germane',
      editMode: false,
    },
    {
      id: 0,
      param: 'Clor rezidual liber',
      value: '≥ 0,1 - ≤ 0,5 mg/l',
      userProvidedValue: '',
      unit: 'mg/l',
      editMode: false,
    },
    {
      id: 0,
      param: 'Escherichia coli (E. coli)',
      value: '0/100 ml',
      userProvidedValue: '',
      unit: 'număr/100 ml',
      editMode: false,
    },
    {
      id: 0,
      param: 'Enterococi',
      value: '0/100 ml',
      userProvidedValue: '',
      unit: 'număr/100 ml',
      editMode: false,
    },
    {
      id: 0,
      param: 'Amoniu',
      value: '0,50 mg/l',
      unit: 'mg/l',
      editMode: false,
    },
    {
      id: 0,
      param: 'Pseudomonas Aeruginosa',
      value: '0/250 ml',
      userProvidedValue: '',
      unit: 'număr/250 ml',
      editMode: false,
    },
    {
      id: 0,
      param: 'Număr colonii la 22°C',
      value: '100/ml',
      userProvidedValue: '',
      unit: 'număr/ml',
      editMode: false,
    },
    {
      id: 0,
      param: 'Număr colonii la 37°C',
      value: '20/ml',
      userProvidedValue: '',
      unit: 'număr/ml',
      editMode: false,
    },
    {
      id: 0,
      param: 'Acrilamida',
      value: '0,10 µg/l',
      userProvidedValue: '',
      unit: 'µg/l',
      editMode: false,
    },
    {
      id: 0,
      param: 'Arsen',
      value: '10 µg/l',
      userProvidedValue: '',
      unit: 'µg/l',
      editMode: false,
    },
    {
      id: 0,
      param: 'Benzen',
      value: '0 µg/l',
      userProvidedValue: '',
      unit: 'µg/l',
      editMode: false,
    },
    {
      id: 0,
      param: 'Benzo(a)piren',
      value: '0,01 µg/l',
      userProvidedValue: '',
      unit: 'µg/l',
      editMode: false,
    },
    {
      id: 0,
      param: 'Bor',
      value: '1,0 mg/l',
      userProvidedValue: '',
      unit: 'mg/l',
      editMode: false,
    },
    {
      id: 0,
      param: 'Bromati',
      value: '10 µg/l',
      userProvidedValue: '',
      unit: 'µg/l',
      editMode: false,
    },
    {
      id: 0,
      param: 'Cadmiu',
      value: '5,0 µg/l',
      userProvidedValue: '',
      unit: 'µg/l',
      editMode: false,
    },
    {
      id: 0,
      param: 'Clorura de vinil',
      value: '0,50 µg/l',
      userProvidedValue: '',
      unit: 'µg/l',
      editMode: false,
    },
    {
      id: 0,
      param: 'Cianuri totale',
      value: '50 µg/l',
      userProvidedValue: '',
      unit: 'µg/l',
      editMode: false,
    },
    {
      id: 0,
      param: 'Cianuri libere',
      value: '10 µg/l',
      userProvidedValue: '',
      unit: 'µg/l',
      editMode: false,
    },
    {
      id: 0,
      param: 'Crom total',
      value: '50 µg/l',
      userProvidedValue: '',
      unit: 'µg/l',
      editMode: false,
    },
    {
      id: 0,
      param: 'Cupru',
      value: '2,0 mg/l',
      userProvidedValue: '',
      unit: 'mg/l',
      editMode: false,
    },
    {
      id: 0,
      param: 'Dicloretan',
      value: '3,0 µg/l',
      userProvidedValue: '',
      unit: 'µg/l',
      editMode: false,
    },
    {
      id: 0,
      param: 'Epiclorhidrina',
      value: '0,10 µg/l',
      userProvidedValue: '',
      unit: 'µg/l',
      editMode: false,
    },
    {
      id: 0,
      param: 'Fluoruri',
      value: '1,5 mg/l',
      userProvidedValue: '',
      unit: 'mg/l',
      editMode: false,
    },
    {
      id: 0,
      param: 'Hidrocarburi policiclice aromatice',
      value: '0,10 µg/l',
      userProvidedValue: '',
      unit: 'µg/l',
      editMode: false,
    },
    {
      id: 0,
      param: 'Mercur',
      value: '1,0 µg/l',
      userProvidedValue: '',
      unit: 'µg/l',
      editMode: false,
    },
    {
      id: 0,
      param: 'Nichel',
      value: '20 µg/l',
      userProvidedValue: '',
      unit: 'µg/l',
      editMode: false,
    },
    {
      id: 0,
      param: 'Pesticide',
      value: '0,10 µg/l',
      userProvidedValue: '',
      unit: 'µg/l',
      editMode: false,
    },
    {
      id: 0,
      param: 'Pesticide total',
      value: '0,50 µg/l',
      userProvidedValue: '',
      unit: 'µg/l',
      editMode: false,
    },
    {
      id: 0,
      param: 'Plumb',
      value: '10 µg/l',
      userProvidedValue: '',
      unit: 'µg/l',
      editMode: false,
    },
    {
      id: 0,
      param: 'Seleniu',
      value: '10 µg/l',
      userProvidedValue: '',
      unit: 'µg/l',
      editMode: false,
    },
    {
      id: 0,
      param: 'Stibiu',
      value: '5,0 µg/l',
      userProvidedValue: '',
      unit: 'µg/l',
      editMode: false,
    },
    {
      id: 0,
      param: 'Tetracloreten și Tricloreten',
      value: '10 µg/l',
      userProvidedValue: '',
      unit: 'µg/l',
      editMode: false,
    },
    {
      id: 0,
      param: 'Trihalometani total',
      value: '100 µg/l',
      userProvidedValue: '',
      unit: 'µg/l',
      editMode: false,
    },
    {
      id: 0,
      param: 'Aluminiu',
      value: '200 µg/l',
      userProvidedValue: '',
      unit: 'µg/l',
      editMode: false,
    },
    {
      id: 0,
      param: 'Bacterii Coliforme',
      value: '0 număr/100 ml',
      userProvidedValue: '',
      unit: 'număr/100 ml',
      editMode: false,
    },
    {
      id: 0,
      param: 'Carbon Organic Total',
      value: 'Nicio modificare anormală',
      userProvidedValue: '',
      unit: '-',
      editMode: false,
    },
    {
      id: 0,
      param: 'Cloruri',
      value: '250 mg/l',
      userProvidedValue: '',
      unit: 'mg/l',
      editMode: false,
    },
    {
      id: 0,
      param: 'Clostridium Perfringens',
      value: '0 număr/100 ml',
      userProvidedValue: '',
      unit: 'număr/100 ml',
      editMode: false,
    },
    {
      id: 0,
      param: 'Culoare',
      value: 'Acceptabilă consumatorilor',
      userProvidedValue: '',
      unit: '-',
      editMode: false,
    },
    {
      id: 0,
      param: 'Fier',
      value: '200 µg/l',
      userProvidedValue: '',
      unit: 'µg/l',
      editMode: false,
    },
    {
      id: 0,
      param: 'Gust',
      value: 'Acceptabil consumatorilor',
      userProvidedValue: '',
      unit: '-',
      editMode: false,
    },
    {
      id: 0,
      param: 'Mangan',
      value: '50 µg/l',
      userProvidedValue: '',
      unit: 'µg/l',
      editMode: false,
    },
    {
      id: 0,
      param: 'Miros',
      value: 'Acceptabil consumatorilor',
      userProvidedValue: '',
      unit: '-',
      editMode: false,
    },
    {
      id: 0,
      param: 'Oxidabilitate',
      value: '5,0 mg O2/l',
      userProvidedValue: '',
      unit: 'mg O2/l',
      editMode: false,
    },
    {
      id: 0,
      param: 'Sodiu',
      value: '200 mg/l',
      userProvidedValue: '',
      unit: 'mg/l',
      editMode: false,
    },
    {
      id: 0,
      param: 'Sulfat',
      value: '250 mg/l',
      userProvidedValue: '',
      unit: 'mg/l',
      editMode: false,
    },
    {
      id: 0,
      param: 'Sulfuri și Hidrogen Sulfurat',
      value: '100 µg/l',
      userProvidedValue: '',
      unit: 'µg/l',
      editMode: false,
    },
    {
      id: 0,
      param: 'Turbiditate',
      value: '≤ UNT',
      userProvidedValue: '',
      unit: '-',
      editMode: false,
    },
    {
      id: 0,
      param: 'Zinc',
      value: '5.000 µg/l',
      userProvidedValue: '',
      unit: 'µg/l',
      editMode: false,
    },
    {
      id: 0,
      param: 'Tritiu',
      value: '100 Bq/l',
      userProvidedValue: '',
      unit: 'Bq/l',
      editMode: false,
    },
    {
      id: 0,
      param: 'Doza Efectivă Totală de Referință',
      value: '0,10 mSv/an',
      userProvidedValue: '',
      unit: 'mSv/an',
      editMode: false,
    },
    {
      id: 0,
      param: 'Activitatea Alfa Globală',
      value: '0,1 Bq/l',
      userProvidedValue: '',
      unit: 'Bq/l',
      editMode: false,
    },
    {
      id: 0,
      param: 'Activitatea Beta Globală',
      value: '1 Bq/l',
      userProvidedValue: '',
      unit: 'Bq/l',
      editMode: false,
    },
  ];
}
