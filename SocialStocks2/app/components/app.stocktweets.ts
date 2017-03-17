import { Component } from '@angular/core';
import { Router, ActivatedRoute, Params } from '@angular/router';

import { tweetUser } from './tweetUser';
import { tweet } from './tweet';
import { stock } from './stock';
import { quote } from './quote';
import { rssitem } from './rssItem';
import { Observable } from 'rxjs/Observable';
import { AppServiceStocks } from '../services/app.service.stocks';

@Component({
    templateUrl: './app/components/app.stocktweets.html',
    providers: [AppServiceStocks]
})


export class TweetComponent {
    mode = 'Observable';
    tweetStock: stock;
    tweets: tweet[];

    stockSymbol: string;
    stockName: string;
    private sub: any;      // -> Subscriber

    constructor(
        private route: ActivatedRoute,
        private _appService: AppServiceStocks,
        private router: Router) {
    }

    ngOnInit() {
    
        this.sub = this.route
            .params
            .subscribe(params => {
                this.stockSymbol = params['id'];
            });

        this.route
            .params
            .switchMap(
            (params: Params) => this._appService.stockTweets(params["id"]))
            .subscribe(r => { this.tweets = r; });
           

    }

}


