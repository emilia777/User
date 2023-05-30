using Microsoft.AspNetCore.Mvc;

namespace User.Dtos
{
    public class GetUserDto : Controller
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
    }
}
