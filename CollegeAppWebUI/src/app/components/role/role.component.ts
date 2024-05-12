import { Component } from '@angular/core';
import { RoleService } from '../../services/role.service';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { FormGroup, FormControl } from '@angular/forms';

@Component({
  selector: 'app-role',
  templateUrl: './role.component.html',
  styleUrl: './role.component.css'
})
export class RoleComponent {
  roles: any;
  closeResult: any;
  roleForm = new FormGroup({
    id: new FormControl(0),
    roleName: new FormControl(''),
    description: new FormControl(''),
    isActive: new FormControl(false),
  });
  constructor(private _roleService: RoleService, private modalService: NgbModal) {
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
  edit(content: any, roleData: any) {
    this.roleForm.patchValue({
      id: roleData.id,
      roleName: roleData.roleName,
      description: roleData.description,
      isActive: roleData.isActive
    });
    this.modalService.open(content, { centered: true, size: 'lg' });
  }
  delete(id: number){
    this._roleService.deleteRole(id).subscribe({
      //Success  
      next: (result: any) => {
        if (result.status)
          alert("Role Deleted.");
        else
          alert("Unable to save");
        this.getRoles();
        console.log(result);
      },
      //Error
      error: (error: any) => {
        console.log(error);
      }
    })
  }
  Save() {
    if (this.roleForm.value.id == 0) {
      this._roleService.createRole({ ...this.roleForm.value }).subscribe({
        //Success  
        next: (result: any) => {
          if (result.status)
            alert("Role Created.");
          else
            alert("Unable to Create");
          this.getRoles();
          console.log(result);
        },
        //Error
        error: (error: any) => {
          console.log(error);
        }
      })
    }
    else {
      this._roleService.updateRole({ ...this.roleForm.value }).subscribe({
        //Success  
        next: (result: any) => {
          if (result.status)
            alert("Role Saved.");
          else
            alert("Unable to save");
          this.getRoles();
          console.log(result);
        },
        //Error
        error: (error: any) => {
          console.log(error);
        }
      })
    }
    // console.log(this.roleForm.value)
    // console.log("Saved.");
    this.modalService.dismissAll();
  }
}
