import { Component, OnInit } from "@angular/core";
import { TimetableType } from "../classes/timetableType";
import { Timetable } from "../classes/timetable";
import { Line } from "../classes/line";
import { DayType } from "../classes/dayType";
import { RedVoznjeInfo } from "../classes/redVoznjeInfo";
import { RedVoznjeHttpService } from "../services/redvoznje.service";

@Component({
  selector: "app-redvoznje",
  templateUrl: "./redvoznje.component.html",
  styleUrls: ["./redvoznje.component.css"]
})

export class RedvoznjeComponent implements OnInit {
  redVoznjeInfo: RedVoznjeInfo = new RedVoznjeInfo();
  selectedTimetableType: string;
  selectedDayType: string;
  selectedLine: Line = new Line();
  selectedLinija: number;
  selectedDan: number;
  selectedTeritorija: number;
  polasci: string;
  filteredLines: Line[] = [];
  timetable: Timetable = new Timetable();
  lines : string [];

  constructor(private http: RedVoznjeHttpService) {}

  ngOnInit() {
    this.ShowLines();
  }

  changeselectedLine() {
    this.filteredLines.splice(0);
    this.redVoznjeInfo.Lines.forEach(element => {
      if (element.SerialNumber == this.selectedLine.SerialNumber) {
        this.filteredLines.push(element);
      }
    });
  }

  ShowLines(){
    this.http.getLines().subscribe(data => {
      console.log(data);
      this.lines = data;
    });
  }

  ispisPolaska() {
    console.log("IM HERE!");
    this.http
      .getSelected(
        this.selectedTimetableType,
        this.selectedDayType,
        1
        // this.selectedLine ----- POPRAVITI
      )
      .subscribe(data => {
        this.timetable.Times = data;
        console.log(this.timetable);
        err => console.log(err);
      });
  }
  onGetPrikazPolazaka() {
    this.http
      .GetPolasci(
        this.selectedLinija,
        this.selectedDan,
        this.selectedTeritorija
      )
      .subscribe(data => {
        this.polasci = data;
        err => console.log(data);
      });
  }
}
