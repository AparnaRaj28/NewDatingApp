import { Component, OnInit, Output } from '@angular/core';
import { Member } from 'src/app/models/member';
import { MemberService } from 'src/app/services/member.service';

@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrls: ['./member-list.component.css']
})
export class MemberListComponent implements OnInit
{
  
  members:Member[] =[];

  constructor(private memberService:MemberService) {
    
    
  }

  ngOnInit(): void 
  {
    this.loadMembers();
  }
  loadMembers()
  {
    this.memberService.getMembers().subscribe({
      next: members=>{
        this.members = members;
      },
      error:error=>console.log(error)
    })

  }
}
