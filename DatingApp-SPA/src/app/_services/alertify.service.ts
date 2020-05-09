import { Injectable } from '@angular/core';
import * as alertify from 'alertifyjs';

@Injectable({
  providedIn: 'root'
})
export class AlertifyService {

constructor() { }

confirm(message: string, okCallback: () => any){
  alertify.confirm(message, (e: any) => {
    if (e)
    {
      okCallback();
    } else {}
  });
}

succes(message: string){
  alertify.succes(message);
}

error(message: string){
  alertify.error(message);
}

warning(message: string){
  alertify.warning(message);
}

message(message: string){
  alertify.message(message);
}
}
