import { Component } from '@angular/core';
// import { NgControl } from '@angular/common';
import { stock } from './stock';
import { Observable } from 'rxjs/Observable';
import { AppServiceStocks } from '../services/app.service.stocks';


@Component({
  selector: 'stocks',
  templateUrl: './app/components/app.component.stocks.html', 
  providers: [AppServiceStocks]
})
export class AppComponent  {
    name = 'Angular Stocks';
    stockslist: stock[];
    mode = 'Observable';

    constructor(private _appService: AppServiceStocks) {
       // this.getStocks();
    }

    ngOnInit() { this.getStocks(); }

    // get stockslist(): stock[] {
    //    return this._stockslist;
    // }

    getStocks() {
        this._appService.stockslist()
            .subscribe(
            stocks => this.stockslist = stocks);
    }
}
