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
export class AppComponent {
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
        // this.getStocks();
    }

    ngOnInit() {
        this.newStockName = "";
        this.getStocks();
    }

    getStocks() {
        this._appService.stockslist()
            .subscribe(
            stocks => this.stockslist = stocks);
    }

    private addStockToList(s: stock) {
        var newListItem = new stock();
        newListItem.Symbol = s.Symbol;
        this.stockslist.push(newListItem);
    }

    symbolChanged() {
        this._appService
            .stockDetail(this.newStockSym)
            .subscribe(result => {
                this.newStockName = result.Name;
                this.newStock.Symbol = this.newStockSym;
            });
    }


    addStock() {
        var stockExists = false;

        this.stockslist.forEach(function (s) {
            if (s.Symbol == this.newStockSym) {
                stockExists = true;
            }
        }, this);

        if (stockExists) {
            return;
        }

        this._appService
            .add(this.newStockSym)
            .subscribe(result => {
                this.addStockToList(this.newStock);
            });
    }
}
