import { CanActivateFn } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from '../_services/account.service';
import { inject } from '@angular/core';

export const adminGuard: CanActivateFn = (route, state) => {
  const accountService = inject(AccountService);
  const toastr= inject(ToastrService);

  if (accountService.roles()?.includes('Admin') || accountService.roles()?.includes('Moderator')) {
    return true;
  }else{
    toastr.error('You are not allowed to enter this area');
    return false;
  }
};
