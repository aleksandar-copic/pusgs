import { Component, OnInit } from '@angular/core';
import { VerificateUserHttpService } from '../services/verificateUser.service';
import { FormBuilder } from '@angular/forms';
import { User } from '../classes/user';

@Component({
  selector: 'app-verificate-user',
  templateUrl: './verificate-user.component.html',
  styleUrls: ['./verificate-user.component.css']
})
export class VerificateUserComponent implements OnInit {

  infoForm = this.fb.group({
    Name: [''],
    Surname: [''],
    Address: [''],
    UserName: [''],
    Email: [''], 
    PhoneNumber: [''],
    Date: [''],
    Verificate : [''],
    UserType : [''],
    ImageUrl: ['']
  });

  constructor(private http: VerificateUserHttpService, private fb: FormBuilder){ }

  users: User[] = []
  odluka: string
  selectedUser: string
  approved: string
  selectedId: string
  imageToValidate = null;

  ngOnInit() {
    this.odluka = "";
    this.http.getUsers().subscribe((usersData) => {
      this.users = usersData;
      err => console.log(err);
    });
  }

  Prihvati(){
    this.http.odluka(this.selectedUser,"prihvati").subscribe((data) => {
      if(data.UserName == this.selectedUser)
      {
        alert("Uspesno ste odobrili profil");
      }
      else
      {
        alert("Doslo je do greske");
      }
      err => console.log(err);
    });
  }

  Odbij(){
    this.http.odluka(this.selectedUser,"odbij").subscribe((data) => {
      if(data.UserName == this.selectedUser)
      {
        alert("Uspesno ste odbili profil");
      }
      else
      {
        alert("Doslo je do greske");
      }
      err => console.log(err);
    });
  }

  PrikaziInfo(){
    this.users.forEach(element => {
      if(element.UserName == this.selectedUser)
      {
        this.selectedId = element.Id
        console.log(this.selectedId);
      }
    });
    this.http.getSelectedUser(this.selectedId).subscribe((data) => {

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

      this.infoForm.patchValue({ Name : data.Name, Surname: data.Surname, 
        Email: data.Email, Address : data.Address, PhoneNumber : data.PhoneNumber, 
        Date : data.Date, Verificate : this.approved, 
        UserType : data.UserType , ImageUrl: "~/~/~/WebApp/WebApp/UploadFile" + data.ImageUrl})

      err => console.log(err);
    });
    
    this.http.downloadImage(this.selectedId).subscribe(
      data => {
        this.imageToValidate = 'data:image/jpeg;base64,' + data;
      }, error => {
        console.log(error);
        this.imageToValidate = null;
      }
    )    
  }

}
