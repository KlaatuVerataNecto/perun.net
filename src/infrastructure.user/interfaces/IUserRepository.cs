﻿using infrastructure.user.entities;
using System.Collections.Generic;

namespace infrastructure.user.interfaces
{
    public interface IUserRepository
    {
        LoginDb getByIdWithUserNameToken(int id);
        LoginDb getByEmailAndProvider(string email, string provider);
        LoginDb getByEmailWithResetInfo(string email, string provider);
        LoginDb getByIdWithResetInfo(int id, string provider);
        List<LoginDb> getLoginsByUserId(int id);
        bool isUsernameAvailable(string username);
        bool isEmailAvailable(string email);
        LoginDb addLogin(LoginDb obj);
        void updateLogin(LoginDb obj);
    }
}
