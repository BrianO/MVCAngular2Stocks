import { Component } from '@angular/core';
import { Router, ActivatedRoute, Params } from '@angular/router';

import { stock } from './stock';
import { quote } from './quote';
import { Observable } from 'rxjs/Observable';
import { AppServiceStocks } from '../services/app.service.stocks';


@Component({
    templateUrl: './app/components/app.stockdetail.html',
    providers: [AppServiceStocks]
})
export class DetailComponent {
    mode = 'Observable';
    stockDetail: quote;
    stockName: string;
    private sub: any;      // -> Subscriber
    statusMessage: string;

    constructor(
        private route: ActivatedRoute,
        private _appService: AppServiceStocks,
        private router: Router) {
    }

    ngOnInit() {

        this.route
            .params
            .switchMap(
            (params: Params) => this._appService.stockDetail(params["id"]))
            .subscribe(r => {
                this.stockDetail = r;
                this.stockName = r.Name;

                if (this.stockDetail.DividendYield.length == 0)
                    this.stockDetail.DividendYield = "n/a";

            },
            error => this.statusMessage = <any>error);

    }

    Return() {
        console.error(" ");  
    }

}

