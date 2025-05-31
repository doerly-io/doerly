import {CurrencyPipe} from '@angular/common';
import {inject, Pipe, PipeTransform} from '@angular/core';
import {ECurrency} from 'app/modules/payments/enums/e-currency';

@Pipe({
  name: 'paymentCurrency'
})
export class PaymentCurrencyPipe implements PipeTransform {

  private readonly currencyPipe: CurrencyPipe = inject(CurrencyPipe);

  transform(value: number, currency: ECurrency = ECurrency.UAH, ...args: any[]): string | null {

    let currencyCode: string;
    switch (currency) {
      case ECurrency.UAH:
      default:
        currencyCode = 'UAH';
        break;
      case ECurrency.USD:
        currencyCode = 'USD';
        break;
      case ECurrency.EUR:
        currencyCode = 'EUR';
        break;
    }

    return this.currencyPipe.transform(value, currencyCode, 'symbol-narrow', '1.2-2');
  }

}
