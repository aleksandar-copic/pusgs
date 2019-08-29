import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable()
export class PriceListEditHttpService{
    base_url = "http://localhost:52295"
    constructor(private http: HttpClient){ }

    getPrice(ticketTypeId: number) : Observable<any>{
        return Observable.create((observer) => {    
            this.http.get<any>(this.base_url + "/api/ticketPriceEdit/ticketPriceEditGetPrice/" + ticketTypeId).subscribe(data =>{
                observer.next(data);
                observer.complete();     
            })             
        });
    }

    editPrice(ticketTypeId: number, price: number): Observable<any>{
        return Observable.create((observer) => {
            let httpOptions={
                headers:{
                    "Content-type": "application/json"
                }
            }
            this.http.post<any>(this.base_url + "/api/ticketPriceEdit/UpdateTicketPrice/" + ticketTypeId + "/"+ price, httpOptions).subscribe(data => {
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