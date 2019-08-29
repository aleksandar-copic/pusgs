import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { PriceListEditHttpService } from '../services/priceListEdit.service';
import { Router } from '@angular/router';
import { TempTicket } from '../classes/tempTicket';

@Component({
  selector: 'app-price-list-edit',
  templateUrl: './price-list-edit.component.html',
  styleUrls: ['./price-list-edit.component.css']
})
export class PriceListEditComponent implements OnInit {

  ticketPriceEdit = this.fb.group({
    Price: ['', Validators.required],
  });

  ticketTypeId: number
  selectedTicket: string
  allTicketTypes: string[] = ["Vremenska karta", "Dnevna karta", "Mesečna karta", "Godišnja karta"]

  tempTicket: TempTicket = new TempTicket()
  constructor(private fb: FormBuilder, private http: PriceListEditHttpService, private router: Router) { }

  ngOnInit() {
    this.selectedTicket = this.allTicketTypes[0]
  }

  Izaberi(){
    if(this.selectedTicket == "Vremenska karta"){
      this.ticketTypeId = 1
    }
    else if(this.selectedTicket == "Dnevna karta"){
      this.ticketTypeId = 2
    }
    else if(this.selectedTicket == "Mesečna karta"){
      this.ticketTypeId = 3
    }
    else if(this.selectedTicket == "Godišnja karta"){
      this.ticketTypeId = 4
    }

    this.http.getPrice(this.ticketTypeId).subscribe((item) =>  {
        console.log(item)
        this.ticketPriceEdit.patchValue({Price: item})
      
    });
  }

  Izmeni(){
    console.log("AAAAAAAAAa")
    this.tempTicket = this.ticketPriceEdit.value
    this.http.editPrice(this.ticketTypeId, this.tempTicket.Price).subscribe((item) =>  {
      if (item == "uspesno") {
        alert("INFO: Uspesno izmenjena cena.")
        this.router.navigate(["/home"]);
      } 
      else {
        err => console.log("Greska pri izmeni profila");
      }
    });
  }

}
