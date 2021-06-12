import { Component, OnInit } from '@angular/core';
import { faCheck, faTimes } from '@fortawesome/free-solid-svg-icons';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { Subject } from 'rxjs';

@Component({
  selector: 'confirm-dialog',
  templateUrl: './confirm-dialog.component.html',
})
export class ConfirmDialogComponent implements OnInit {
  faTimes = faTimes;
  faCheck = faCheck;

  title: string;
  content: string;

  public onClose: Subject<boolean>;

  constructor(public bsModalRef: BsModalRef) { }

  ngOnInit() {
    this.onClose = new Subject();
  }

  confirm() {
    this.onClose.next(true);
    this.bsModalRef.hide();
  }

  cancel() {
    this.onClose.next(false);
    this.bsModalRef.hide();
  }
}
