using Application.Dto;
using Application.Dto.User;
using Application.ViewModels;
using System;
using System.Collections.Generic;

namespace Application.Interfaces
{
    public interface IUserService
    {                                               //uzywane w webApi
        IEnumerable<UserViewModel> GetAllUsers();
        IEnumerable<UserViewModel> GetAllNonDisabledUsers();
        IEnumerable<UserViewModel> GetAllDisabledUsers();
        IEnumerable<UserDto> GetAllStateUsers();
        IEnumerable<UserDto> GetAllNonStateUsers();
        UserDto GetUserById(int id);
        UserViewModel GetUser(string login, string passwd);
        UserDto AddNewUser(CreateUserDto user);
        void UpdateUser(UserDto user);
        void UpdateHoursLimit(UpdateMinimalHoursDto dto);
        void DisableUser(int id);
        void EnableUser(int id);
        void ActivateUser(int id);
        IEnumerable<UserViewModel> GetAllOddUsers();
        IEnumerable<UserViewModel> GetAllEvenUsers();
        IEnumerable<UserViewModel> GetAllAdmins();
        int CountAllUsers();
        int CountAdmins();
        void UpdatePassword(UpdateUserPasswordDto dto);
        void UpdateWant24h(UpdateWant24 obj);
        bool CheckPassword(string passwd, int user_id);
        DateTime UserSince(int user_id);
        int GetHoursLimit(int user_id);
        bool GetWant24h(int user_id);
        List<string> GetRanks();
        List<string> GetContracTypes();
        List<string> GetVacationsTypes();
    }
}