using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using IdentityServer4.Validation;
using IdentityServer4.Services;
using IdentityServer4.Models;
using System.Security.Claims;
using IdentityModel;

namespace IndentityServer
{
    public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        private IConfiguration _config;

        public ResourceOwnerPasswordValidator(IConfiguration config)
        {
            _config = config;
        }
        /**
         * The “ValidateAsync” method search the Users in database and  verify their password.
         * 
         * */
        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            await Task.Run(() =>
            {
                IEnumerable<User> users = Config.Users.Where(x => x.UserName == context.UserName);

                if (users == null)
                {
                    context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "The user or password are incorrect", null);
                }
                else if (users.Count() == 0 |
                    (users.Count() == 1 && (users).Single().DecryptedPassword != context.Password))
                {
                    context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "The user or password are incorrect", null);
                }
                else if (users.Count() > 1)
                {
                    context.Result = new GrantValidationResult(TokenRequestErrors.InvalidClient, "Multiple user name found", null);
                }
                else if ((users).Single().DecryptedPassword == context.Password & !(users).Single().IsActive)
                {
                    context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "The user is not active", null);
                }
                else  // then the user is valid
                {
                    User user = (users).Single();
                    context.Result = new GrantValidationResult(user.UserId.ToString(), "password", DateTime.Now, GetUserClaims(user), "local", null); //this will set the userID that is used to get the claims in the ProfileService.cs
                }
            });

        }

        //build claims array from user data
        public static List<Claim> GetUserClaims(User user)
        {
            List<Claim> claims = new List<Claim>
            {
            new Claim("user_id", user.UserId.ToString() ?? ""),
            new Claim(JwtClaimTypes.Name, (!string.IsNullOrEmpty(user.FirstName) && !string.IsNullOrEmpty(user.LastName)) ? (user.FirstName + " " + user.LastName) : ""),
            new Claim(JwtClaimTypes.GivenName, user.FirstName  ?? ""),
            new Claim(JwtClaimTypes.FamilyName, user.LastName  ?? ""),
            new Claim(JwtClaimTypes.Email, user.Email  ?? ""),
            new Claim("description", user.Description ?? ""),   //any custom claim type
            };
            //add user roles (authorization)
            foreach (string role in user.Roles)
            {
                claims.Add(new Claim(JwtClaimTypes.Role, role));
            }
            return claims;
        }

    }

    public class ProfileService : IProfileService
    {
        private IConfiguration _config;

        /// <summary>
        /// This method is called after the user is authenticated in the ResourceOwnerPasswordValidator 
        /// and it adds the claims to the JWT Token.
        /// </summary>
        /// <param name="authRepository"></param>
        public ProfileService(IConfiguration config)
        {
            _config = config;
        }
        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            await Task.Run(() =>
            {
                var id = context.Subject.Claims.FirstOrDefault().Value;
                int userId = Int32.Parse(id);
                User user = Config.Users.FirstOrDefault(x => x.UserId == userId);
                context.IssuedClaims = ResourceOwnerPasswordValidator.GetUserClaims(user);
            });
        }
        public async Task IsActiveAsync(IsActiveContext context)
        {
            await Task.Run(() =>
            {
                try
                {
                    //get subject from context (set in ResourceOwnerPasswordValidator.ValidateAsync),
                    var userId = context.Subject.Claims.FirstOrDefault(x => x.Type == "user_id");
                    if (!string.IsNullOrEmpty(userId?.Value) && long.Parse(userId.Value) > 0)
                    {
                        User user = Config.Users.FirstOrDefault(x => x.UserId == int.Parse(userId.Value));
                        if (user != null)
                        {
                            if (user.IsActive)
                            {
                                context.IsActive = user.IsActive;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            });
        }
    }

}
