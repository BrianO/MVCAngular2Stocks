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
var AppComponent = (function () {
    function AppComponent(_appService) {
        this._appService = _appService;
        this.name = 'Angular Stocks';
        this.mode = 'Observable';
        this.statusMessage = "";
        this.newStockName = "";
        this.newStockSym = "";
        this.newStock = new stock_1.stock();
        this.rownum = 0;
        this.interval = 2;
        // this.getStocks();
    }
    AppComponent.prototype.ngOnInit = function () {
        this.newStockName = "";
        this.getStocks();
    };
    AppComponent.prototype.getStocks = function () {
        var _this = this;
        this._appService.stockslist()
            .subscribe(function (stocks) { return _this.stockslist = stocks; });
    };
    AppComponent.prototype.addStockToList = function (s) {
        var newListItem = new stock_1.stock();
        newListItem.Symbol = s.Symbol;
        this.stockslist.push(newListItem);
    };
    AppComponent.prototype.symbolChanged = function () {
        var _this = this;
        this._appService
            .stockDetail(this.newStockSym)
            .subscribe(function (result) {
            _this.newStockName = result.Name;
            _this.newStock.Symbol = _this.newStockSym;
        });
    };
    AppComponent.prototype.addStock = function () {
        var _this = this;
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
            .subscribe(function (result) {
            _this.addStockToList(_this.newStock);
        });
    };
    return AppComponent;
}());
AppComponent = __decorate([
    core_1.Component({
        selector: 'stocks',
        templateUrl: './app/components/app.component.stocks.html',
        providers: [app_service_stocks_1.AppServiceStocks]
    }),
    __metadata("design:paramtypes", [app_service_stocks_1.AppServiceStocks])
], AppComponent);
exports.AppComponent = AppComponent;
//# sourceMappingURL=app.component.stocks.js.map