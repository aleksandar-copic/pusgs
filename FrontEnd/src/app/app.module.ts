import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import {FormsModule} from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS} from '@angular/common/http';
import { RouterModule,Routes} from '@angular/router';
import { AppComponent } from './app.component';
import { LoginComponent } from './login/login.component';
import { AuthHttpService } from './services/http/auth.service';
import { TokenInterceptor } from './interceptors/token.interceptor';
import { HomeComponent } from './home/home.component';
import { RegisterComponent } from './register/register.component';
import { UiModule } from './ui/ui.module';
import { ReactiveFormsModule } from '@angular/forms';
import { RedvoznjeComponent } from './redvoznje/redvoznje.component';
//import { RedVoznjeHttpService } from 'src/services/redvoznje.service';
import { CenovnikComponent } from './cenovnik/cenovnik.component';
import { CenovnikHttpService } from './services/cenovnik.service';
import { ProfilComponent } from './profil/profil.component';
import { ProfilHttpService } from './services/profil.service';
import { CardVerificationComponent } from './card-verification/card-verification.component';
import { CardVerificationHttpService } from './services/cardVerification.service';
import { RedVoznjeHttpService } from './services/redvoznje.service';
/*import { LineEditComponent } from './line-edit/line-edit.component';
import { StationEditComponent } from './station-edit/station-edit.component';
import { TimetableEditComponent } from './timetable-edit/timetable-edit.component';
import { LineEditHttpService } from 'src/services/lineEdit.service';
import { TimetableEditHttpService } from 'src/services/timetableEdit.service';
import { StationEditHttpService } from 'src/services/stationEdit.service';
*/
import { from } from 'rxjs';
import { AuthGuardAdmin } from './services/http/auth.guard';
import { AuthGuardController } from './services/http/auth2.guard';
import { VerificateUserComponent } from './verificate-user/verificate-user.component';
import { VerificateUserHttpService } from './services/verificateUser.service';
import { LineComponent } from './line/line.component';
import { PriceListEditComponent } from './price-list-edit/price-list-edit.component';
import { StationEditComponent } from './station-edit/station-edit.component';


const routes : Routes = [
  {path : "login", component: LoginComponent},
  {path : "home", component: HomeComponent},
  {path : "register", component: RegisterComponent},
  {path : "redvoznje", component: RedvoznjeComponent},
  {path : "cenovnik", component: CenovnikComponent},
  {path : "profil", component: ProfilComponent},
  {path: "cardVerification", component: CardVerificationComponent},
  {path: "verificateUser", component: VerificateUserComponent},
  {path : "", component: HomeComponent, pathMatch:"full"},
  {path : "**", redirectTo: ""},
]


@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    HomeComponent,
    RegisterComponent,
    RedvoznjeComponent,
    CardVerificationComponent,
    CenovnikComponent,
    ProfilComponent,
    VerificateUserComponent,
    LineComponent,
    PriceListEditComponent,
    StationEditComponent,
    //LineEditComponent,
    ///StationEditComponent,
   // TimetableEditComponent,
    
  ],
  imports: [
    BrowserModule,
    FormsModule,
    HttpClientModule,
    RouterModule.forRoot(routes),  
    ReactiveFormsModule,
    UiModule 
  ],
  providers: [{provide: HTTP_INTERCEPTORS, useClass:TokenInterceptor, multi:true},AuthHttpService,CardVerificationHttpService, CenovnikHttpService,ProfilHttpService,RedVoznjeHttpService, VerificateUserHttpService], //svi mogu da pristupe(injektuju servis)
  bootstrap: [AppComponent]
})
export class AppModule { }
