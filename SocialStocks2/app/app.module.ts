import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { HttpModule, JsonpModule } from '@angular/http';
import { Http } from '@angular/http';
import { Router } from '@angular/router';

import { AppServiceStocks } from './services/app.service.stocks';
import { AppStocks } from './components/app.component.stocks';
import { AddComponent } from './components/app.component.stockadd';
import { AppComponent } from './components/app.component';

import { PageNotFoundComponent } from './components/not-found.component';
import { AppRoutingModule } from './app-routing';

@NgModule({
    imports: [BrowserModule, HttpModule, FormsModule, AppRoutingModule],
    declarations: [AppComponent, AppStocks, AddComponent, PageNotFoundComponent],
    bootstrap: [AppComponent]
})
export class AppModule {

    // Diagnostic only: inspect router configuration
    constructor(router: Router) {
        console.log('Routes: ', JSON.stringify(router.config, undefined, 2));
    }
}
