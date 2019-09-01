import { Injectable } from "@angular/core";
import { HttpClient } from '@angular/common/http';
import { Observable } from "rxjs";
import { User } from "../classes/user";

@Injectable()
export class ProfilHttpService{
    base_url = "http://localhost:52295"
    constructor(private http: HttpClient){ }

    getUser() : Observable<any>{
        return Observable.create((observer) => {    
            this.http.get<any>(this.base_url + "/api/Profile/User").subscribe(data =>{
                observer.next(data);
                observer.complete();     
            })             
        });
    }

    updateUser(user: User) : Observable<any>{

        return Observable.create((observer) => {
            let data = user;
            let httpOptions={
                headers:{
                    "Content-type": "application/json"
                }
            }
            this.http.post<any>(this.base_url + "/api/Profile/UpdateUser",data,httpOptions).subscribe(data => {
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