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
var platform_browser_1 = require("@angular/platform-browser");
var forms_1 = require("@angular/forms");
var http_1 = require("@angular/http");
var router_1 = require("@angular/router");
var app_component_stocks_1 = require("./components/app.component.stocks");
var app_component_stockadd_1 = require("./components/app.component.stockadd");
var app_component_stockrem_1 = require("./components/app.component.stockrem");
var app_component_stocknews_1 = require("./components/app.component.stocknews");
var app_component_1 = require("./components/app.component");
var not_found_component_1 = require("./components/not-found.component");
var app_routing_1 = require("./app-routing");
var AppModule = (function () {
    // Diagnostic only: inspect router configuration
    function AppModule(router) {
        // console.log('Routes: ', JSON.stringify(router.config, undefined, 2));
    }
    return AppModule;
}());
AppModule = __decorate([
    core_1.NgModule({
        imports: [platform_browser_1.BrowserModule, http_1.HttpModule, forms_1.FormsModule, app_routing_1.AppRoutingModule],
        declarations: [
            app_component_1.AppComponent,
            app_component_stocks_1.AppStocks,
            app_component_stockadd_1.AddComponent,
            not_found_component_1.PageNotFoundComponent,
            app_component_stockrem_1.RemComponent,
            app_component_stocknews_1.NewsComponent
        ],
        bootstrap: [app_component_1.AppComponent]
    }),
    __metadata("design:paramtypes", [router_1.Router])
], AppModule);
exports.AppModule = AppModule;
//# sourceMappingURL=app.module.js.map