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

export class ReoccuringPayment extends CreatedBase {
  id: string;
  title: string;
  amount: number;
  startDate: string;
  endDate: string;
  reoccuringType: ReoccuringType;
  tags: Tag[];
}

export enum ReoccuringType {
  Hourly = 0,
  Daily = 1,
  Weekly = 2,
  Monthly = 3,
  Yearly = 4
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
