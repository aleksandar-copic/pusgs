import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Station } from '../classes/station';
import { AddStation } from '../classes/addStation';

@Injectable()
export class StationEditHttpService{
    base_url = "http://localhost:52295"
    constructor(private http: HttpClient){ }

    getAll() : Observable<any>{
        return Observable.create((observer) => {    
            this.http.get<any>(this.base_url + "/api/StationEdit/GetStations").subscribe(data =>{
                observer.next(data);
                observer.complete();     
            })             
        });
    }

    getStation(id: string) : Observable<any>{
        return Observable.create((observer) => {    
            this.http.get<any>(this.base_url + "/api/StationEdit/GetSelectedStation/" + id).subscribe(data =>{
                observer.next(data);
                observer.complete();     
            })             
        });
    }

    getLines(id: string) : Observable<any>{
        return Observable.create((observer) => {    
            this.http.get<any>(this.base_url + "/api/StationEdit/GetLines/" + id).subscribe(data =>{
                observer.next(data);
                observer.complete();     
            })             
        });
    }

    getSelectedLine(name: string) : Observable<any>{
        return Observable.create((observer) => {    
            this.http.get<any>(this.base_url + "/api/StationEdit/SelectedLine/" + name).subscribe(data =>{
                observer.next(data);
                observer.complete();     
            })             
        });
    }

    getAllLines() : Observable<any>{
        return Observable.create((observer) => {    
            this.http.get<any>(this.base_url + "/api/StationEdit/GetAllLines").subscribe(data =>{
                observer.next(data);
                observer.complete();     
            })             
        });
    }

    updateStation(station: Station) : Observable<any>{

        return Observable.create((observer) => {
            let data = station;
            let httpOptions={
                headers:{
                    "Content-type": "application/json"
                }
            }
            this.http.post<any>(this.base_url + "/api/StationEdit/UpdateStation",data,httpOptions).subscribe(data => {
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

    deleteSelectedStation(id: number) : Observable<any>{
        return Observable.create((observer) => {    
            this.http.delete<any>(this.base_url + "/api/StationEdit/DeleteSelectedStation/" + id).subscribe(data =>{
                observer.next(data);
                observer.complete();     
            })             
        });
    }

    addStation(line: AddStation) : Observable<any>{

        return Observable.create((observer) => {
            let data = line;
            let httpOptions={
                headers:{
                    "Content-type": "application/json"
                }
            }
            this.http.post<any>(this.base_url + "/api/StationEdit/AddStation",data,httpOptions).subscribe(data => {
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

    // addLines() : Observable<any>{
    //     return Observable.create((observer) => {   
    //         let data = line; 
    //         this.http.post<any>(this.base_url + "/api/StationEdit/AddStationToLine", data).subscribe(data =>{
    //             observer.next(data);
    //             observer.complete();     
    //         })             
    //     });
    // }
}