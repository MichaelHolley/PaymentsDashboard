export enum SortBy {
  Date, Amount
}

export enum TagType {
  Primary = 0,
  Secondary = 1
}

export class CreatedBase {
  created: Date;
}

export class Payment extends CreatedBase {
  paymentId: string;
  title: string;
  amount: number;
  date: string;
  tags: Tag[];
}

export class Tag extends CreatedBase {
  tagId: string;
  title: string;
  hexColorCode: string;
  type: TagType;
  payments: Payment[];
}

export class PaymentsPerDateModel {
  date: string;
  payments: Payment[];
}
