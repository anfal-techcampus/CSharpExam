using System.ComponentModel.DataAnnotations;

namespace CSharpExam.Models
{
    public class UserLogin
    {
        [Required]
        [MinLength(3)]
        [MaxLength(15)]
        [Display(Name = "User Name")]
        public string UserNameLogin{ get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string PasswordLogin{ get; set; } 
    }
}