
//We are intentionally lax with this interface types, for now
export interface Transaction {
  transactionID: any;
  amount: any;
  currency: any;
  bank: any;
  isChecked: boolean;
}
