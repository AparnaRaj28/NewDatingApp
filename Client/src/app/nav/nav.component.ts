import { Component, OnInit } from '@angular/core';
import { Route, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr'
import { AccountService } from '../services/account.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit{
  ngModel:any={}
  // isLoggedIn = false;
  //we assign a value to the observalble and in this case its an observable of null
  // currentUser$:Observable<User | null> =of(null)
  

   constructor(public accountService:AccountService,private router:Router
               ,private toastr:ToastrService){}

  ngOnInit(): void {
    // this.getCurrentUser();
    // this.currentUser$ = this.accountService.currentUser$;
  }


  //  getCurrentUser(){
  //   this.accountService.currentUser$.subscribe({
  //     next:user=>this.isLoggedIn = !!user,
  //     error:error =>console.log(error)
  //   })
  //  }

  login(){
    this.accountService.login(this.ngModel).subscribe({
      next: response=>{ 
                console.log(response)
                this.router.navigateByUrl("/members")
                //  this.isLoggedIn = true;
                 },
      error: error=>{
        //error are handled in the interceptor
        //  this.toastr.error(error.error)
        console.log(error)
      },     
    }
    );
  }

  logout(){
    this.accountService.logout(); //removes the item from local storage
    // this.isLoggedIn = false;
    this.router.navigateByUrl("/");
    
  }
}
