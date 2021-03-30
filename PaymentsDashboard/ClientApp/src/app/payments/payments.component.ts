import { Component, OnInit, Input } from '@angular/core';
import { Tag, Payment, SortBy, PaymentsPerDateModel } from '../../assets/shared/models/models';
import { PaymentService } from '../../assets/shared/services/payment.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { TagService } from '../../assets/shared/services/tag.service';

@Component({
  selector: 'app-payments',
  templateUrl: './payments.component.html'
})
export class PaymentsComponent implements OnInit {

  availableTags: Tag[];
  usedTags: Tag[] = [];

  numberOfDisplayedMonths: number;
  displayedPayments: PaymentsPerDateModel[];

  showForm = false;
  paymentForm: FormGroup;

  resetFromJSON = {
    paymentId: undefined,
    title: '',
    amount: 0,
    date: this.dateToString(new Date),
    tags: []
  }

  constructor(private tagsService: TagService,
    private paymentService: PaymentService,
    private formBuilder: FormBuilder
  ) { }

  ngOnInit() {
    this.paymentForm = this.formBuilder.group({
      paymentId: [undefined],
      title: [""],
      amount: [0, [Validators.required, Validators.min(0.01)]],
      date: ["", Validators.required],
      tags: [[]]
    });

    this.getTags();

    this.numberOfDisplayedMonths = 0;
    this.displayedPayments = [];

    this.getPayments(this.numberOfDisplayedMonths);
  }

  getPayments(numberOfMonths: number, clearExisting: boolean = false) {
    if (clearExisting) {
      this.displayedPayments = [];
    }

    this.paymentService.getPaymentsByMonths(numberOfMonths).subscribe(result => {
      result.forEach(r => {
        let p: Payment = {
          paymentId: r.paymentId,
          title: r.title,
          amount: r.amount,
          date: r.date,
          tags: r.tags
        };

        if (this.getIndexOfDate(p) == -1) {
          this.displayedPayments.push({ date: p.date, payments: [] });
          this.displayedPayments[this.displayedPayments.length - 1].payments.push(p);
        } else {
          this.displayedPayments[this.getIndexOfDate(p)].payments.push(p);
        }
      });

      this.displayedPayments.sort((a: PaymentsPerDateModel, b: PaymentsPerDateModel) => { return new Date(b.date).getTime() - new Date(a.date).getTime(); });

      this.displayedPayments[this.displayedPayments.length - 1].payments.sort((a: Payment, b: Payment) => { return b.amount - a.amount; })

      result.forEach(payment => {
        if (payment.tags) {
          payment.tags.forEach(tag => {
            this.addToUsedTags(tag);
          });
        }
      });
    });
  }

  getTags() {
    this.tagsService.getAllTags().subscribe(result => { this.availableTags = result; });
  }

  getIndexOfDate(payment: Payment) {
    let index = -1;
    for (let i = 0; i < this.displayedPayments.length; i++) {
      if (new Date(this.displayedPayments[i].date).getTime() == new Date(payment.date).getTime()) {
        return i;
      }
    }
    return index;
  }

  getPreviousMonth() {
    this.numberOfDisplayedMonths++;
    this.getPayments(this.numberOfDisplayedMonths);
  }

  addToUsedTags(tag: Tag) {
    if (!this.usedTags.includes(tag)) {
      this.usedTags.push(tag);
    }
  }

  resetForm() {
    this.paymentForm.patchValue(this.resetFromJSON);
  }

  onSubmit() {
    if (this.paymentForm.invalid) {
      return;
    }

    let postPayment = this.paymentForm.value as Payment;

    this.paymentService.createOrUpdatePayment(postPayment).subscribe(result => {
      this.resetForm();
      this.showForm = false;
      this.getPayments(this.numberOfDisplayedMonths, true);
    });
  }

  editPayment(payment: Payment) {
    this.showForm = true;

    window.scroll(0, 0);

    this.paymentForm.patchValue({
      paymentId: payment.paymentId,
      title: payment.title,
      amount: payment.amount,
      date: payment.date,
      tags: payment.tags
    });
  }

  addButtonAction() {
    this.showForm = !this.showForm;
    this.resetForm();
    this.getTags();
  }

  dateToString(date: Date) {
    return (date.getFullYear() + '-' + ((date.getMonth() > 8) ? (date.getMonth() + 1) : ('0' + (date.getMonth() + 1))) + '-' + ((date.getDate() > 9) ? date.getDate() : ('0' + date.getDate())));
  }

  deletePayment(payment: Payment) {
    this.paymentService.deletePayment(payment.paymentId).subscribe(result => {
      this.getPayments(this.numberOfDisplayedMonths, true);
    });
  }
}
