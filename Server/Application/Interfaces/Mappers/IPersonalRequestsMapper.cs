using Application.ViewModels;
using Domain.Entites;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces.Mappers
{
    public interface IPersonalRequestsMapper
    {
        PersonalRequestsViewModel Map(PersonalRequests source);
        PersonalRequests Map(PersonalRequestsViewModel source);
        ICollection<PersonalRequestsViewModel> MapElements(ICollection<PersonalRequests> sources);
        ICollection<PersonalRequests> MapElements(ICollection<PersonalRequestsViewModel> sources);
    }
}
