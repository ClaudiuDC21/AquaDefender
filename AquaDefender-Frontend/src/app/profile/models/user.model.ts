export class User {
  id?: number;
  userName: string = '';
  email: string = '';
  birthDate: Date = new Date();
  phoneNumber: string = '';
  registrationDate: Date = new Date();
  countyId: number = 0;
  cityId: number = 0;
  profilePictureUrl: string[] = [];
  hasProfilePicture: boolean = false;
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
    hasProfilePicture: boolean = false,
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
    this.hasProfilePicture = hasProfilePicture;
    this.currentIndex = currentIndex;
  }
}
