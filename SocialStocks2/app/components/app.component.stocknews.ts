import { Component } from '@angular/core';
import { Router, ActivatedRoute, Params } from '@angular/router';

import { stock } from './stock';
import { quote } from './quote';
import { rssitem } from './rssItem';
import { Observable } from 'rxjs/Observable';
import { AppServiceStocks } from '../services/app.service.stocks';


@Component({
    templateUrl: './app/components/app.component.news.html?v=3',
    providers: [AppServiceStocks]
})
export class NewsComponent {

    mode = 'Observable';
    stockSymbol: string;
    stockName: string;
    statusMessage: string;
    newsItems: rssitem[];

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
            (params: Params) => this._appService.readYahooNews(params["id"]))
            .subscribe(r => {
                this.newsItems = r;

                this.newsItems.forEach(function (item) {
                    item.isVisible = false;
                    item.expandChar = "+";
                });

            });
    }

    toggle(item: rssitem) {
        item.isVisible = !item.isVisible;
        if (item.isVisible)
            item.expandChar = "-";
        else
            item.expandChar = "+";
    };


    

}

