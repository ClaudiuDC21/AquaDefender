export class User {
  id?: number; // Opțional pentru utilizatori noi unde ID-ul este generat de backend
  userName: string = '';
  email: string = '';
  birthDate: Date = new Date();
  phoneNumber: string = '';
  registrationDate: Date = new Date(); // Acesta va fi setat în backend, așa că este opțional aici
  countyId: number = 0;
  cityId: number = 0;
  profilePictureUrl: string[] = [];
  currentIndex = 0;

  constructor(
    id?: number,
    userName: string = '',
    email: string = '',
    birthDate: Date = new Date(),
    phoneNumber: string = '',
    registrationDate: Date = new Date(),
    countyId: number = 0,
    cityId: number = 0,
    profilePictureUrl: string[] = [],
    currentIndex = 0
  ) {
    this.id = id;
    this.userName = userName;
    this.email = email;
    this.birthDate = birthDate;
    this.phoneNumber = phoneNumber;
    this.registrationDate = registrationDate;
    this.countyId = countyId;
    this.cityId = cityId;
    this.profilePictureUrl = profilePictureUrl;
    this.currentIndex = currentIndex;
  }
}
