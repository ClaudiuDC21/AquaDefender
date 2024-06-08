import { Injectable } from '@angular/core';
import {
  IconDefinition,
  faHome,
  faTint,
  faExclamationTriangle,
  faTasks,
  faUser,
  faEdit,
  faWater,
  faSignInAlt,
  faUserPlus,
  faCog,
  faUserCircle,
  faEyeSlash,
  faEye,
  faXmark,
  faTimesCircle,
  faInfoCircle,
  faCheck,
  faWarning,
  faKey,
  faTrash,
  faHourglassStart,
  faHourglassHalf,
  faCheckCircle,
  faUserEdit,
  faQuestionCircle,
  faCalendarCheck,
  faFileMedical,
  faTimes,
  faSave,
} from '@fortawesome/free-solid-svg-icons';

@Injectable({
  providedIn: 'root',
})
export class IconService {
  faHome: IconDefinition = faHome;
  faTint: IconDefinition = faTint;
  faExclamationTriangle: IconDefinition = faExclamationTriangle;
  faTasks: IconDefinition = faTasks;
  faUser: IconDefinition = faUser;
  faEdit: IconDefinition = faEdit;
  faWater: IconDefinition = faWater;
  faEyeSlash: IconDefinition = faEyeSlash;
  faEye: IconDefinition = faEye;
  faSignInAlt: IconDefinition = faSignInAlt;
  faUserPlus: IconDefinition = faUserPlus;
  faCog: IconDefinition = faCog;
  faUserCircle: IconDefinition = faUserCircle;
  faClose: IconDefinition = faXmark;
  faError: IconDefinition = faTimesCircle;
  faInfo: IconDefinition = faInfoCircle;
  faSuccess: IconDefinition = faCheck;
  faWarning: IconDefinition = faWarning;
  faKey: IconDefinition = faKey;
  faTrash: IconDefinition = faTrash;
  faHourglassStart: IconDefinition = faHourglassStart;
  faHourglassHalf: IconDefinition = faHourglassHalf;
  faCheckCircle: IconDefinition = faCheckCircle;
  faUserEdit: IconDefinition = faUserEdit;
  faHelp: IconDefinition = faQuestionCircle;
  calendarCheck: IconDefinition = faCalendarCheck;
  fileMedical: IconDefinition = faFileMedical;
  faTimes: IconDefinition = faTimes;
  faSave: IconDefinition = faSave;

  constructor() {}
}
