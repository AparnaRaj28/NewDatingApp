import { Component, EventEmitter, Output } from '@angular/core';
import { NgModel } from '@angular/forms';
import { ToastrService } from 'ngx-toastr'
import { AccountService } from '../services/account.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent {
   
  @Output('cancel') cancelRegister = new EventEmitter();
  model:any={};
 /**
  *
  */
 constructor(private accountService:AccountService,private toastr:ToastrService) {

 }
  OnSubmitForm(){
   this.register();
  }

  cancelForm(){
   this.cancelRegister.emit(false);
  }

  register(){
    this.accountService.register(this.model).subscribe({
      next:response=>{
          console.log(response);        
          window.alert("User successfully registered!")
          this.cancelForm();
        },
      error:error=>{
        console.log(error)
        this.toastr.error(error.errors)
      }
    })
  }
}
