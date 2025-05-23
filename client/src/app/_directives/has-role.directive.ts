import { Directive, inject, Input, OnInit, TemplateRef, ViewContainerRef } from '@angular/core';
import { AccountService } from '../_services/account.service';

@Directive({
  selector: '[appHasRole]'
})
export class HasRoleDirective implements OnInit {


  @Input('appHasRole') appHasRole: string[] = [];
  private accountService = inject(AccountService);
  private ViewCntainerREF = inject(ViewContainerRef);
  private templateRef = inject(TemplateRef);

  ngOnInit(): void {
    if (this.accountService.roles()?.some((r:string) => this.appHasRole.includes(r))) {
      this.ViewCntainerREF.createEmbeddedView(this.templateRef);
    } else {
      this.ViewCntainerREF.clear();
    }

  }

}
