import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { AppComponent } from './components/app.component';
import { AppStocks } from './components/app.component.stocks';
import { AddComponent } from './components/app.component.stockadd';

import { PageNotFoundComponent } from './components/not-found.component';

// import { CanDeactivateGuard } from './can-deactivate-guard.service';
// import { AuthGuard } from './auth-guard.service';

import { SelectivePreloadingStrategy } from './components/selective-preloading-strategy';

const appRoutes: Routes = [
    {
        path: 'Stocks/StocksAngular/Add',
        component: AddComponent
    },
    {
        path: 'Stocks/StocksAngular',
        component: AppStocks
    },    
    { path: '', redirectTo: 'Stocks/StocksAngular', pathMatch: 'full' },
    { path: '**', component: PageNotFoundComponent }
];

@NgModule({
    imports: [
        RouterModule.forRoot(
            appRoutes,
            { preloadingStrategy: SelectivePreloadingStrategy }
        )
    ],
    exports: [
        RouterModule
    ],
    providers: [
   //     CanDeactivateGuard,
        SelectivePreloadingStrategy
    ]
})
export class AppRoutingModule { }


/*
Copyright 2017 Google Inc. All Rights Reserved.
Use of this source code is governed by an MIT-style license that
can be found in the LICENSE file at http://angular.io/license
*/
