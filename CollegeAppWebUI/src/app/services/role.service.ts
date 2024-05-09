import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Injectable } from "@angular/core";

@Injectable({ providedIn: 'root' })
export class RoleService {

    constructor(private _httpClient: HttpClient) { }
    getRoles() {
        return this._httpClient.get('https://localhost:7185/api/Role/All', this.getHeaders());
    }
    private loginHeaders(): any {
        return {
            headers: new HttpHeaders({
                'Content-Type': 'application/json;',
                'Accept': 'application/json;',
            })
        };
    }
    private getHeaders(token?: string): any {
        return {
            headers: new HttpHeaders({
                'Content-Type': 'application/json;',
                'Accept': 'application/json;',
                'Authorization': 'bearer ' + token
            })
        };
    }
}