namespace CollegeApp.Data
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string PasswordSalt { get; set; }
        public int UserTypeId { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public virtual UserType UserType { get; set; }
        public virtual ICollection<UserRoleMapping> UserRoleMappings { get; set; }
        //Add-Migration <name>
        //Update-Database
        //Remove-Migration
    }
}
