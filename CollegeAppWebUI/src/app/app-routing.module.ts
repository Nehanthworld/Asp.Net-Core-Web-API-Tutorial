import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { StudentComponent } from './components/student/student.component';
import { RoleComponent } from './components/role/role.component';
import { UserComponent } from './components/user/user.component';
import { RolePrivilegeComponent } from './components/role-privilege/role-privilege.component';

const routes: Routes = [
  { path: 'students', component: StudentComponent, pathMatch: 'full' },
  { path: 'roles', component: RoleComponent, pathMatch: 'full' },
  { path: 'role-privileges/:id', component: RolePrivilegeComponent, pathMatch: 'full' },
  { path: 'users', component: UserComponent, pathMatch: 'full' },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
