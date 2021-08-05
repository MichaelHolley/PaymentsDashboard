import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'input-validation',
  templateUrl: './input-validation.component.html'
})
export class InputValidationComponent implements OnInit {

  @Input() invalid: boolean = false;
  @Input() invalidMessage: string;

  constructor() { }

  ngOnInit() { }
}
