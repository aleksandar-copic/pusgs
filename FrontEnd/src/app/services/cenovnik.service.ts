import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { KupiKartu } from '../classes/kupiKartu';

@Injectable()
export class CenovnikHttpService{
    base_url = "http://localhost:52295"
    constructor(private http: HttpClient){ }

    getUserType() : Observable<any>{
        return Observable.create((observer) => {    
            this.http.get<any>(this.base_url + "/api/PriceList/UserType").subscribe(data =>{
                observer.next(data);
                observer.complete();     
            })             
        });
    }

    getCene(tipKarte: string,tipKorisnika: string) : Observable<any>{
        return Observable.create((observer) => {    
            this.http.get<any>(this.base_url + "/api/PriceList/Cene/" + tipKarte + "/" + tipKorisnika).subscribe(data =>{
                observer.next(data);
                observer.complete();     
            })             
        });
    }
    
    kupiKartu(karta: KupiKartu) : Observable<any>{

        return Observable.create((observer) => {
            let data = karta;
            let httpOptions={
                headers:{
                    "Content-type": "application/json"
                }
            }
            this.http.post<any>(this.base_url + "/api/PriceList/KupiKartu",data,httpOptions).subscribe(data => {
                observer.next("uspesno");
                observer.complete();
            },
            err => {
                console.log(err);
                observer.next("neuspesno");
                observer.complete();
            });
        });
     
    }
}