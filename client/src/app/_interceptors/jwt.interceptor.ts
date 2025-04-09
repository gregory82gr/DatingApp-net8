import { HttpInterceptorFn } from '@angular/common/http';
import { AccountService } from '../_services/account.service';
import { inject } from '@angular/core';

export const jwtInterceptor: HttpInterceptorFn = (req, next) => {
  const accountService = inject(AccountService);
  const currentUser = accountService?.currentUser();
  if (currentUser ) {
    req = req.clone({
      setHeaders: {
        Authorization: `Bearer ${currentUser?.token}`
      }
    });
  }
  
  return next(req);
};
