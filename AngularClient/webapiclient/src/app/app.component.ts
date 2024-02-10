import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterOutlet } from '@angular/router';
import { HttpClient, HttpClientModule, HttpHeaders } from '@angular/common/http';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, RouterOutlet, HttpClientModule],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  title = 'webapiclient';
  currentUser: any;
  allStudents: any;
  constructor(private _httpClient: HttpClient) {

  }
  private loginHeaders(): any {
    return {
      headers: new HttpHeaders({
        'Content-Type': 'application/json;',
        'Accept': 'application/json;',
      })
    };
  }
  private getHeaders(): any {
    return {
      headers: new HttpHeaders({
        'Content-Type': 'application/json;',
        'Accept': 'application/json;',
        'Authorization': 'bearer ' + this.currentUser?.token
      })
    };
  }
  getJWTToken(){
    let payload = {
      "username": "Venkat",
      "password": "Venkat123",
      "policy": "Local"
    };

    this._httpClient.post('https://localhost:7185/api/Login', payload, this.loginHeaders()).subscribe({
      //Success  
      next: (result: any) => {
        console.log(result);
        this.currentUser = result;
      },
      //Error
      error: (error: any) => {
        console.log(error);
      }
    })
  }
  getStudents() {
    this._httpClient.get('https://localhost:7185/api/Student/All', this.getHeaders()).subscribe({
      //Success  
      next: (result: any) => {
        this.allStudents = result;
        console.log(result);
      },
      //Error
      error: (error: any) => {
        console.log(error);
      }
    })
  }
  callMicrosoft() {
    this._httpClient.get('https://localhost:7185/api/Microsoft', this.getHeaders()).subscribe({
      //Success  
      next: (result: any) => {
        this.allStudents = result;
        console.log(result);
      },
      //Error
      error: (error: any) => {
        console.log(error);
      }
    })
  }
}
