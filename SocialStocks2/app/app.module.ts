import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { HttpModule, JsonpModule } from '@angular/http';
import { Http } from '@angular/http';
import { Router } from '@angular/router';

import { AppServiceStocks } from './services/app.service.stocks';
import { AppStocks } from './components/app.component.stocks';
import { AddComponent } from './components/app.component.stockadd';
import { RemComponent } from './components/app.component.stockrem';
import { NewsComponent } from './components/app.component.stocknews';
import { TweetComponent } from './components/app.stocktweets';
import { DetailComponent } from './components/app.stockdetail';

import { AppComponent } from './components/app.component';

import { PageNotFoundComponent } from './components/not-found.component';
import { AppRoutingModule } from './app-routing';

@NgModule({
    imports: [BrowserModule, HttpModule, FormsModule, AppRoutingModule],
    declarations: [
        AppComponent,
        AppStocks,
        AddComponent,
        PageNotFoundComponent,
        RemComponent,
        DetailComponent,
        TweetComponent,
        NewsComponent],
    bootstrap: [AppComponent]
})
export class AppModule {

    // Diagnostic only: inspect router configuration
    constructor(router: Router) {
       // console.log('Routes: ', JSON.stringify(router.config, undefined, 2));
    }
}
