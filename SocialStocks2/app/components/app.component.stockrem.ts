import { Component } from '@angular/core';
import { Router, ActivatedRoute, Params } from '@angular/router';

import { stock } from './stock';
import { quote } from './quote';
import { Observable } from 'rxjs/Observable';
import { AppServiceStocks } from '../services/app.service.stocks';


@Component({
    templateUrl: './app/components/app.component.stockrem.html?v=21',
    providers: [AppServiceStocks]
})
export class RemComponent {
    mode = 'Observable';
    removeStock: stock;
    stockSymbol: string;
    stockName: string;
    private sub: any;      // -> Subscriber

    constructor(
        private route: ActivatedRoute,
        private _appService: AppServiceStocks,
        private router: Router) {
    }

    ngOnInit() {

        this.route
            .params
            .switchMap(
            (params: Params) => this._appService.getNameFromSymbol(params["id"]))
            .subscribe(r => { this.stockName = r; });

       this.sub = this.route
           .params
           .subscribe(params => {         
               this.stockSymbol = params['id'];
           });

    }
    
    remove() {
        
        this.removeStock.Symbol = this.stockSymbol;

        this._appService
            .remove(this.removeStock)
            .subscribe(result => {
                this.router.navigate(['/']);
            });
    }

}

