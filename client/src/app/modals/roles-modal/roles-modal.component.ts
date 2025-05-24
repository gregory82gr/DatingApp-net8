import { Component, inject } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';

@Component({
  selector: 'app-roles-modal',
  imports: [],
  templateUrl: './roles-modal.component.html',
  styleUrl: './roles-modal.component.css'
})
export class RolesModalComponent {

  bsModalRef=inject(BsModalRef);
  username=''
  title='User Roles';
  availableRoles: string[] = [];
  selectedRoles: string[] = [];
  rolesUpdated = false;

  updateChecked(ckeckedValue: string) {
    if(this.selectedRoles.includes(ckeckedValue)) {
      this.selectedRoles = this.selectedRoles.filter(role => role !== ckeckedValue);
    }else{
      this.selectedRoles.push(ckeckedValue);
    }
  }

  onSelectRoles() {
    this.rolesUpdated = true;
    this.bsModalRef.hide();
  }
}
