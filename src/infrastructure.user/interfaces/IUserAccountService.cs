﻿using infrastructure.user.models;
using System.Collections.Generic;

namespace infrastructure.user.interfaces
{
    public interface IUserAccountService
    {
        UserProfile getUserProfile(int userId);
        string changeUsername(int userid, string username);
        string changeAvatar(int userid, string avatarImage);
        string changeCover(int userid, string coverImage);
        UserIdentity changeUsernameByToken(int userid, string username, string token);
        UserLogin getApplicationLoginById(int userid);
        UserUsername getUsernameByUserId(int userid);
        EmailChange createEmailChangeRequest(int userId, string password, string newEmail, int tokenLength, int expiryDays);
        EmailChanged applyEmailByToken(int userId, string token);
        void cancelEmailActivation(int userId);
        string getPendingNewEmailActivation(int loginId);
        List<UserLogin> getLoginsByUserId(int userid);
        bool changePassword(int userid, string currentPassowrd, string newPassowrd, int saltLength);
    }
}
