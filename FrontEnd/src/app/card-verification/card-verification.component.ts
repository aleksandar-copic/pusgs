import { Component, OnInit } from '@angular/core';

import { NgForm, FormBuilder, Validators } from '@angular/forms';
import { CardVerificationHttpService } from '../services/cardVerification.service';
import { Observable } from 'rxjs';
import { Card } from '../classes/card';

@Component({
  selector: 'app-card-verification',
  templateUrl: './card-verification.component.html',
  styleUrls: ['./card-verification.component.css']
})
export class CardVerificationComponent implements OnInit {

  verificationForm = this.fb.group({
    Id: ['', Validators.required]
  });
  card: Card = new Card()
  message: string
  id: string
  constructor(private fb: FormBuilder, private http: CardVerificationHttpService) { }

  checkId()
  {
    this.card = this.verificationForm.value
    console.log("penal: " + this.card.Id);
      this.http.getStatus(this.card.Id).subscribe(temp => {
      if(temp == "true"){
        this.message = "Aktivan"
      }
      else{
        this.message = "Istekla"
      }
    });
  }

  ngOnInit() {
  }

}
