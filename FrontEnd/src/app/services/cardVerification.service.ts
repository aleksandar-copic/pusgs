import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable()
export class CardVerificationHttpService{
    base_url = "http://localhost:52295"
    constructor(private http: HttpClient){ }

    getStatus(i: number) : Observable<any>{
        return Observable.create((observer) => {    
        console.log("boske2" + i)
            this.http.get<any>(this.base_url + "/api/CardVerification/Check/" + i).subscribe(data =>{
                observer.next(data);
                observer.complete();     
            })             
        });
    }
}