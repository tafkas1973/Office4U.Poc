import { AfterViewInit, Component, Input, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'o4u-table',
  templateUrl: './o4u-table.component.html',
  styleUrls: ['./o4u-table.component.css']
})
export class O4uTableComponent implements OnInit {
  @Input() dataRows: Array<any> = [];
  @Input() doubleClickUrl: string;
  @Input() columnTitles: Array<string> = [];
  @Input() rowCellPropertyNames: Array<string> = [];
  rowObjectKeys: Array<string> = [];

  constructor(private router: Router) { }

  ngOnInit(): void { }

  onRowClick(id: number) {
    this.router.navigateByUrl(this.doubleClickUrl + id.toString());
  }
}
