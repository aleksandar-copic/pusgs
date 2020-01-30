import { Component, OnInit, NgZone } from '@angular/core';
import { LineHttpService } from '../services/line.service';

@Component({
  selector: 'app-current-location',
  templateUrl: './current-location.component.html',
  styleUrls: ['./current-location.component.css'],
  styles: ['agm-map {height: 500px; width: 100%;}']
})
export class CurrentLocationComponent implements OnInit {

  constructor(private ngZone: NgZone, private lineService: LineHttpService) { }

  ngOnInit() {
  }

}
