export class Report {
    id?: number; // Opțional pentru rapoarte noi unde ID-ul este generat de backend
    title: string = '';
    description: string = '';
    reportDate?: Date; // Acesta va fi setat în backend, așa că este opțional aici
    userId: number = 0;
    username: string = '';
    county: string = '';
    city: string = '';
    locationDetails: string = '';
    latitude: number = 0;
    longitude: number = 0;
    isAnonymous: boolean = false;
    status: number = 0; // Inițializează cu un string gol sau cu o valoare default validă
    statusText: string = '';
    severity: number = 0; // Inițializează cu un string gol sau cu o valoare default validă
    severityText: string = '';
    imageFiles: File[] = [];
    imageUrls: string[] = [];
    currentIndex: number = 0;
    hasImages:boolean = false;


  constructor(
    id?: number, 
    title: string = '', 
    description: string = '', 
    reportDate?: Date, 
    userId: number = 0, 
    username: string = '',
    county: string = '', 
    city: string = '', 
    locationDetails: string = '', 
    latitude: number = 0, 
    longitude: number = 0, 
    isAnonymous: boolean = false, 
    status: number = 0, 
    statusText: string = '',
    severity: number = 0, // Inițializează cu un string gol sau cu o valoare default validă
    severityText: string = '',
    imageFiles: File[] = [],
    imageUrls: string[] = [],
    currentIndex: number = 0,
    hasImages:boolean = false
  ) {
    this.id = id;
    this.title = title;
    this.description = description;
    this.reportDate = reportDate;
    this.userId = userId;
    this.username = username;
    this.county = county;
    this.city = city;
    this.locationDetails = locationDetails;
    this.latitude = latitude;
    this.longitude = longitude;
    this.isAnonymous = isAnonymous;
    this.status = status;
    this.statusText = statusText;
    this.severity = severity;
    this.severityText = severityText;
    this.imageFiles = imageFiles;
    this.imageUrls = imageUrls;
    this.currentIndex = currentIndex;
    this.hasImages = hasImages;
  }
}