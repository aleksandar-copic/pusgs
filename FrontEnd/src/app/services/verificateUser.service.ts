import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable()
export class VerificateUserHttpService
{
    base_url = "http://localhost:52295"
    constructor(private http: HttpClient){ }

    getUsers() : Observable<any>{
        return Observable.create((observer) => {    
        console.log("boske2")
            this.http.get<any>(this.base_url + "/api/Verification/ReturnUsers").subscribe(data =>{
                observer.next(data);
                observer.complete();     
            })             
        });
    }


    getSelectedUser(id: string) : Observable<any>{
        return Observable.create((observer) => {  
            this.http.get<any>(this.base_url + "/api/Verification/SelectedUser/" + id).subscribe(data =>{
                observer.next(data);
                observer.complete();     
            })             
        });
    }

    downloadImage(id : string) : Observable<any> {
        return Observable.create((observer) => {  
            this.http.get<any>(this.base_url + "/api/Verification/DownloadPicture/" + id).subscribe(data =>{
                observer.next(data);
                observer.complete();     
            })             
        });
    }

    odluka(id : string,odluka: string) : Observable<any> {
        return Observable.create((observer) => {  
            this.http.get<any>(this.base_url + "/api/Verification/Odluka/" + id + "/"+ odluka).subscribe(data =>{
                observer.next(data);
                observer.complete();     
            })             
        });
    }
}