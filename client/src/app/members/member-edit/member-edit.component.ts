import { Component, inject, OnInit, ViewChild } from '@angular/core';
import { Member } from '../../_models/member';
import { MembersService } from '../../_services/members.service';
import { AccountService } from '../../_services/account.service';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { FormsModule, NgForm } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-member-edit',
  imports: [TabsModule,FormsModule],
  templateUrl: './member-edit.component.html',
  styleUrl: './member-edit.component.css'
})
export class MemberEditComponent implements OnInit {
  @ViewChild('editForm') editForm?: NgForm;
  member?: Member;
  private memberService = inject(MembersService);
  private accountService = inject(AccountService);
  private toastr = inject(ToastrService);


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

  updateMember() {
    if (this.member) {
      console.log('Updated member:', this.member);
      this.toastr.success('Profile updated successfully');
      this.editForm?.reset(this.member);

    }
  }



}
