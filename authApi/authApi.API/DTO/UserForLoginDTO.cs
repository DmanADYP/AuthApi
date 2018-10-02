using System.ComponentModel.DataAnnotations;

namespace authApi.API.DTO
{
    public class UserForLoginDTO
    {
        public string username { get; set; }
         public string password { get; set; }
    }
}