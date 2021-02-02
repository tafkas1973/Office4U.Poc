import { CommonModule } from "@angular/common";
import { HttpClientModule, HTTP_INTERCEPTORS } from "@angular/common/http";
import { NgModule, Optional, SkipSelf } from "@angular/core";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { BrowserModule } from "@angular/platform-browser";
import { BrowserAnimationsModule } from "@angular/platform-browser/animations";
import { BsDropdownModule } from "ngx-bootstrap/dropdown";
import { AppRoutingModule } from "../app-routing.module";
import { HasRoleDirective } from "../shared/directives/has-role.directive";
import { EnsureModuleLoadedOnceGuard } from "./ensure-module-loaded-once.guard";
import { ErrorInterceptor } from "./interceptors/error.interceptor";
import { JwtInterceptor } from "./interceptors/jwt.interceptor";
import { LoadingInterceptor } from "./interceptors/loading.interceptor";
import { NavComponent } from "./nav/nav.component";

@NgModule({
    declarations: [
        NavComponent,
        HasRoleDirective
    ],    
    imports: [
        CommonModule,
        FormsModule,
        BsDropdownModule.forRoot(),
        
        ReactiveFormsModule,
        BrowserModule,
        HttpClientModule,
        AppRoutingModule,
        BrowserAnimationsModule
    ],
    exports: [
        NavComponent,
        BsDropdownModule,
        HasRoleDirective 
    ],
    providers: [
        {            
            provide: HTTP_INTERCEPTORS,
            useClass: ErrorInterceptor,
            multi: true
        },
        {
            provide: HTTP_INTERCEPTORS,
            useClass: JwtInterceptor,
            multi: true
        },
        {
            provide: HTTP_INTERCEPTORS,
            useClass: LoadingInterceptor,
            multi: true
        }
    ] // these should be singleton
})

// Ensure that CoreModule is only loaded into AppModule
export class CoreModule extends EnsureModuleLoadedOnceGuard {

    // Looks for the module in the parent injector to see if it's already been loaded (only want it loaded once)
    constructor(@Optional() @SkipSelf() parentModule: CoreModule) {
        super(parentModule);
    }
}