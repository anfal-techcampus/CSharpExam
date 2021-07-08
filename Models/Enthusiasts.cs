namespace CSharpExam.Models
{
    public class Enthusiasts
    {
        public int EnthusiastsId {get;set;}
        public int UserId {get;set;}
        public int HobbyId {get;set;}
        public string Proficiency {get;set;} 
        public User User {get;set;} 
        public Hobby Hobby {get;set;} 
    }
}