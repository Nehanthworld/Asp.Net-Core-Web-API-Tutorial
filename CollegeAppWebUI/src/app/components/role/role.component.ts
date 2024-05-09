import { Component } from '@angular/core';
import { RoleService } from '../../services/role.service';

@Component({
  selector: 'app-role',
  templateUrl: './role.component.html',
  styleUrl: './role.component.css'
})
export class RoleComponent {
  roles: any;
  constructor(private _roleService: RoleService) {
    this.getRoles();
  }
  getRoles() {
    this._roleService.getRoles().subscribe({
      //Success  
      next: (result: any) => {
        this.roles = result.data;
        console.log(result);
      },
      //Error
      error: (error: any) => {
        console.log(error);
      }
    })
  }
}
