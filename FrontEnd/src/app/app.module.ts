import { UiModule } from './ui/ui.module';
import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppComponent } from './app.component';
import { HomeComponent } from './home/home.component';
import { RegisterComponent } from './register/register.component';
import { RouterModule,Routes} from '@angular/router';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BusLinesComponent } from './bus-lines/bus-lines.component';
import { CardVerificationComponent } from './card-verification/card-verification.component';
import { CenovnikComponent } from './cenovnik/cenovnik.component';
import { LoginComponent } from './login/login.component';
import { ProfilComponent } from './profil/profil.component';
import { RedvoznjeComponent } from './redvoznje/redvoznje.component';

const routes : Routes = [
  {path : "home", component: HomeComponent},
  {path : "register", component: RegisterComponent},
  {path : "", component: HomeComponent, pathMatch:"full"},
  {path : "**", redirectTo: ""},
]

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    RegisterComponent,
    BusLinesComponent,
    CardVerificationComponent,
    CenovnikComponent,
    LoginComponent,
    ProfilComponent,
    RedvoznjeComponent
  ],
  imports: [
    BrowserModule,
    UiModule,
    RouterModule.forRoot(routes), 
    ReactiveFormsModule,
    FormsModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
