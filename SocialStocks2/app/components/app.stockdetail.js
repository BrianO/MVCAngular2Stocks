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
var router_1 = require("@angular/router");
var app_service_stocks_1 = require("../services/app.service.stocks");
var DetailComponent = (function () {
    function DetailComponent(route, _appService, router) {
        this.route = route;
        this._appService = _appService;
        this.router = router;
        this.mode = 'Observable';
    }
    DetailComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.route
            .params
            .switchMap(function (params) { return _this._appService.stockDetail(params["id"]); })
            .subscribe(function (r) {
            _this.stockDetail = r;
            _this.stockName = r.Name;
            if (_this.stockDetail.DividendYield.length == 0)
                _this.stockDetail.DividendYield = "n/a";
        }, function (error) { return _this.statusMessage = error; });
    };
    DetailComponent.prototype.Return = function () {
        console.error(" ");
    };
    return DetailComponent;
}());
DetailComponent = __decorate([
    core_1.Component({
        templateUrl: './app/components/app.stockdetail.html',
        providers: [app_service_stocks_1.AppServiceStocks]
    }),
    __metadata("design:paramtypes", [router_1.ActivatedRoute,
        app_service_stocks_1.AppServiceStocks,
        router_1.Router])
], DetailComponent);
exports.DetailComponent = DetailComponent;
//# sourceMappingURL=app.stockdetail.js.map