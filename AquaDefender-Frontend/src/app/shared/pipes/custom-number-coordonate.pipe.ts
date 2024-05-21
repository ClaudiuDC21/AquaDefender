import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'customNumberCoordonate',
})
export class CustomNumberCoordonatePipe implements PipeTransform {
  transform(value: number): string | null {
    // Verifică dacă valoarea este un număr valid
    if (value === null || value === undefined) return null;

    // Convertește numărul într-un string
    const numStr = value.toString();

    // Verifică dacă numărul are cel puțin două cifre
    if (numStr.length < 2) return value.toFixed(5); // dacă nu, folosește formatul standard

    // Extragerea primele două cifre și următoarele cifre necesare
    const firstTwoDigits = numStr.substring(0, 2);
    const restOfNumber = numStr.substring(2);

    // Converteste restul numărului înapoi în float și limitează la 5 zecimale
    const formattedRest = parseFloat('0.' + restOfNumber)
      .toFixed(5)
      .substring(2);

    return `${firstTwoDigits}.${formattedRest}`;
  }
}
