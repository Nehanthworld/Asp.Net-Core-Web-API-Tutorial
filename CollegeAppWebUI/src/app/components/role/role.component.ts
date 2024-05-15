import { Component } from '@angular/core';
import { RoleService } from '../../services/role.service';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { FormGroup, FormControl } from '@angular/forms';
import { NotificationService } from '../../services/common/notification.service';

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
  constructor(private _roleService: RoleService, private modalService: NgbModal,
    private _notificationService: NotificationService) {
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
  delete(id: number) {
    this._notificationService.deleteConfirmation("Are you sure, you want to delete the role", "Delete Confirmation",
      () => {
        this._roleService.deleteRole(id).subscribe({
          //Success  
          next: (result: any) => {
            if (result.status)
              this._notificationService.successMessage("Role deleted Successfully", "Deleted");
            else
              this._notificationService.errorMessage("Unable to delete role", "Error");
            this.getRoles();
            console.log(result);
          },
          //Error
          error: (error: any) => {
            console.log(error);
          }
        })
      });

  }
  Save() {
    if (this.roleForm.value.id == 0) {
      this._roleService.createRole({ ...this.roleForm.value }).subscribe({
        //Success  
        next: (result: any) => {
          if (result.status)
            this._notificationService.successMessage("Role created Successfully", "Created");
          else
            this._notificationService.errorMessage("Unable to create role", "Error");
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
            this._notificationService.successMessage("Role updated Successfully", "Updated");
          else
            this._notificationService.errorMessage("Unable to update role", "Error");
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
