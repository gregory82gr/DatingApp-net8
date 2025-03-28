import { Component, inject } from '@angular/core';
import {FormsModule} from '@angular/forms'
import { AccountService } from '../_services/account.service';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { Router, RouterLink, RouterLinkActive } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { TitleCasePipe } from '@angular/common';
@Component({
  selector: 'app-nav',
  imports: [FormsModule,BsDropdownModule,RouterLink,RouterLinkActive,TitleCasePipe],
  templateUrl: './nav.component.html',
  styleUrl: './nav.component.css'
})
export class NavComponent {
  accountService=inject(AccountService);
  private router =inject(Router);
  private toastr=inject(ToastrService);

  loggedIn=false;
  model:any={};

  login(){
    this.accountService.login(this.model).subscribe(
      response => {
      this.router.navigateByUrl('/members');
      this.loggedIn=true;
    },error=>{
      this.toastr.error(error.error);
    });
    console.log(this.model);
  }

  logout(){
    this.accountService.logout();
    this.router.navigateByUrl('/');
  }
}
