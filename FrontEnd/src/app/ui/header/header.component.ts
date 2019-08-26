import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit {

  constructor() { }

  jwtIsUndefined() : boolean{
    return localStorage.getItem('jwt') != "null" && localStorage.getItem('jwt') != "undefined" && localStorage.getItem('jwt') != "";
  }

  logout(){
    localStorage.jwt = undefined;
    localStorage.loggedUser = undefined;
    localStorage.role = undefined;
  }

  jwtIsAdmin() : boolean{
    return localStorage.getItem('role') == "Admin"
  }

  jwtIsController() : boolean{
    return localStorage.getItem('role') == "Controller"
  }

  ngOnInit() {
  }

}
