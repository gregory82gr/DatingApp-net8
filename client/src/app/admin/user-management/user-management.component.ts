import { Component, inject, OnInit } from '@angular/core';
import { AdminService } from '../../_services/admin.service';
import { User } from '../../_models/user';

@Component({
  selector: 'app-user-management',
  imports: [],
  templateUrl: './user-management.component.html',
  styleUrl: './user-management.component.css'
})
export class UserManagementComponent implements  OnInit{

  private adminService = inject(AdminService); // Assuming you have an AdminService for user management

  users: User[] = []; // This will hold the list of users

  ngOnInit(): void {
    // Initialization logic can go here
    this.getUsersWithRoles();
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
