namespace ShortVideo.API.Data.DbModels
{
    public class User
    {
        public int Id { get; set; }
        public Authority Authority { get; set; }
        public string Username { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
    }

    public enum Authority
    {
        User,
        Admin,
    }
}