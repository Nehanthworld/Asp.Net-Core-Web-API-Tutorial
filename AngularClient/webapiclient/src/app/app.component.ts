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
  constructor(private _httpClient: HttpClient) {

  }
  private getHeaders(): any {
    return {
      headers: new HttpHeaders({
        'Content-Type': 'application/json;',
        'Accept': 'application/json;',
        'myheader': 'Venkat'
      })
    };
  }
  getStudents() {
    this._httpClient.get('https://localhost:7185/api/Student/All', this.getHeaders()).subscribe({
      //Success  
      next: (result: any) => {
        console.log(result);
      },
      //Error
      error: (error: any) => {
        console.log(error);
      }
    })
  }
}
