
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable()
export class ApiService {

  gatewayBaseUrl: string;

  constructor(
    private http: HttpClient,
  ) {
    this.gatewayBaseUrl = "http://localhost:54176/transactions";
  }

  getMerchantTransactions(merchantId: any) {
    return this.http.get(this.gatewayBaseUrl + "/GetTransactionsByMerchantID/" + merchantId);
  }
}
