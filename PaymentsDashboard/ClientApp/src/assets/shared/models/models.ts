export enum SortBy {
  Date, Amount
}

export class Payment {
  paymentId: string;
  title: string;
  amount: number;
  date: string;
  tags: Tag[];
}

export class Tag {
  id: string;
  title: string;
  hexColorCode: string;
  payments: Payment[];
}

export class PaymentsPerDateModel {
  date: string;
  payments: Payment[];
}
