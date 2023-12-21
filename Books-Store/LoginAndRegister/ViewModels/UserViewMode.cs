namespace LoginAndRegister.ViewModels
{
    public class UserViewMode
    {
        public string Id { get; set; }
        public string FristName { get; set; }
        public string LasttName { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public IEnumerable<string> Roles { get; set;}
    }
}
