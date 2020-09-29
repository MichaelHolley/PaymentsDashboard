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

export class PaymentPostModel {
  paymentId: string;
  title: string;
  amount: number;
  date: string;
  tagIds: string[];
}

export class Tag {
  id: string;
  title: string;
  hexColorCode: string;
}
