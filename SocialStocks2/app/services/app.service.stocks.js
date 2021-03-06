"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
Object.defineProperty(exports, "__esModule", { value: true });
var core_1 = require("@angular/core");
var http_1 = require("@angular/http");
var Observable_1 = require("rxjs/Observable");
require("rxjs/Rx");
var AppServiceStocks = /** @class */ (function () {
    function AppServiceStocks(http) {
        this.http = http;
        this._getStocksListUrl = 'Stocks/StocksJSON';
        this._getStockDetailUrl = "Stocks/ReadStockQuote?Id=";
        this._getPriceUrl = "Stocks/ReadPrice?Id=";
        this._getNameUrl = "Stocks/GetNameFromSymbol?Id=";
        this._getNewsUrl = "http://feeds.finance.yahoo.com/rss/2.0/headline?s=";
        this._getTweetsUrl = "Stocks/GetTweets?Symbol=";
        this._deleteStockUrl = "Stocks/Remove";
        this._addStockUrl = "Stocks/AddJSON";
    }
    AppServiceStocks.prototype.stockslist = function () {
        return this.http.get(this._getStocksListUrl)
            .map(this.extractData)
            .catch(this.handleError);
    };
    AppServiceStocks.prototype.stockTweets = function (symbol) {
        return this.http.get(this._getTweetsUrl + symbol)
            .map(this.extractData)
            .catch(this.handleError);
    };
    AppServiceStocks.prototype.stockDetail = function (symbol) {
        return this.http.get(this._getStockDetailUrl + symbol)
            .map(this.extractData)
            .catch(this.handleError);
    };
    AppServiceStocks.prototype.getNameFromSymbol = function (symbol) {
        return this.http.get(this._getNameUrl + symbol)
            .map(this.extractData)
            .catch(this.handleError);
    };
    AppServiceStocks.prototype.readPrice = function (symbol) {
        return this.http.get(this._getPriceUrl + symbol)
            .map(this.extractData)
            .catch(this.handleError);
    };
    AppServiceStocks.prototype.readNews = function (symbol) {
        var newsLink = this._getNewsUrl
            + symbol
            + "&region=US&lang=en-US";
        var params = new http_1.URLSearchParams();
        params.set('Link', newsLink);
        return this.http.get("Stocks/ReadNewsData?", { search: params })
            .map(this.extractData)
            .catch(this.handleError);
    };
    AppServiceStocks.prototype.readYahooNews = function (symbol) {
        var params = new http_1.URLSearchParams();
        params.set('Symbol', symbol);
        return this.http.get("Stocks/ReadYahooNewsData", { search: params })
            .map(this.extractData)
            .catch(this.handleError);
    };
    AppServiceStocks.prototype.remove = function (s) {
        var headers = new http_1.Headers({ 'Content-Type': 'application/json' });
        var options = new http_1.RequestOptions({ headers: headers });
        return this.http.post(this._deleteStockUrl, { item: s }, options)
            .map(this.extractData)
            .catch(this.handleError);
    };
    AppServiceStocks.prototype.add = function (s) {
        var headers = new http_1.Headers({ 'Content-Type': 'application/json' });
        var options = new http_1.RequestOptions({ headers: headers });
        return this.http.post(this._addStockUrl, { Symbol: s }, options)
            .map(this.extractData)
            .catch(this.handleError);
    };
    AppServiceStocks.prototype.handleError = function (error) {
        // In a real world app, we might use a remote logging infrastructure
        var errMsg;
        if (error instanceof http_1.Response) {
            var body = error.json() || '';
            var err = body.error || JSON.stringify(body);
            errMsg = error.status + " - " + (error.statusText || '') + " " + err;
        }
        else {
            errMsg = error.message ? error.message : error.toString();
        }
        console.error(errMsg);
        return Observable_1.Observable.throw(errMsg);
    };
    AppServiceStocks.prototype.extractData = function (res) {
        var body = res.json();
        return body || {};
    };
    AppServiceStocks = __decorate([
        core_1.Injectable(),
        __metadata("design:paramtypes", [http_1.Http])
    ], AppServiceStocks);
    return AppServiceStocks;
}());
exports.AppServiceStocks = AppServiceStocks;
//# sourceMappingURL=app.service.stocks.js.map