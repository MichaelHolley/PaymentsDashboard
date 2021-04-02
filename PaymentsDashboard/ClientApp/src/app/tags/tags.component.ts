import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { faEdit, faPlusCircle, faTrash, faUndoAlt } from '@fortawesome/free-solid-svg-icons';
import { Tag } from '../../assets/shared/models/models';
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

  existingTags: Tag[];

  resetFromJSON = {
    tagId: undefined,
    title: "",
    hexColorCode: "#ffffff"
  }

  constructor(private tagService: TagService,
    private formBuilder: FormBuilder) {
  }

  ngOnInit() {
    this.tagForm = this.formBuilder.group({
      tagId: [undefined],
      title: ["", Validators.required],
      hexColorCode: ["#ffffff"]
    });

    this.getTags();
  }

  getTags() {
    this.tagService.getAllTags().subscribe(result => {
      this.existingTags = result;
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
    this.tagService.deleteTag(tag.tagId).subscribe(result => this.getTags());
  }

}
