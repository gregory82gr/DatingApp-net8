import { HttpClient } from '@angular/common/http';
import { computed, inject, Injectable, signal } from '@angular/core';
import { User } from '../_models/user';
import { map } from 'rxjs';
import { environment } from '../../environments/environment.development';
import { LikesService } from './likes.service';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  private likeSevice=inject(LikesService);

  private http=inject(HttpClient);
  baseUrl= environment.apiUrl;
  currentUser=signal<User | null>(null);
  roles=computed(()=>{
    const user=this.currentUser();
    if(user && user.token){
      const role= JSON.parse(atob(user.token.split('.')[1])).role;
      return Array.isArray(role)?role:[role];
    }
    return null;
  });

  login(model:any){
    return this.http.post<User>(this.baseUrl+'account/login',model).pipe(
      map((response:User)=>{
        const user=response;
        if(user){
          this.setCurrentUser(user);

        }
      })
    )
  }

  register(model:any){
    return this.http.post<User>(this.baseUrl+'account/register',model).pipe(
      map((response:User)=>{
        const user=response;
        if(user){
          this.setCurrentUser(user);
        }
        return user;
      })
    )
  }

  setCurrentUser(user:User){
    localStorage.setItem('user',JSON.stringify(user));
    this.currentUser.set(user);
    this.likeSevice.getLikeIds();
  }
  logout(){
    localStorage.removeItem('user');
    this.currentUser.set(null);
  }
}
