import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { AccountService } from '../services/account.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit{
  registerMode=false;
  users:any;
 ngOnInit(){
  this.getUsers();
 }
  constructor(private http:HttpClient) {
  
  }
  toggleRegisterMode(){
    this.registerMode = !this.registerMode
  }
  cancelFromRegister(event:boolean){
   this.registerMode = event;
  }
  getUsers(){
    this.http.get('https://localhost:5001/api/users')
    .subscribe({
      next: response=>{
                      this.users=response;
                      console.log(response)
                      },
      error: error=>{console.log(error)},
      complete:()=>console.log("Request is completed")
    })
  }
}
