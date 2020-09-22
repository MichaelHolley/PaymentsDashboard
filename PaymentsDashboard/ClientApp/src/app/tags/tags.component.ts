import { Component, OnInit } from '@angular/core';
import { Tag } from '../../assets/shared/models/models';
import { TagService } from '../../assets/shared/services/tag.service';

@Component({
  selector: 'app-tags',
  templateUrl: './tags.component.html',
})
export class TagsComponent implements OnInit {

  showTagsComponent = false;

  existingTags: Tag[];

  constructor(private tagsService: TagService) {
    this.tagsService.getAllTags().subscribe(result => {
      this.existingTags = result;
    });
  }

  ngOnInit() {
  }

  tagsButtonAction() {
    this.showTagsComponent = !this.showTagsComponent;
  }

}
