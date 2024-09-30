using Application.ViewModels;
using Domain.Entites;
using System.Collections.Generic;

namespace Application.Interfaces.Mappers
{
    public interface IUserMapper
    {
        UserViewModel Map(User source);
        User Map(UserViewModel source);
        ICollection<UserViewModel> MapElements(ICollection<User> sources);
        ICollection<User> MapElements(ICollection<UserViewModel> sources);
    }
}
