import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Member } from '../models/member';

@Injectable({
  providedIn: 'root'
})
export class MemberService {

  constructor(private http:HttpClient) { }
  baseUrl = environment.apiUrl;
  getMembers()
  {
    return this.http.get<Member[]>(this.baseUrl+'users');  
  }
  getMember(username:string)
  {
     //return this.http.get<Member>(this.baseUrl+'users/'+username,this.getHttpOptions());
     return this.http.get<Member>(this.baseUrl+'users/'+username);
  }
  //for passing the authoization token for http requests
  //we need options which include authorization headers
  // getHttpOptions(){
  //   const userString = localStorage.getItem('user');
  //   if(!userString) return;
  //   const user = JSON.parse(userString);
  //   return {
  //     headers : new HttpHeaders({
  //       Authorization : 'Bearer '+user.token
  //     })
  //   } 
  // }
}
