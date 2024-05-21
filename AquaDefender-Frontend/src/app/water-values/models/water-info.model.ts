export interface WaterInfo {
  id: number;
  name: string;
  county: string;
  city: string;
  dateReported: Date;
  additionalNotes?: string; // Facem această proprietate opțională, similar cu "= DateTime.Now;" din C#
}
