
export interface Role {
    id: number;
    roleName: string;
    description: string;
    active: boolean;
}

export interface RolePrivilege {
    id: number;
    rolePrivilegeName: string;
    description: string;
    isActive: boolean;
    roleId: number;
}

