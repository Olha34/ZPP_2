using System.ComponentModel.DataAnnotations;

namespace zpp_aplikacja.Pages.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(8)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public UserRole Role { get; set; }

        public int Points { get; set; }

        public User()
        {
            Role = UserRole.Child; // Domyślnie ustawiamy rolę na dziecko
            Points = 0;
        }

        public bool IsParent()
        {
            return Role == UserRole.Parent;
        }

        public void AddPoints(int points)
        {
            Points += points;
        }
    }

    public enum UserRole
    {
        Child,
        Parent
    }
}