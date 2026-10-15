namespace PSite3.Models.DTOs.Users;

public static class UserDtoMapper
{
    public static UserDto ToDto(User user)
    {
        return new UserDto
        {
            IdUser = user.IdUser,
            UserName = user.UserName,
            Email = user.Email,
            Hash = user.Hash,
            RoleId = user.RoleId,
            IsActive = user.IsActive
        };
    }
}
