import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";

@Injectable()
export class RedVoznjeHttpService {
  base_url = "http://localhost:52295";
  constructor(private http: HttpClient) {}

  /*redVoznjeVremena(username: string, password: string) : Observable<any>{

        return Observable.create((observer) => {

            let data = `username=${username}&password=${password}&grant_type=password`;
            let httpOptions={
                headers:{
                    "Content-type": "application/x-www-form-urlencoded"
                }
            }
            this.http.post<any>(this.base_url + "api/RedVoznje",data,httpOptions).subscribe(data => {
                localStorage.jwt = data.access_token;
                observer.next("uspesno");
                localStorage.setItem("loggedUser",username);
                observer.complete();
            },
            err => {
                console.log(err);
                observer.next("neuspesno");
                observer.complete();
            });
        });
     
    }*/

  //   getAll(): Observable<any> {
  //     return Observable.create(observer => {
  //       this.http
  //         .get<any>(this.base_url + "/api/RedVoznje/")
  //         .subscribe(data => {
  //           observer.next(data);
  //           observer.complete();
  //         });
  //     });
  //   }

  getSelected(
    timetableTypeId: number,
    dayTypeId: number,
    lineId: number
  ): Observable<any> {
    return Observable.create(observer => {
      this.http
        .get<any>(
          this.base_url +
            "/api/RedVoznje/IspisReda" +
            `/${timetableTypeId}` +
            `/${dayTypeId}` +
            `/${lineId}`
        )
        .subscribe(data => {
          observer.next(data);
          observer.complete();
        });
    });
  }
  GetPolasci(
    selectedLinija: number,
    selectedDan: number,
    selectedTeritorija: number
  ): Observable<any> {
    return Observable.create(observer => {
      let data = selectedLinija;
      let httpOptions = {
        headers: {
          "Content-type": "application/json"
        }
      };
      this.http
        .get<any>(
          this.base_url +
            "/api/RedVoznje/GetPolasci/" +
            selectedLinija +
            "/" +
            selectedDan +
            "/" +
            selectedTeritorija
        )
        .subscribe(data => {
          observer.next(data);
          observer.complete();
        });
    });
  }
}
