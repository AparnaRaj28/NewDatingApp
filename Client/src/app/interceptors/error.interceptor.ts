import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
  HttpErrorResponse
} from '@angular/common/http';
import { catchError, Observable } from 'rxjs';
import { NavigationExtras, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {

  constructor(private router:Router,private toastr:ToastrService) {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    return next.handle(request).pipe(
      //cath any errors from the interceptor
      catchError((errorRes:HttpErrorResponse)=>{
        //check if error exists
        if(errorRes){
          switch(errorRes.status)
          {
            //there are two 400 rquests- bad request and validation error
            //validation errors comes inside an object called "error" nad the actual error is in another object called "errors"
            case 400:
              if(errorRes.error.errors){
                // errorRes.error.errors is an array objects
                const modelStateErrors =[];
                //looping over the errors
                for(const key in errorRes.error.errors){
                  //check if there exists anything in errorRes.error.errors
                  if(errorRes.error.errors[key]){
                    modelStateErrors.push(errorRes.error.errors[key]) //this builds an array of errors for validation errors
                  }
                }
                throw modelStateErrors.flat();//falt is used to transform the array into a single array.
              }
              else{
                this.toastr.error(errorRes.error,errorRes.status.toString())
              }
              break;
            case 401:
              this.toastr.error("Unauthorised:",errorRes.status.toString())
              break;
            case 404:
              this.router.navigateByUrl("/not-found");
              break;
            case 500:
              //the component needs access to the error response
              const navigationExtras:NavigationExtras = {state:{errorRes:errorRes.error}};
              this.router.navigateByUrl('/server-error',navigationExtras)
              break;
            default: 
                this.toastr.error("Something unexpected happened.Try again later");
                console.log(errorRes);
                break;
          }
        }
        throw errorRes;
      })
    )
  }
}
