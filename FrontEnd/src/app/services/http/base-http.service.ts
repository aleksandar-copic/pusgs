import {HttpClient} from '@angular/common/http';
import { Observable } from 'rxjs';
import { Injectable } from '@angular/core';

@Injectable()
export class BaseHttpService<T>
{
    baseUrl = "http://localhost:52295"
    specificUrl = "";

    constructor(private http: HttpClient){

    }


    getAll() : Observable<T[]>{
        return this.http.get<T[]>(this.baseUrl + this.specificUrl);
    }

    getById() : Observable<T>{
        return this.http.get<T>(this.baseUrl + this.specificUrl + '/${id}');
    }
}
