using Application.Dto.Swap;
using Application.ViewModels;
using Domain.Entites;
using System;
using System.Collections.Generic;

namespace Application.Interfaces
{
    public interface ISwapService
    {
        IEnumerable<SwapViewModel> GetAllSwaps();
        IEnumerable<SwapViewModel> GetAllSwapsConfirmed();
        IEnumerable<SwapViewModel> GetAllSwapsUnConfirmed();
        IEnumerable<SwapViewModel> GetAllSwapsByDate(DateTime date);
        IEnumerable<SwapViewModel> GetAllSwapsByUserId(int user_id);
        IEnumerable<SwapViewModel> GetAllSwapsToConfrm(int user_id);

        SwapViewModel GetSwapById(int id);
        Swap AddNewSwap(CreateSwapDto swp);
        void UpdateSwap(SwapDto swp);
        void DeleteSwap(int id);
        void UpdateUncheckedSwap(int id);
        IEnumerable<SwapViewModel> GetAllUncheckedByAdmin();
        void ConfirmSwap(int id, bool flag);
        bool NotificationListener(int user_id);
        void AcceptSwapByUSer(int swap_id, bool flag);
    }
}
