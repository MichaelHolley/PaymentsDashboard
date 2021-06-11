import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { faEdit, faPlusCircle, faTrash, faUndoAlt } from '@fortawesome/free-solid-svg-icons';
import { BsModalService } from 'ngx-bootstrap/modal';
import { ConfirmDialogComponent } from '../../assets/shared/dialogs/confirm-dialog.component';
import { Tag, TagType } from '../../assets/shared/models/models';
import { TagService } from '../../assets/shared/services/tag.service';

@Component({
  selector: 'app-tags',
  templateUrl: './tags.component.html',
})
export class TagsComponent implements OnInit {
  faPlusCircle = faPlusCircle;
  faTrash = faTrash;
  faEdit = faEdit;
  faUndoAlt = faUndoAlt;

  showForm = false;
  tagForm: FormGroup;
  tagTypes = [];
  TagType = TagType;

  displayTagType = 0;
  displayedTags: Tag[];

  resetFromJSON = {
    tagId: undefined,
    title: "",
    hexColorCode: "#ffffff",
    type: 0
  }

  constructor(private tagService: TagService,
    private formBuilder: FormBuilder,
    private modalService: BsModalService
  ) { }

  ngOnInit() {
    for (let enumMember in TagType) {
      if (!isNaN(parseInt(enumMember, 10))) {
        this.tagTypes.push({ key: parseInt(enumMember), value: TagType[enumMember] });
      } 
    }

    this.tagForm = this.formBuilder.group({
      tagId: [undefined],
      title: ["", Validators.required],
      hexColorCode: ["#ffffff"],
      type: [0, Validators.required]
    });

    this.getTags();
  }

  getTags() {
    this.tagService.getAllTags().subscribe(result => {
      this.displayedTags = result.filter(t => t.type === this.displayTagType);
    });
  }

  resetForm() {
    this.tagForm.patchValue(this.resetFromJSON);
  }

  editTag(tag: Tag) {
    this.showForm = true;

    window.scroll(0, 0);

    this.tagForm.patchValue(tag);
  }

  addButtonAction() {
    this.showForm = !this.showForm;
    this.resetForm();
  }

  onSubmit() {
    if (this.tagForm.invalid) {
      return;
    }

    let postTag = this.tagForm.value as Tag;

    this.tagService.createOrUpdateTag(postTag).subscribe(result => {
      this.resetForm();
      this.showForm = false;
      this.getTags();
    });
  }

  deleteTag(tag: Tag) {
    let modalRef = this.openDeleteConfirmDialog();
    modalRef.content.onClose.subscribe(confirmed => {
      if (confirmed) {
        this.tagService.deleteTag(tag.tagId).subscribe(result => this.getTags());
      }
    });
  }

  displayTagTypeChange(value) {
    this.getTags();
  }

  openDeleteConfirmDialog() {
    const initialState = {
      title: 'Delete Tag',
      content: 'Do you want to delete this tag?'
    };
    return this.modalService.show(ConfirmDialogComponent, { initialState });
  }
}
