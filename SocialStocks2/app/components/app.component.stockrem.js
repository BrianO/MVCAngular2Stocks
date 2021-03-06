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
var router_1 = require("@angular/router");
var stock_1 = require("./stock");
var app_service_stocks_1 = require("../services/app.service.stocks");
var RemComponent = /** @class */ (function () {
    function RemComponent(route, _appService, router) {
        this.route = route;
        this._appService = _appService;
        this.router = router;
        this.mode = 'Observable';
    }
    RemComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.route
            .params
            .switchMap(function (params) { return _this._appService.getNameFromSymbol(params["id"]); })
            .subscribe(function (r) { _this.stockName = r; });
        this.sub = this.route
            .params
            .subscribe(function (params) {
            _this.stockSymbol = params['id'];
        });
    };
    RemComponent.prototype.remove = function () {
        var _this = this;
        this.removeStock = new stock_1.stock();
        this.removeStock.Symbol = this.stockSymbol;
        this._appService
            .remove(this.removeStock)
            .subscribe(function (result) {
            _this.router.navigate(['/']);
        });
    };
    RemComponent = __decorate([
        core_1.Component({
            templateUrl: './app/components/app.component.stockrem.html?v=21',
            providers: [app_service_stocks_1.AppServiceStocks]
        }),
        __metadata("design:paramtypes", [router_1.ActivatedRoute,
            app_service_stocks_1.AppServiceStocks,
            router_1.Router])
    ], RemComponent);
    return RemComponent;
}());
exports.RemComponent = RemComponent;
//# sourceMappingURL=app.component.stockrem.js.map