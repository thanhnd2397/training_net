namespace Training.Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string? LastName { get; set; }
        public string? FullName { get; set; }
        public int? Age { get; set; }
        public string? Address { get; set; }
        public string? Mail { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public bool DeleteFlg { get; set; } = false;
        public DateTime CreateDt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdateDt { get; set; }
        public DateTime? LastLogin { get; set; }
        public int LoginFailCount { get; set; } = 0;
        public int RoleId { get; set; }
        public Role Role { get; set; }
    }
}