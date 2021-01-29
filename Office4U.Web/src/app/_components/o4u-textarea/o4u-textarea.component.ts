import { Component, Input, Self } from '@angular/core';
import { ControlValueAccessor, NgControl } from '@angular/forms';

@Component({
  selector: 'o4u-textarea',
  templateUrl: './o4u-textarea.component.html',
  styleUrls: ['./o4u-textarea.component.css']
})
export class O4uTextareaComponent implements ControlValueAccessor {
  @Input() label: string;
  @Input() rows: number;
  @Input() maxLength: number;

  constructor(@Self() public ngControl: NgControl) {
    this.ngControl.valueAccessor = this;
  }

  writeValue(obj: any): void { }

  registerOnChange(fn: any): void { }

  registerOnTouched(fn: any): void { }

  ngOnInit(): void {
  }

}
