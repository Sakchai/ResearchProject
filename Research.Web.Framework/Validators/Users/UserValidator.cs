using System.Collections.Generic;
using System.Linq;
using Research.Core.Domain.Users;
using Research.Data;
using Research.Services;
using Research.Core.Domain;
using Research.Web.Models.Users;
using FluentValidation;
using System;

namespace Research.Web.Validators.Users
{

    public partial class UserValidator : BaseResearchValidator<UserModel>
    {
        public UserValidator(IUserService userService,
            IDbContext dbContext)
        {
            RuleFor(x => x.TitleId)
                .NotEqual(0)
                .WithMessage("ระบุคำนำหน้าชื่อ!");

            RuleFor(x => x.AgencyId)
                .NotEqual(0)
                .WithMessage("ระบุรหัสหน่วยงาน!");

            //ensure that valid email address is entered if Usered role is checked to avoid registered users with empty email address
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress()
                .WithMessage("ระบุอีเมล!");

            RuleFor(x => x.UserTypeId)
                .NotEqual(0)
                .WithMessage("ระบุระดับสิทธิ์!");
            SetDatabaseValidationRules<User>(dbContext);
        }


    }
}