using Microsoft.AspNetCore.Mvc;

namespace User.Dtos
{
    public class RegisterRquestDto 
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
