import { Component, inject, OnInit } from '@angular/core';
import { Member } from '../../_models/member';
import { MembersService } from '../../_services/members.service';
import { AccountService } from '../../_services/account.service';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-member-edit',
  imports: [TabsModule,FormsModule],
  templateUrl: './member-edit.component.html',
  styleUrl: './member-edit.component.css'
})
export class MemberEditComponent implements OnInit {
  member?: Member;
  private memberService = inject(MembersService);
  private accountService = inject(AccountService);


  ngOnInit(): void {
    this.loadMember();

  }

  loadMember() {
      const user =this.accountService.currentUser();
      if(!user) return;
      console.log('Member:', user);
      if (user.userName) {
        this.memberService.getMember(user.userName).subscribe(member => {
          this.member = member;
          console.log('Member:', member);
        }, error => {
          console.error(error);
        });
      }
  }


}
