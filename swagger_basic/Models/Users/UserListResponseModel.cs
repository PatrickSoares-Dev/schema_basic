namespace swagger_basic.Models.Users
{
    public class UserListResponseModel
    {
        public int TotalUsers { get; set; }
        public List<UserResponseModel> Users { get; set; }
        
    }
}
