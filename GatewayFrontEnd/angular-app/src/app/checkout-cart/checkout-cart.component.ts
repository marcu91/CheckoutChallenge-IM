import { HttpClient } from '@angular/common/http';
import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { ApiService } from '../api.service';

@Component({
  selector: 'app-checkout-cart',
  templateUrl: './checkout-cart.component.html',
  styleUrls: ['./checkout-cart.component.css'],
  encapsulation: ViewEncapsulation.None
})
export class CheckoutCartComponent implements OnInit {

  basicUrl = "http://localhost:54176/transactions/process";

  constructor(
    public api: ApiService,
    private http: HttpClient,
  ) {

  }

  transactionData = {
    currency: "EUR",
    amount: 100.00,
    card:
    {
      cardNumber: "5170450230011041",
      holderName: "Marcu Iulian",
      expiryMonth: "05",
      expiryYear: 2022,
      cvv: "965"
    },
    merchantID: "B3E7C684-99A2-4253-898B-01515B92F1B1",
    bank: "MockBank"
  };

  ngOnInit(): void {
  }

  sendShoppingCartDetails() {

    console.log(this.transactionData);

    this.http
      .post(this.basicUrl, this.transactionData)
      .subscribe((data: any) => { console.log(data) },
        error => { console.log(error) });
  }

  onCurrencySelection(e: any) {
    let target = e.source.selected._element.nativeElement;
    let selectedData = {
      value: e.value,
      text: target.innerText.trim()
    };
    this.transactionData.currency = selectedData.value;
    console.log(selectedData);
  }
}
