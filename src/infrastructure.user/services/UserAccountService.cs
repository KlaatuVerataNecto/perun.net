﻿
using infrastructure.user.entities;
using infrastructure.user.interfaces;
using infrastructure.user.models;
using infrastucture.libs.cryptography;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace infrastructure.user.services
{
    public class UserAccountService : IUserAccountService
    {       
        private readonly IUserRepository _userRepository;
        private readonly IAuthSchemeNameService _authSchemeNameService;
        private readonly ILogger _logger;
        private readonly IUserAuthentiactionService _userAuthentiactionService;

        public UserAccountService(
            IUserRepository userRepository,
            IUserAuthentiactionService userAuthentiactionService,
            IAuthSchemeNameService authSchemeNameService,
            ILogger<UserAccountService> logger
            )
        {
            _userRepository = userRepository;
            _userAuthentiactionService = userAuthentiactionService;
            _authSchemeNameService = authSchemeNameService;
            _logger = logger;
        }

        public UserIdentity changeUsername(int userid , string username, string token)
        {
            if (!_userRepository.isUsernameAvailable(username)) return null;
            var login = _userRepository.getByIdWithUserNameToken(userid);

            if(login == null || 
               login.User == null ||
               login.User.UsernameToken == null ||
               login.User.UsernameToken.token != token)
            {
                _logger.LogError("User tries to change username after social login without valid token.");
                return null;
            }

            login.User.username = username;
            login.User.UsernameToken.token = null;
            _userRepository.updateLogin(login);

            // TODO: Use Automapper
            return new UserIdentity(
                login.User.id,
                login.id,
                login.User.username,
                login.email,
                login.provider,
                login.User.roles,
                login.User.avatar);
        }

        // TODO: validate password too
        public EmailChange createEmailChangeRequest(int userid, string password, string newemail, int tokenLength, int expiryDays)
        {
            if (!_userRepository.isEmailAvailable(newemail)) return null;
            var login = _userRepository.getByIdWithResetInfo(userid, _authSchemeNameService.getDefaultProvider());
            if (login == null)
            {
                _logger.LogError("User intents to generate Email Change Token with invalid userid.", new object[] { userid });
                return null;
            }

           if(_userAuthentiactionService.login(login.email, password) == null)
            {
                _logger.LogError("User intents to generate Email Change Token with invalid password.", new object[] { login.email, });
                return null;
            }

            var now = DateTime.Now;

            // check if email change token exists 
            if (login.UserEmailChanges == null)
            {
                login.UserEmailChanges = new List<UserEmailDb>();
            } 

            var emailChange = login.UserEmailChanges.Where(x => x.token_expiry_date >= now).SingleOrDefault();

            if (emailChange != null)
            {
                return new EmailChange(
                    login.user_id,
                    login.email,
                    newemail,
                    emailChange.token,
                    emailChange.token_expiry_date
                    );
            }

            // create new email change token 
            emailChange = new UserEmailDb
            {
                date_created = now,
                token = CryptographicService.GenerateRandomString(tokenLength),
                token_expiry_date = now.AddDays(expiryDays),
                date_modified = now,
                newemail = newemail
            };

            // update database
            login.UserEmailChanges.Add(emailChange);
            _userRepository.updateLogin(login);

            // TODO: Automapper
            return new EmailChange(
                login.User.id,
                login.email,
                emailChange.newemail,
                emailChange.token,
                emailChange.token_expiry_date
            );
        }


        public List<UserLogin> getLoginsByUserId(int userid)
        {
            var list = _userRepository.getLoginsByUserId(userid);
            var myLogins = new List<UserLogin>();

            foreach (var l in list)
            {
                myLogins.Add(new UserLogin(l.User.id, l.email, l.provider, _authSchemeNameService.getDefaultProvider()));
            }

            return myLogins;
        }
    }
}
