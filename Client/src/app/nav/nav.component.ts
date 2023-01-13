import { Component, OnInit } from '@angular/core';
import { AccountService } from '../services/account.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit{
  ngModel:any={}
  isLoggedIn = false;
   constructor(private accountService:AccountService){}

  ngOnInit(): void {
    this.getCurrentUser();
  }


   getCurrentUser(){
    this.accountService.currentUser$.subscribe({
      next:user=>this.isLoggedIn == !!user,
      error:error =>console.log(error)
    })
   }

  login(){
    this.accountService.login(this.ngModel).subscribe({
      next: response=>{ 
                console.log(response)
                 this.isLoggedIn = true;
                 },
      error: error=>{console.log(error)},     
    }
    );
  }

  logout(){
    this.accountService.logout();
    this.isLoggedIn = false;
    
  }
}
