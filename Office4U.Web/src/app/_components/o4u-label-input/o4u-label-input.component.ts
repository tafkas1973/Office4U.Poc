import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';

@Component({
  selector: 'o4u-label-input',
  templateUrl: './o4u-label-input.component.html',
  styleUrls: ['./o4u-label-input.component.css']
})
export class O4uLabelInputComponent implements OnInit {
  @Input() labelText: string;
  @Input() inputControl: string;
  @Output() inputControlChange = new EventEmitter();

  constructor() { }

  ngOnInit(): void {
  }

  onInputControlChange(event: string) {
    this.inputControlChange.emit(event);
  }

}
