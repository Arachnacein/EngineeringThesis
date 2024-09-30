using Application.ViewModels;
using Domain.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Mappers
{
    public interface ISwapMapper
    {
        SwapViewModel Map(Swap source);
        Swap Map(SwapViewModel source);
        ICollection<SwapViewModel> MapElements(ICollection<Swap> sources);
        ICollection<Swap> MapElements(ICollection<SwapViewModel> sources);
    }
}
