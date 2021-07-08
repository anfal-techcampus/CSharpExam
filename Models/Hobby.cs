using System;
using System.Collections.Generic;

using System.ComponentModel.DataAnnotations;

namespace CSharpExam.Models
{
    public class Hobby
    {
        [Key]
        public int HobbyId {get;set;}
        [Required]
        public string HobbyName{ get; set;}
        [Required]
        public string Description{ get; set;}
        public List<Enthusiasts> Enthusiasts {get; set;}
        public DateTime CreatedAt {get;set;} = DateTime.Now;
        public DateTime UpdatedAt {get;set;} = DateTime.Now;

    }
}