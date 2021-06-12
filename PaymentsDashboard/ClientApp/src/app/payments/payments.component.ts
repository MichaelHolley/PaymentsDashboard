import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { faEdit, faPlusCircle, faTrash, faUndoAlt } from '@fortawesome/free-solid-svg-icons';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { ConfirmDialogComponent } from '../../assets/shared/dialogs/confirm-dialog.component';
import { Payment, PaymentsPerDateModel, Tag, TagType } from '../../assets/shared/models/models';
import { PaymentService } from '../../assets/shared/services/payment.service';
import { TagService } from '../../assets/shared/services/tag.service';

@Component({
  selector: 'app-payments',
  templateUrl: './payments.component.html'
})
export class PaymentsComponent implements OnInit {
  faPlusCircle = faPlusCircle;
  faTrash = faTrash;
  faEdit = faEdit;
  faUndoAlt = faUndoAlt;

  bsModalRef: BsModalRef;

  primaryTags: Tag[];
  secondaryTags: Tag[];
  usedTags: Tag[] = [];
  TagType = TagType;

  numberOfDisplayedMonths: number;
  displayedPayments: PaymentsPerDateModel[];

  showForm = false;
  paymentForm: FormGroup;

  resetFromJSON = {
    paymentId: undefined,
    title: '',
    amount: 0,
    date: this.dateToString(new Date),
    primaryTag: undefined,
    secondaryTags: []
  }

  constructor(private tagsService: TagService,
    private paymentService: PaymentService,
    private formBuilder: FormBuilder,
    private modalService: BsModalService
  ) { }

  ngOnInit() {
    this.paymentForm = this.formBuilder.group({
      paymentId: [undefined],
      title: [""],
      amount: [0, [Validators.required, Validators.min(0.01)]],
      date: ["", Validators.required],
      primaryTag: [undefined, Validators.required],
      secondaryTags: [[]]
    });

    this.getTags();

    this.numberOfDisplayedMonths = 0;
    this.displayedPayments = [];

    this.getPayments(this.numberOfDisplayedMonths, true);
  }

  getPayments(numberOfMonths: number, clearExisting: boolean = false) {
    if (clearExisting) {
      this.displayedPayments = [];
      for (let i = 0; i < numberOfMonths; i++) {
        this.getPaymentsByMonths(i);
      }
    }

    this.getPaymentsByMonths(numberOfMonths);
  }

  getPaymentsByMonths(numberOfMonths) {
    this.paymentService.getPaymentsByMonths(numberOfMonths).subscribe(result => {
      if (result == null || result.length == 0) {
        return;
      }

      result.forEach(r => {
        let p: Payment = r as Payment;

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
    this.tagsService.getAllTags().subscribe(result => {
      this.primaryTags = result.filter(t => t.type === TagType.Primary);
      this.secondaryTags = result.filter(t => t.type === TagType.Secondary);
      this.primaryTags.forEach(t => t.payments = null);
      this.secondaryTags.forEach(t => t.payments = null);
    });
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
    postPayment.tags = [];
    postPayment.tags.push(this.paymentForm.value.primaryTag);
    postPayment.tags = postPayment.tags.concat(this.paymentForm.value.secondaryTags);

    this.paymentService.createOrUpdatePayment(postPayment).subscribe(result => {
      this.resetForm();
      this.showForm = false;
      this.getPayments(this.numberOfDisplayedMonths, true);
    });
  }

  editPayment(payment: Payment) {
    this.showForm = true;

    this.paymentForm.patchValue({
      paymentId: payment.paymentId,
      title: payment.title,
      amount: payment.amount,
      date: payment.date,
      primaryTag: payment.tags.find(t => t.type === TagType.Primary) ? this.primaryTags.find(pT => pT.tagId === payment.tags.find(t => t.type === TagType.Primary).tagId) : undefined,
      secondaryTags: this.secondaryTags.filter(sT => payment.tags.some(t => t.tagId === sT.tagId))
    });

    window.scroll(0, 0);
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
    let modalRef = this.openDeleteConfirmDialog();
    modalRef.content.onClose.subscribe(confirmed => {
      if (confirmed) {
        this.paymentService.deletePayment(payment.paymentId).subscribe(result => {
          this.getPayments(this.numberOfDisplayedMonths, true);
        });
      }
    });
  }

  sortTags(tags: Tag[]) {
    return tags.sort((a, b) => a.type - b.type);
  }

  openDeleteConfirmDialog() {
    const initialState = {
      title: 'Delete Payment',
      content: 'Do you want to delete this payment?'
    };
    return this.modalService.show(ConfirmDialogComponent, { initialState });
  }
}
