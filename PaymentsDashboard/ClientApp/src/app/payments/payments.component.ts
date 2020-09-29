import { Component, OnInit, Input } from '@angular/core';
import { Tag, Payment, SortBy, PaymentPostModel } from '../../assets/shared/models/models';
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

  displayedPayments: { date: string, payments: Payment[] }[];

  showForm = false;
  paymentForm: FormGroup;

  resetFromJSON = {
    id: undefined,
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

    this.tagsService.getAllTags().subscribe(result => { this.availableTags = result; });

    this.getPayments();
  }

  getPayments() {
    this.displayedPayments = [];

    this.paymentService.getAllPayments().subscribe(result => {
      result.forEach(r => {
        let p: Payment = {
          paymentId: r.paymentId,
          title: r.title,
          amount: r.amount,
          date: r.date,
          tags: r.tags
        };

        if (this.displayedPayments.length == 0 || new Date(this.displayedPayments[this.displayedPayments.length - 1].date).getTime() != new Date(p.date).getTime()) {
          this.displayedPayments.push({ date: p.date, payments: [] });
        }
        this.displayedPayments[this.displayedPayments.length - 1].payments.push(p);
        this.displayedPayments[this.displayedPayments.length - 1].payments.sort((a: Payment, b: Payment) => { return b.amount - a.amount; })
      });

      result.forEach(payment => {
        if (payment.tags) {
          payment.tags.forEach(tag => {
            this.addToUsedTags(tag);
          });
        }
      });

      console.log(this.displayedPayments);
    });
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
    // TODO CHECK IF VALID
    let postPayment: PaymentPostModel = new PaymentPostModel();
    postPayment.paymentId = this.paymentForm.value.id;
    postPayment.title = this.paymentForm.value.title;
    postPayment.amount = this.paymentForm.value.amount;
    postPayment.date = this.paymentForm.value.date;
    postPayment.tagIds = [];
    this.paymentForm.value.tags.forEach(tag => { postPayment.tagIds.push(tag.tagId) });

    this.paymentService.createOrUpdatePayment(postPayment).subscribe(result => {
      //TODO add result to payments-list of parent
      this.resetForm();
      this.getPayments();
    });
  }

  editPayment(payment: Payment) {
    this.showForm = true;

    console.log(payment)

    this.paymentForm.patchValue({
      paymentId: payment.paymentId,
      title: payment.title,
      amount: payment.amount,
      date: payment.date,
      tags: payment.tags
    });

    console.log(this.paymentForm.value)
  }

  paymentAddButtonAction() {
    this.showForm = !this.showForm;
    this.resetForm();
    this.tagsService.getAllTags().subscribe(result => { this.availableTags = result });
  }

  dateToString(date: Date) {
    return (date.getFullYear() + '-' + ((date.getMonth() > 8) ? (date.getMonth() + 1) : ('0' + (date.getMonth() + 1))) + '-' + ((date.getDate() > 9) ? date.getDate() : ('0' + date.getDate())));
  }
}
