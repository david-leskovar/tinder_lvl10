using API.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tinder_lvl10.Entities


{
    [Table("Photos")]
    public class Photo { 
    
        public int Id { get; set; }
        public string? Url { get; set; }
        public bool IsMain { get; set; }
        public string? PublicId { get; set; }

        public AppUser AppUser { get; set; }

        public int AppUserId { get; set; }

      

    }

    public class UpdateUserDTO {

        public int id { get; set; }

        public string username { get; set; }

        public byte[]? PasswordHash { get; set; }

        public byte[]? PasswordSalt { get; set; }

        public DateTime dateOfBirth { get; set; }

        public string? knownAs { get; set; }

        public DateTime? created { get; set; } = DateTime.Now;

        public DateTime? lastActive { get; set; } = DateTime.Now;
        public string? gender { get; set; }

        public string? introduction { get; set; }

        public string? lookingFor { get; set; }

        public string? interests { get; set; }

        public string? city { get; set; }

        public string? country { get; set; }

        public ICollection<Photo>? photos { get; set; }

        public ICollection<UserLike>? likedByUsers { get; set; }

        public ICollection<UserLike>? likedUsers { get; set; }

        public ICollection<Message>? messagesSent { get; set; }

        public ICollection<Message>? messagesReceived { get; set; }


    }

    


    public class AppUser
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public byte[]? PasswordHash { get; set; }

        public byte[]? PasswordSalt { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string? KnownAs { get; set; }

        public DateTime? Created { get; set; } = DateTime.Now;

        public DateTime? LastActive { get; set; } = DateTime.Now;
        public string? Gender { get; set; }

        public string? Introduction { get; set; }

        public string? LookingFor { get; set; }

        public string? Interests { get; set; }

        public string? City { get; set; }

        public string? Country { get; set; }

        public ICollection<Photo>? Photos {get;set;}
        
        public ICollection<UserLike>? LikedByUsers { get; set; }

        public ICollection<UserLike>? LikedUsers { get; set; }

        public  ICollection<Message>? MessagesSent { get; set; }

        public ICollection<Message>? MessagesReceived  { get; set; }
        

    }
}
