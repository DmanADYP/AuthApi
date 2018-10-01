using System.ComponentModel.DataAnnotations;

namespace authApi.API.DTO
{
    public class UserRegDTO
    {
        [Required]
        public string username { get; set; }
        [Required]
        [StringLength(8, MinimumLength =4, ErrorMessage="Password must be between 4 and 8 characters")]
        public string password { get; set; }
    }
}