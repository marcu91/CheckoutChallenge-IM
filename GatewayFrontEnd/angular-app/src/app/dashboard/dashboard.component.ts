import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { ApiService } from '../api.service';
import { Transaction } from '../interfaces/transaction.interface';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {

  displayedColumns: string[] = ["action", "transactionID", "amount", "currency", "bank",];
  dataSource = new MatTableDataSource<Transaction>();

  @ViewChild(MatSort) sort: MatSort;
  @ViewChild(MatPaginator) paginator: MatPaginator;

  pageEvent: PageEvent;

  length = 100;
  pageIndex = 0;
  pageSize = 10;
  pageSizeOptions = [1, 2, 5, 10];

  constructor(
    private service: ApiService
  ) { }

  ngOnInit(): void {
    var merchantString = localStorage.getItem('merchantId');

    console.log(merchantString);
    this.service.getMerchantTransactions(merchantString)
      .subscribe((data: any) => {
        data.forEach((e: any) => {
          e.isChecked = false;
        });
        console.log(data);
        this.dataSource.data = data;
      })
  }

  ngAfterViewInit() {
    this.dataSource.sort = this.sort;
    this.dataSource.paginator = this.paginator;
  }

  selectAll() {
    for (var x of this.dataSource.data) {
      x.isChecked = !x.isChecked;
    }
  }

}
