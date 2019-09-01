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
  selectedTimetableType: TimetableType = new TimetableType();
  selectedDayType: DayType = new DayType();
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
    // this.http.getAll().subscribe(redVoznjeInfo => {
    //   this.redVoznjeInfo = redVoznjeInfo;
    //   err => console.log(err);
    // });
  }

  changeselectedLine() {
    this.filteredLines.splice(0);
    this.redVoznjeInfo.Lines.forEach(element => {
      if (element.SerialNumber == this.selectedLine.SerialNumber) {
        this.filteredLines.push(element);
      }
    });
  }

  ShowLines(routeType : number){
    this.http.getLines(routeType).subscribe(data => {
      console.log(data);
      this.lines = data;
    });
  }

  ispisPolaska() {
    this.http
      .getSelected(
        this.selectedTimetableType.Id,
        this.selectedDayType.Id,
        this.selectedLine.Id
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
