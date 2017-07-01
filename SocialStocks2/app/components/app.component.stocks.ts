import { Component } from '@angular/core';
import { stock } from './stock';
import { quote } from './quote';
import { Observable } from 'rxjs/Observable';
import { AppServiceStocks } from '../services/app.service.stocks';

@Component({
    selector: 'stocks',
    templateUrl: './app/components/app.component.stocks.html',
    providers: [AppServiceStocks]
})

export class AppStocks {

    name = 'Angular Stocks';
    stockslist: stock[];
    mode = 'Observable';
    statusMessage = "";
    newStockName = "";
    newStockSym = "";
    newStock = new stock();
    rownum = 0;
    interval = 2;
    
    constructor(private _appService: AppServiceStocks) {
        
    }

    ngOnInit() {
        this.newStockName = "";
        this.getStocks();
    }

    private readPrice() {
        this._appService.readPrice(this.stockslist[this.rownum].Symbol)
            .subscribe(result => {
                this.stockslist[this.rownum].Price = result.Price;
                this.stockslist[this.rownum].Color = result.Color;
                this.rownum++;
                if (this.rownum == this.stockslist.length) {
                    this.rownum = 0;
                }
                this.getNextQuote();
            });
    }

    private getNextQuote() {
      setTimeout(() => { this.readPrice() }, this.interval * 1000);
    }

    getStocks() {
        this._appService.stockslist()
            .subscribe(
            stocks =>
            {
                this.stockslist = stocks;
                this.getNextQuote();
            });
    }

    

}
