import { Component, inject, OnInit, ViewChild } from '@angular/core';
import { MembersService } from '../../_services/members.service';
import { ActivatedRoute } from '@angular/router';
import { Member } from '../../_models/member';
import { TabDirective, TabsetComponent, TabsModule } from 'ngx-bootstrap/tabs';
import {  GalleryItem, GalleryModule, ImageItem ,Gallery} from 'ng-gallery';
import { TimeagoModule } from 'ngx-timeago';
import { DatePipe } from '@angular/common';
import { MemberMessagesComponent } from '../member-messages/member-messages.component';
import { Message } from '../../_models/message';
import { MessageService } from '../../_services/message.service';

@Component({
  selector: 'app-member-detail',
  imports: [TabsModule,GalleryModule,TimeagoModule,DatePipe,MemberMessagesComponent],
  templateUrl: './member-detail.component.html',
  styleUrl: './member-detail.component.css'
})
export class MemberDetailComponent implements OnInit {
  @ViewChild('memberTabs') memberTabs?: TabsetComponent;
 private memberService=inject(MembersService);
 private messageService=inject(MessageService);
 private route=inject(ActivatedRoute);
 member?: Member ;
 images:GalleryItem[]=[];
 activeTab?: TabDirective;
 messages:Message[] = [];



 constructor(private gallery: Gallery) {}
  ngOnInit(): void {
    this.loadMember();
    console.log('Gallery images:', this.images);
  }

  onTabActivated(data: TabDirective) {
    this.activeTab = data;
    if (this.activeTab.heading === 'Messages' && this.member && this.messages.length === 0) {
      this.messageService.getMessageThread(this.member.userName).subscribe({
        next: (messages) => {
          this.messages = messages;
        },
        error: (error) => {
          console.error(error);
        }
      });
    }
  }
  loadMember() {
    const username = this.route.snapshot.paramMap.get('username');
    if (username) {
      this.memberService.getMember(username).subscribe(member => {
        this.member = member;
        member.photos.map(p=>{
          this.images.push(new ImageItem({src:p.url, thumb:p.url}))
        })
      }, error => {
        console.error(error);
      });
    }
  }
}
