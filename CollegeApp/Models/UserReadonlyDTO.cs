namespace CollegeApp.Models
{
    public class UserReadonlyDTO
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public bool IsActive { get; set; }
        public int UserTypeId { get; set; }
    }
}
