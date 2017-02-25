import { Component } from '@angular/core';
import { Router } from '@angular/router';

import { stock } from './stock';
import { quote } from './quote';
import { Observable } from 'rxjs/Observable';
import { AppServiceStocks } from '../services/app.service.stocks';


@Component({
    templateUrl: './app/components/app.component.stockadd.html?v=2',
    providers: [AppServiceStocks]
})
export class AddComponent {
    name = 'Stock Add';
    mode = 'Observable';
    statusMessage = "";
    stockslist: stock[];
    newStockName = "";
    newStockSym = "";
    newStock = new stock();


    constructor(private _appService: AppServiceStocks,
                private router: Router) {
    }

    ngOnInit() {
        this.newStockName = "";
        this.getStocks();
    }

    getStocks() {
        this._appService.stockslist()
            .subscribe(
            stocks => {
                this.stockslist = stocks;
            });
    }


  //  private addStockToList(s: stock) {
  //      var newListItem = new stock();
  //      newListItem.Symbol = s.Symbol;
  //      // this.stockslist.push(newListItem);
  //  }

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
                // this.addStockToList(this.newStock);

                this.router.navigate([ '/' ]);
            });
    }

}
