import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { faEdit, faPlusCircle, faTrash, faUndoAlt } from '@fortawesome/free-solid-svg-icons';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { ConfirmDialogComponent } from '../../assets/shared/dialogs/confirm-dialog.component';
import { ReoccuringPayment, ReoccuringType, Tag, TagType } from '../../assets/shared/models/models';
import { DateTimeHelperService } from '../../assets/shared/services/datetimehelper.service';
import { ReoccuringPaymentService } from '../../assets/shared/services/reoccuringpayment.service';
import { TagService } from '../../assets/shared/services/tag.service';

@Component({
  selector: 'app-reoccuring-payments',
  templateUrl: './reoccuring-payments.component.html'
})
export class ReoccuringPaymentsComponent implements OnInit {

  faPlusCircle = faPlusCircle;
  faTrash = faTrash;
  faEdit = faEdit;
  faUndoAlt = faUndoAlt;

  bsModalRef: BsModalRef;

  primaryTags: Tag[];
  secondaryTags: Tag[];

  showForm = false;
  reoccuringPaymentForm: FormGroup;
  reoccuringTypes = [];

  displayReoccuringStatus = 0;
  displayedReoccuringPayments: ReoccuringPayment[];

  resetFromJSON = {
    id: undefined,
    title: '',
    amount: 0,
    startDate: this.dateTimeHelperService.dateToString(new Date),
    endDate: undefined,
    reoccuringType: undefined,
    primaryTag: undefined,
    secondaryTags: []
  }

  constructor(
    private formBuilder: FormBuilder,
    private modalService: BsModalService,
    private reoccuringPaymentsService: ReoccuringPaymentService,
    private tagsService: TagService,
    private dateTimeHelperService: DateTimeHelperService) { }

  ngOnInit() {
    for (let enumMember in ReoccuringType) {
      if (!isNaN(parseInt(enumMember, 10))) {
        this.reoccuringTypes.push({ key: parseInt(enumMember), value: ReoccuringType[enumMember] });
      }
    }

    this.reoccuringPaymentForm = this.formBuilder.group({
      id: [undefined],
      title: [""],
      amount: [0, [Validators.required, Validators.min(0.01)]],
      startDate: ["", Validators.required],
      endDate: [""],
      reoccuringType: [undefined, Validators.required],
      primaryTag: [undefined, Validators.required],
      secondaryTags: [[]]
    });

    this.getTags();
    this.getReoccuringPayments();
  }

  getReoccuringPayments() {
    this.reoccuringPaymentsService.getAllReoccuringPayments().subscribe(result => {
      this.displayedReoccuringPayments = result;
    });
  }

  resetForm() {
    this.reoccuringPaymentForm.patchValue(this.resetFromJSON);
  }

  editReoccuringPayment(payment: ReoccuringPayment) {
    this.showForm = true;
    this.reoccuringPaymentForm.patchValue(payment);
    this.reoccuringPaymentForm.patchValue({
      primaryTag: payment.tags.find(t => t.type === TagType.Primary) ? this.primaryTags.find(pT => pT.tagId === payment.tags.find(t => t.type === TagType.Primary).tagId) : undefined,
      secondaryTags: this.secondaryTags.filter(sT => payment.tags.some(t => t.tagId === sT.tagId))
    });
    window.scroll(0, 0);
  }

  addButtonAction() {
    this.showForm = !this.showForm;
    this.resetForm();
  }

  getTags() {
    this.tagsService.getAllTags().subscribe(result => {
      this.primaryTags = result.filter(t => t.type === TagType.Primary);
      this.secondaryTags = result.filter(t => t.type === TagType.Secondary);
      this.primaryTags.forEach(t => t.payments = null);
      this.secondaryTags.forEach(t => t.payments = null);
    });
  }

  onSubmit() {
    if (this.reoccuringPaymentForm.invalid) {
      return;
    }

    let postPayment = this.reoccuringPaymentForm.value as ReoccuringPayment;
    postPayment.tags = [];
    postPayment.tags.push(this.reoccuringPaymentForm.value.primaryTag);
    postPayment.tags = postPayment.tags.concat(this.reoccuringPaymentForm.value.secondaryTags);

    this.reoccuringPaymentsService.createOrUpdateReoccuringPayment(postPayment).subscribe(result => {
      this.resetForm();
      this.showForm = false;
      this.getReoccuringPayments();
    });
  }

  deleteReoccuringPayment(payment: ReoccuringPayment) {
    let modalRef = this.openDeleteConfirmDialog();
    modalRef.content.onClose.subscribe(confirmed => {
      if (confirmed) {
        this.reoccuringPaymentsService.deleteReoccuringPayment(payment.id).subscribe(result => this.getReoccuringPayments());
      }
    });
  }

  openDeleteConfirmDialog() {
    const initialState = {
      title: 'Delete Payment',
      content: 'Do you want to delete this payment?'
    };
    return this.modalService.show(ConfirmDialogComponent, { initialState });
  }

  sortTags(tags: Tag[]) {
    return tags.sort((a, b) => a.type - b.type);
  }

}
