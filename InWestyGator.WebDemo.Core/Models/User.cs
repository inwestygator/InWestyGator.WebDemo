namespace InWestyGator.WebDemo.Core.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        
        // TODO: naturally we never store it this way and recently won't even use such solutions
        // just wanted to add some formal example
        public string Password { get; set; }
    }
}
