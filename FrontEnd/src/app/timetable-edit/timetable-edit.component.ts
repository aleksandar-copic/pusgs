import { Component, OnInit } from '@angular/core';
import { TimetableEditHttpService } from '../services/timeTableEdit.service';
import { Timetable } from '../classes/timetable';
import { FormBuilder, Validators } from '@angular/forms';
import { Line } from '../classes/line';
import { Router } from '@angular/router';

@Component({
  selector: 'app-timetable-edit',
  templateUrl: './timetable-edit.component.html',
  styleUrls: ['./timetable-edit.component.css']
})
export class TimetableEditComponent implements OnInit {

  timetableAdd = this.fb.group({
    TimetableTypeId: ['', Validators.required],
    DayTypeId: ['', Validators.required],
    LineId: ['', Validators.required],
    Times: ['', Validators.required]
  });

  timetableEdit = this.fb.group({
    Id: ['', Validators.required],
    TimetableTypeId: ['', Validators.required],
    DayTypeId: ['', Validators.required],
    LineId: ['', Validators.required],
    Times: ['', Validators.required]
  });

  allTimetables: Timetable[]
  TimetableTypeId: number
  DayTypeId: number
  LineId: number
  TimeTableId: number
  Times: string
  selectedType: string
  selectedDayType: string
  selectedLine: string
  selectedTimetable: Timetable = new Timetable()
  allLines: string[]
  dayTypes: string[] = ["Work day", "Saturday", "Sunday"]
  types: string[] = ["Urban", "Suburban"]

  constructor(private fb: FormBuilder, private http: TimetableEditHttpService, private router: Router) { }

  ngOnInit() {
    this.selectedDayType = this.dayTypes[0]
    this.selectedType = this.types[0]
    this.http.getAll().subscribe((item) => {
      this.allTimetables = item 
      err => console.log(err);
    });

    this.http.getAllLines().subscribe((item) => {
      this.allLines = item 
      this.selectedLine = this.allLines[0]
      err => console.log(err);
    });
  }

  TimetableAdd(){
    if(this.selectedType === "Urban"){
      this.TimetableTypeId = 1
    }
    else if(this.selectedType === "Suburban"){
      this.TimetableTypeId = 2
    }
    
    if(this.selectedDayType === "Work day"){
      this.DayTypeId = 1
    }
    else if(this.selectedDayType === "Saturday"){
      this.DayTypeId = 2
    }
    else if(this.selectedDayType === "Sunday"){
      this.DayTypeId = 3
    }

    this.LineId = parseInt(this.selectedLine)

    this.timetableAdd.patchValue({TimetableTypeId: this.TimetableTypeId, DayTypeId: this.DayTypeId, LineId: this.LineId})

    this.http.addTimetable(this.timetableAdd.value).subscribe((item) =>  {
      if (item == "successfull") {
        alert("Successfully added timetable.")
        this.router.navigate(["/home"]);
      } 
      else {
        err => console.log("Greska pri izmeni profila");
      }
    });
   }

   Izaberi(){
    if(this.selectedType === "Urban"){
      this.TimetableTypeId = 1
    }
    else if(this.selectedType === "Suburban"){
      this.TimetableTypeId = 2
    }
    
    if(this.selectedDayType === "Work day"){
      this.DayTypeId = 1
    }
    else if(this.selectedDayType === "Saturday"){
      this.DayTypeId = 2
    }
    else if(this.selectedDayType === "Sunday"){
      this.DayTypeId = 3
    }

    this.TimeTableId = this.selectedTimetable.Id
    this.LineId = parseInt(this.selectedLine)
    this.timetableEdit.patchValue({Id: this.TimeTableId, TimetableTypeId: this.TimetableTypeId, DayTypeId: this.DayTypeId, LineId: this.LineId, Times: this.selectedTimetable.Times})
  }

   Izmeni(){
    this.http.updateTimetable(this.timetableEdit.value).subscribe((item) =>  {
      if (item == "uspesno") {
        alert("Successfully modified timetable.")
        this.router.navigate(["/home"]);
      } 
      else {
        err => console.log("Greska pri izmeni profila");
      }
    });
   }

   Obrisi(){
    this.http.deleteTimetable(this.timetableEdit.value).subscribe((item) =>  {
      if (item == "uspesno") {
        alert("Successfully deleted timetable.")
        this.router.navigate(["/home"]);
      } 
      else {
        err => console.log("Greska pri izmeni profila");
      }
    });
   }
}
