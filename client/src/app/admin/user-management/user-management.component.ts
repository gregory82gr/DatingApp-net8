import { Component, inject, OnInit } from '@angular/core';
import { AdminService } from '../../_services/admin.service';
import { User } from '../../_models/user';
import { RolesModalComponent } from '../../modals/roles-modal/roles-modal.component';
import { BsModalRef, BsModalService, ModalOptions } from 'ngx-bootstrap/modal';
import { Title } from '@angular/platform-browser';

@Component({
  selector: 'app-user-management',
  imports: [],
  templateUrl: './user-management.component.html',
  styleUrl: './user-management.component.css'
})
export class UserManagementComponent implements  OnInit{

  private adminService = inject(AdminService); // Assuming you have an AdminService for user management
 private modalService = inject(BsModalService); // Injecting the modal service to open modals
  users: User[] = []; // This will hold the list of users
  bsModalRef: BsModalRef<RolesModalComponent>=new BsModalRef<RolesModalComponent>();

  ngOnInit(): void {
    // Initialization logic can go here
    this.getUsersWithRoles();
  }


  openRolesModal() {
   const initialState:ModalOptions= {
      class: 'modal-lg',
      initialState: {
        Title: 'User Roles ',
        list:['Admin', 'Moderator', 'Member']
      }
    }
    this.bsModalRef = this.modalService.show(RolesModalComponent, initialState);
  }
  // Add methods to handle user management actions, such as fetching users, updating user roles, etc.
  // getUsersWithRoles() {
  //   this.adminService.getUsersWithRoles().subscribe({
  //     next: users => this.users = users
  //   });
  // }

  getUsersWithRoles() {
    this.adminService.getUsersWithRoles()
      .subscribe({
        next: rawUsers => {
          // rawUsers has a `username` field, not `userName`
          this.users = rawUsers.map(u => ({
            ...u,
            userName: (u as any).username    // pull `username` into your interface’s `userName`
          } as User));
        },
        error: err => console.error(err)
      });
  }



}
