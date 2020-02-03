import { FormBuilder, Validators } from '@angular/forms';
import { Station } from './../classes/station';
import { LineHttpService } from './../services/line.service';
import { Polyline } from './../map/model/polyline';
import { GeoLocation } from './../map/model/geolocation';
import { MarkerInfo } from '../map/model/marker-info.model';
import { Component, OnInit, NgZone } from '@angular/core';
import { Line } from '../classes/line';

@Component({
  selector: 'app-routes',
  templateUrl: './routes.component.html',
  styleUrls: ['./routes.component.css'],
  styles: ['agm-map {height: 500px; width: 700px;}'] //postavljamo sirinu i visinu mape
})
export class RoutesComponent implements OnInit {

  constructor(private ngZone: NgZone, private http: LineHttpService, private fb: FormBuilder){ }

  // lineForm = this.fb.group({
  //   SerialNumber: ['', Validators.required],
  // });

  // stationForm = this.fb.group({
  //   Name: ['', Validators.required],
  //   Address: ['', Validators.required],
  //   X: ['', Validators.required],
  //   Y: ['', Validators.required],
  // });

  markerInfo: MarkerInfo;
  public polyline: Polyline;
  public zoom: number;

  lines: string[];
  selectedLine: string;
  line: Line;
  stations: string[];
  stationsArray: Station[] = [];
  selectedStation: string;
  station: Station;
  clicked: boolean;
 
  /* ------------------------------- Test variables ------------------------------- */
  public test_polyline: Polyline;
  test_markerInfo: MarkerInfo;
  /* ------------------------------------------------------------------------------ */

  ngOnInit() {
    this.markerInfo = new MarkerInfo(new GeoLocation(45.242268, 19.842954), 
      "assets/ftn.png",
      "Jugodrvo" , "" , "http://ftn.uns.ac.rs/691618389/fakultet-tehnickih-nauka");

    this.polyline = new Polyline([], 'blue', { url:"assets/busicon.png", scaledSize: {width: 50, height: 50}});
    
    /* ------------------------------------ Test ------------------------------------ */
    this.test_markerInfo = new MarkerInfo(new GeoLocation(46, 20),
      "assets/ftn.png",
      "Test", "" , "http://google.rs");

    this.test_polyline = new Polyline([], 'blue', { url:"assets/busicon.png", scaledSize: {width: 50, height: 50}});
    this.test_polyline.addLocation(new GeoLocation(46, 19));
    this.test_polyline.addLocation(new GeoLocation(46, 20));
    this.test_polyline.addLocation(new GeoLocation(46.5, 19.5));
    /* ------------------------------------------------------------------------------ */
    
    // Get all lines from database on init
    this.http.getAll().subscribe((line) => {
      this.lines = line;
      err => console.log(err);
    });    
  }

  // Get all lines from database
  getAllLines(){
    this.http.getAll().subscribe((line) => {
      this.lines = line;
      err => console.log(err);
    });    
  }

  // Get all stations from selected line and add them into stationsArray and polyline
  getSelectedLine(lineName: string){
    this.polyline.path = [];
    this.stations = [];
    this.stationsArray = [];
    
    this.http.getLine(lineName).subscribe((data) => {
      this.line = data;
      //this.lineForm.patchValue({ SerialNumber : data.SerialNumber })
      err => console.log(err);
      this.http.getStations(this.line.SerialNumber.toString()).subscribe((data) => {
        this.stations = data;
        //this.selectedStation = this.stations[0];
        err => console.log(err);
        
        for(let i = 0; i < this.stations.length; i++){
          this.http.getSelectedStation(this.stations[i]).subscribe((data) => {
            this.station = data;
            this.stationsArray.push(data);
            this.polyline.addLocation(new GeoLocation(this.station.X, this.station.Y));
          })
        }
      });
    });
  }

  // Puts Marker on click
  placeMarker($event){
    this.polyline.addLocation(new GeoLocation($event.coords.lat, $event.coords.lng))
    console.log(this.polyline)
  }
  
}
