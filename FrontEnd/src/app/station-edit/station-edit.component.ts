import { Component, OnInit } from '@angular/core';
import { Validators, FormBuilder } from '@angular/forms';
import { StationEditHttpService } from '../services/stationEdit.service';
import { Station } from '../classes/station';
import { AddStation } from '../classes/addStation';
import { Line } from '../classes/line';
import { Router } from '@angular/router';

@Component({
  selector: 'app-station-edit',
  templateUrl: './station-edit.component.html',
  styleUrls: ['./station-edit.component.css']
})
export class StationEditComponent implements OnInit {

  constructor(private fb: FormBuilder,private router: Router,private http:StationEditHttpService) { }

  lineForm = this.fb.group({
    SerialNumber: ['', Validators.required],
  });

  stationForm = this.fb.group({
    Name: ['', Validators.required],
    Address: ['', Validators.required],
  });

  selectedLine: string
  selectedStation: string
  stations: string[] = []
  station: Station = new Station()
  lines: string[] = []
  idStation: number
  stationUpdate: Station = new Station()

  line: Line = new Line()

  LinesAdd: Array<string> = [];
  linesToChose: Array<string> = [];
  lineAddSelected: string

  temp: boolean = true
  newStation: AddStation = new AddStation()

  ime: string
  adresa: string

  ngOnInit() {
    this.http.getAll().subscribe((station) => {
      this.stations = station;
      this.selectedStation = this.stations[0];
      err => console.log(err);
    });
    this.http.getAllLines().subscribe((data) => {
      this.linesToChose = data;
      this.lineAddSelected = this.linesToChose[0];
      err => console.log(err);
    });
  }

  getSelectedStation(){
    this.http.getStation(this.selectedStation).subscribe((data) => {
      this.idStation = data.Id;
      this.stationForm.patchValue({ Name : data.Name, Address : data.Address })
      this.http.getLines(this.selectedStation).subscribe((data) => {
        this.lines = data;
        this.selectedLine = this.lines[0];
        err => console.log(err);
      })
      err => console.log(err);
    });
    // this.http.getLines(this.selectedStation).subscribe((data) => {
      // this.lines = data;
      // this.selectedLine = this.lines[0];
      // err => console.log(err);
    // });
  }

  getSelectedLine(){
    this.http.getSelectedLine(this.selectedLine).subscribe((data) => {
      this.line = data;
      this.lineForm.patchValue({ SerialNumber : data.SerialNumber })
      err => console.log(err);
    });
  }

  deleteSelectedStation(){
    this.http.deleteSelectedStation(this.idStation).subscribe((data) => {
      if(data == "success")
      {
        alert("Uspesno obrisana linija");
        this.router.navigate(["/stationEdit"]);
      }
      else
      {
        alert("Vec postoji linija sa tim rednim brojem");
        this.router.navigate(["/stationEdit"]);
      }
      err => console.log(err);
    });
  }

  UpdateStation(){
    this.stationUpdate = this.stationForm.value;
    this.stationUpdate.Id = this.idStation;
    this.http.updateStation(this.stationUpdate).subscribe((data) => {
      if(data == "uspesno")
      {
        alert("Uspesno izmenjena stanica");
        this.router.navigate(["/stationEdit"]);
      }
      else
      {
        alert("Vec postoji stanica sa tim imenom");
        this.router.navigate(["/stationEdit"]);
      }
      err => console.log(err);
    });
  }

  addLine()
  {
    this.temp = true;
    this.LinesAdd.forEach(element => {
      if(element == this.lineAddSelected){
        this.temp = false;
      }
    });
    
    if(this.temp == true)
    {
      this.LinesAdd.push(this.lineAddSelected);
      this.lineAddSelected = null;
    }
    else
    {
      this.temp = true;
    }
  }

  AddStation(){
    this.newStation.Lines = this.LinesAdd;
    this.newStation.Name = this.ime;
    this.newStation.Address = this.adresa;
    console.log(this.newStation.Name + this.ime);
    this.http.addStation(this.newStation).subscribe((data) => {
      if(data == "uspesno")
      {
        this.newStation = new AddStation();
        this.LinesAdd = [];
        this.lineAddSelected = this.linesToChose[0];

        alert("Uspesno dodata stanica");
        this.router.navigate(["/stationEdit"]);
      }
      else
      {
        this.newStation = new AddStation();
        this.LinesAdd = [];
        this.lineAddSelected = this.linesToChose[0];

        alert("Neuspesno dodata stanica");
        this.router.navigate(["/stationEdit"]);
      }
      err => console.log(err);
    });
  }
}