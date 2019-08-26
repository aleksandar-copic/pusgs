import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { NgForm, FormBuilder, Validators } from '@angular/forms';
import { User } from '../classes/user';
import { AuthHttpService } from '../services/http/auth.service' 

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {

  registacijaForm = this.fb.group({
    Name: ['', Validators.required],
    Surname: ['', Validators.required],
    UserName: ['', Validators.required],
    Password: ['', Validators.required],
    ConfirmPassword: ['', Validators.required],
    Email: ['', Validators.required],
    Address: ['', Validators.required],
    PhoneNumber: ['', Validators.required],
    ImageUrl: [''],
    Date: ['', Validators.required]
  });
  
  selectedFile: File = null;
  onFileSelected(event) {
    this.selectedFile = <File>event.target.files[0];
  }

  constructor(private http:AuthHttpService, private router: Router, private fb: FormBuilder) { }

  ngOnInit() {
  }

  register(){
    let regModel: User = this.registacijaForm.value;
    let formData: FormData = new FormData();

    if (this.selectedFile != null) {
      formData.append('ImageUrl', this.selectedFile, this.selectedFile.name);
    }

    this.http.register(regModel).subscribe(temp => {
      if(temp == "uspesno")
      {
        if (this.selectedFile != null) {
          this.http.uploadImage(formData, regModel.UserName).subscribe(ret => {
            alert("Unseccesfull!!!");
            this.router.navigate(["/home"]);
          },
            err => console.log(err));
        }
        else {
          alert("Succesfully registered!");
          this.router.navigate(["/login"]);
        }
      }
      else if(temp == "neuspesno")
      {
        console.log(temp);
        this.router.navigate(["/login"])
      }
    });
  }

  public imagePath;
  imgURL: any;
  public msg: string;
 
  preview(files) {
    if (files.length === 0)
      return;
 
    var mimeType = files[0].type;
    if (mimeType.match(/image\/*/) == null) {
      this.msg = "Only images are supported.";
      return;
    }
 
    var reader = new FileReader();
    this.imagePath = files;
    reader.readAsDataURL(files[0]); 
    reader.onload = (_event) => { 
      console.log(reader);
      this.imgURL = reader.result; 
    }
  }

}
