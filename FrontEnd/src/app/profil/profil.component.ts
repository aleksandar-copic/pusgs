import { Component, OnInit } from '@angular/core';
import { Validators, FormBuilder } from '@angular/forms';
import { ProfilHttpService } from '../services/profil.service';
import { User } from '../classes/user';

@Component({
  selector: 'app-profil',
  templateUrl: './profil.component.html',
  styleUrls: ['./profil.component.css']
})
export class ProfilComponent implements OnInit {

  updateForm = this.fb.group({
    Name: ['', Validators.required],
    Surname: ['', Validators.required],
    Address: ['', Validators.required],
    Email: ['', Validators.required], 
    PhoneNumber: ['', Validators.required],
    Date: ['', Validators.required],
    Verificate : [''],
    UserType : [''],
    ImageUrl: ['']
  });

  user: User = new User();
  approved: string
  message:string
  constructor(private fb: FormBuilder,private http: ProfilHttpService) { }

  ngOnInit() {
    this.http.getUser().subscribe((data) => {
      this.user = data;

      if (data.VerificateAcc == "0") {
        this.approved = "Na cekanju";
      } 
      else if(data.Verificate == "1"){
        this.approved = "Odobren";
      }
      else
      {
        this.approved = "Nije odobren";
      }

      if (data.TypeId == "1") {
        data.UserType = "Djak/Student";
      }
      else if (data.TypeId == "2") {
        data.UserType = "Penzioner";
      }
      else if (data.TypeId == "3") {
        data.UserType = "Regularan putnik";
      }
      else {
        data.UserType = "Nepoznato";
      }

      this.updateForm.patchValue({ Name : data.Name, Surname: data.Surname, 
        Email: data.Email, Address : data.Address, PhoneNumber : data.PhoneNumber, 
        Date : data.Date, Verificate : this.approved, 
        UserType : data.UserType , ImageUrl: data.ImageUrl})

      err => console.log(err);
    });
  }

  jwtIsController() : boolean{
    return localStorage.getItem('role') == "Controller"
  }

  jwtIsAdmin() : boolean{
    return localStorage.getItem('role') == "Admin"
  }

  UpdateUser() {
    this.http.updateUser(this.updateForm.value).subscribe((userProfileInfo) =>  {
      if (userProfileInfo == "uspesno") {
        //this.updateForm.reset();
        this.message = "Uspesno ste izmenili profil";
      } 
      else {
        err => console.log("Greska pri izmeni profila");
        this.message = "GresGreska pri izmeni profila";
      }
    });
  }

}
