import { inject, Injectable, signal } from '@angular/core';
import { environment } from '../../environments/environment';
import { HubConnection, HubConnectionBuilder, HubConnectionState } from '@microsoft/signalr';
import { ToastrService } from 'ngx-toastr';
import { User } from '../_models/user';
import { state } from '@angular/animations';

@Injectable({
  providedIn: 'root'
})
export class PresenceService {

  hubsUrl=environment.hubsUrl;
  private hubConnection?: HubConnection;
  private toastr=inject(ToastrService);
  onlineUsers= signal<string[]>([]);

  createHubConnection(user:User){
    this.hubConnection = new HubConnectionBuilder()
      .withUrl(this.hubsUrl + 'presence', {
        accessTokenFactory: () => user.token
      })
      .withAutomaticReconnect()
      .build();

    this.hubConnection
      .start().catch(error => {
        console.log(error);
        this.toastr.error(error.message, 'Error starting connection');
      });

    this.hubConnection.on('UserIsOnline', username => {
      this.toastr.info(username + ' has connected ');
    });
    this.hubConnection.on('UserIsOffline', username => {
      this.toastr.warning(username + ' has disconnected ');
    });
    this.hubConnection.on('GetOnlineUsers', (usernames: string[]) => {
      this.onlineUsers.set(usernames);
    });
  }

  stopHubConnection() {
    if (this.hubConnection?.state === HubConnectionState.Connected) {
      this.hubConnection.stop().catch(error => {
        console.log(error);
        this.toastr.error(error.message, 'Error stopping connection');
      });
    }
  }
  constructor() { }
}
