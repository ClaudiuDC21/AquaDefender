import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'customNumberCoordonate',
})
export class CustomNumberCoordonatePipe implements PipeTransform {
  transform(value: number): string | null {
    if (value === null || value === undefined) return null;

    const numStr = value.toString();

    if (numStr.length < 2) return value.toFixed(5);

    const firstTwoDigits = numStr.substring(0, 2);
    const restOfNumber = numStr.substring(2);

    const formattedRest = parseFloat('0.' + restOfNumber)
      .toFixed(5)
      .substring(2);

    return `${firstTwoDigits}.${formattedRest}`;
  }
}
