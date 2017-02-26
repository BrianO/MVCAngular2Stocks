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
var core_1 = require("@angular/core");
var stock_1 = require("./stock");
var app_service_stocks_1 = require("../services/app.service.stocks");
var AppStocks = (function () {
    function AppStocks(_appService) {
        this._appService = _appService;
        this.name = 'Angular Stocks';
        this.mode = 'Observable';
        this.statusMessage = "";
        this.newStockName = "";
        this.newStockSym = "";
        this.newStock = new stock_1.stock();
        this.rownum = 0;
        this.interval = 2;
    }
    AppStocks.prototype.ngOnInit = function () {
        this.newStockName = "";
        this.getStocks();
    };
    AppStocks.prototype.readPrice = function () {
        var _this = this;
        this._appService.readPrice(this.stockslist[this.rownum].Symbol)
            .subscribe(function (result) {
            _this.stockslist[_this.rownum].Price = result.Price;
            _this.stockslist[_this.rownum].Color = result.Color;
            _this.rownum++;
            if (_this.rownum == _this.stockslist.length) {
                _this.rownum = 0;
            }
            _this.getNextQuote();
        });
    };
    AppStocks.prototype.getNextQuote = function () {
        var _this = this;
        setTimeout(function () { _this.readPrice(); }, this.interval * 1000);
    };
    AppStocks.prototype.getStocks = function () {
        var _this = this;
        this._appService.stockslist()
            .subscribe(function (stocks) {
            _this.stockslist = stocks;
            _this.getNextQuote();
        });
    };
    return AppStocks;
}());
AppStocks = __decorate([
    core_1.Component({
        selector: 'stocks',
        templateUrl: './app/components/app.component.stocks.html?v=4',
        providers: [app_service_stocks_1.AppServiceStocks]
    }),
    __metadata("design:paramtypes", [app_service_stocks_1.AppServiceStocks])
], AppStocks);
exports.AppStocks = AppStocks;
//# sourceMappingURL=app.component.stocks.js.map