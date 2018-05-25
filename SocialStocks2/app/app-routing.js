"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
Object.defineProperty(exports, "__esModule", { value: true });
var core_1 = require("@angular/core");
var router_1 = require("@angular/router");
var app_component_stocks_1 = require("./components/app.component.stocks");
var app_component_stockadd_1 = require("./components/app.component.stockadd");
var app_component_stockrem_1 = require("./components/app.component.stockrem");
var app_component_stocknews_1 = require("./components/app.component.stocknews");
var app_stockdetail_1 = require("./components/app.stockdetail");
var app_stocktweets_1 = require("./components/app.stocktweets");
var not_found_component_1 = require("./components/not-found.component");
// import { CanDeactivateGuard } from './can-deactivate-guard.service';
// import { AuthGuard } from './auth-guard.service';
var selective_preloading_strategy_1 = require("./components/selective-preloading-strategy");
var appRoutes = [
    {
        path: 'Stocks/StocksAngular/Add',
        component: app_component_stockadd_1.AddComponent
    },
    {
        path: 'Stocks/StocksAngular/Delete/:id',
        component: app_component_stockrem_1.RemComponent
    },
    {
        path: 'Stocks/StocksAngular/Detail/:id',
        component: app_stockdetail_1.DetailComponent
    },
    {
        path: 'Stocks/StocksAngular/News/:id',
        component: app_component_stocknews_1.NewsComponent
    },
    {
        path: 'Stocks/StocksAngular/Tweets/:id',
        component: app_stocktweets_1.TweetComponent
    },
    {
        path: 'Stocks/StocksAngular',
        component: app_component_stocks_1.AppStocks
    },
    {
        path: 'stocks/stocksangular',
        component: app_component_stocks_1.AppStocks
    },
    { path: '', redirectTo: 'Stocks/StocksAngular', pathMatch: 'full' },
    { path: '**', component: not_found_component_1.PageNotFoundComponent }
];
var AppRoutingModule = /** @class */ (function () {
    function AppRoutingModule() {
    }
    AppRoutingModule = __decorate([
        core_1.NgModule({
            imports: [
                router_1.RouterModule.forRoot(appRoutes, { preloadingStrategy: selective_preloading_strategy_1.SelectivePreloadingStrategy })
            ],
            exports: [
                router_1.RouterModule
            ],
            providers: [
                //     CanDeactivateGuard,
                selective_preloading_strategy_1.SelectivePreloadingStrategy
            ]
        })
    ], AppRoutingModule);
    return AppRoutingModule;
}());
exports.AppRoutingModule = AppRoutingModule;
/*
Copyright 2017 Google Inc. All Rights Reserved.
Use of this source code is governed by an MIT-style license that
can be found in the LICENSE file at http://angular.io/license
*/
//# sourceMappingURL=app-routing.js.map