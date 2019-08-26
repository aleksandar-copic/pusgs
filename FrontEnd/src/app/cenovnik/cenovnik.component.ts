import { Component, OnInit } from '@angular/core';
import { CenovnikHttpService } from '../services/cenovnik.service';
import { KupiKartu } from '../classes/kupiKartu';

@Component({
  selector: 'app-cenovnik',
  templateUrl: './cenovnik.component.html',
  styleUrls: ['./cenovnik.component.css']
})
export class CenovnikComponent implements OnInit {

  constructor(private http: CenovnikHttpService) { }
  karta: KupiKartu = new KupiKartu();
  userType: string
  cena: number
  karte: string[] = []
  selectedTip: string
  kupljena: string

  ngOnInit() {
    this.karte = [];
    this.kupljena = "";
    this.http.getUserType().subscribe((userType) => {
      this.userType = userType;
      localStorage.userType = userType;
      if(this.userType == "neregistrovan")
      {
        console.log("neregistrovan");
        this.karte.push("Vremenska karta");
      }
      else
      {
        console.log("this: " + this.userType);
        console.log("ret: " + userType);
        this.karte.push("Vremenska karta");
        this.karte.push("Dnevna karta");
        this.karte.push("Mesečna karta");
        this.karte.push("Godišnja karta");         
      }
      this.selectedTip = this.karte[0];
      err => console.log(err);
    });   
  }

  getCenaKarte(){
    console.log("selektovana karta44: " + this.selectedTip);
    this.http.getCene(this.selectedTip,this.userType).subscribe((data) => {
      this.cena = data;
      err => console.log(err);
    });
  }
  
  kupiKartu(){
    console.log("selektovana karta2: " + this.selectedTip);

    this.karta.Price = this.cena;
    this.karta.Username = localStorage.loggedUser;
    this.karta.TipKarte = this.selectedTip;
    this.http.kupiKartu(this.karta).subscribe((data) => {
      if(data == "uspesno")
      {
        this.kupljena = "uspesno";
      }
      else
      {
        this.kupljena = "doslo je do greske prilikom kupovine karte.";
      }
      err => console.log(err);
    });
  }

}