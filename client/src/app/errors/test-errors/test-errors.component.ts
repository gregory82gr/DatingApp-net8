import { HttpClient } from '@angular/common/http';
import { Component, inject } from '@angular/core';
import { environment } from '../../../environments/environment.development';

@Component({
  selector: 'app-test-errors',
  imports: [],
  templateUrl: './test-errors.component.html',
  styleUrl: './test-errors.component.css'
})
export class TestErrorsComponent {
  baseUrl = environment.apiUrl;
  private http=inject(HttpClient);
  validationErrors:string[]=[];

  get400Error(){
    this.http.get(this.baseUrl+'buggy/bad-request').subscribe((response: any)=>{
      console.log(response);
    },(error: any)=>{
      console.log(error);
    });
  }

  get401Error(){
    this.http.get(this.baseUrl+'buggy/auth').subscribe((response: any)=>{
      console.log(response);
    },(error: any)=>{
      console.log(error);
    });
  }

  get404Error(){
    this.http.get(this.baseUrl+'buggy/not-found').subscribe((response: any)=>{
      console.log(response);
    },(error: any)=>{
      console.log(error);
    });
  }

  get500Error(){
    this.http.get(this.baseUrl+'buggy/server-error').subscribe((response: any)=>{
      console.log(response);
    },(error: any)=>{
      console.log(error);
    });
  }

  get400ValidationError(){
    this.http.post(this.baseUrl+'account/register',{}).subscribe((response: any)=>{
      console.log(response);
    },(error: any)=>{
      console.log(error);
      this.validationErrors=error;
    });
  }

}
