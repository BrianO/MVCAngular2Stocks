import { Injectable } from '@angular/core';
import { Http, Response, URLSearchParams, RequestOptions, Headers } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import 'rxjs/Rx';
import { stock } from '../components/stock';
import { rssitem } from '../components/rssitem';
import { quote } from '../components/quote';

@Injectable()
export class AppServiceStocks {

    private _getStocksListUrl = 'Stocks/StocksJSON';
    private _getStockDetailUrl = "Stocks/ReadStockQuote?Id=";
    private _getPriceUrl = "Stocks/ReadPrice/";

    private _getNewsUrl = "http://feeds.finance.yahoo.com/rss/2.0/headline?s=";

    private _deleteStockUrl = "Stocks/Remove";
    private _addStockUrl = "Stocks/AddJSON";

    private _stockslist: stock[];

    constructor(private http: Http) {
    }

    stockslist(): Observable<stock[]> {
        return this.http.get(this._getStocksListUrl)
            .map(this.extractData)
            .catch(this.handleError);
    }

    stockDetail(symbol : string): Observable<quote> {
        return this.http.get(this._getStockDetailUrl + symbol)
            .map(this.extractData)
            .catch(this.handleError);
    }
    
    readPrice(symbol: string): Observable<string> {
        return this.http.get(this._getPriceUrl + symbol)
            .map(this.extractData)
            .catch(this.handleError);
    }

    readNews(symbol: string): Observable<rssitem[]> {
        var newsLink = this._getNewsUrl
            + symbol
            + "&region=US&lang=en-US";

        let params: URLSearchParams = new URLSearchParams();
        params.set('Link', newsLink);
    
        return this.http.get("/Stocks/ReadNewsData?",
            { search: params })
            .map(this.extractData)
            .catch(this.handleError);
    }

    remove(s : stock) : Observable<string> {
        let headers = new Headers({ 'Content-Type': 'application/json' });
        let options = new RequestOptions({ headers: headers });

        return this.http.post(this._deleteStockUrl, { item : s }, options)
            .map(this.extractData)
            .catch(this.handleError);
    }

    add(s: string): Observable<string> {
        let headers = new Headers({ 'Content-Type': 'application/json' });
        let options = new RequestOptions({ headers: headers });

        return this.http.post(this._addStockUrl, { Symbol : s }, options)
            .map(this.extractData)
            .catch(this.handleError);
    }

    private handleError(error: Response | any) {
        // In a real world app, we might use a remote logging infrastructure
        let errMsg: string;
        if (error instanceof Response) {
            const body = error.json() || '';
            const err = body.error || JSON.stringify(body);
            errMsg = `${error.status} - ${error.statusText || ''} ${err}`;
        } else {
            errMsg = error.message ? error.message : error.toString();
        }
        console.error(errMsg);
        return Observable.throw(errMsg);
    }

    private extractData(res: Response) {
        let body = res.json();
        return body || {};
    }


}
