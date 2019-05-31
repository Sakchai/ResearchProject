﻿using System.Collections.Generic;
using System.Linq;
using Research.Core.Domain.Users;
using Research.Data;
using Research.Services;
using Research.Core.Domain;
using Research.Web.Models.Users;
using FluentValidation;

namespace Research.Web.Validators.Users
{

    public partial class RegisterValidator : BaseResearchValidator<RegisterModel>
    {
        public RegisterValidator(IUserService userService,
            IDbContext dbContext)
        {
            RuleFor(x => x.TitleId)
                .NotEqual(0)
                .WithMessage("ระบุคำนำหน้าชื่อ!");

            RuleFor(x => x.AgencyId)
                .NotEqual(0)
                .WithMessage("ระบุรหัสหน่วยงาน!");

            //ensure that valid email address is entered if Registered role is checked to avoid registered users with empty email address
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress()
                //.WithMessage("Valid Email is required for user to be in 'Registered' role")
                .WithMessage("ระบุอีเมล!");
            //only for registered users
            //.When(x => IsRegisteredUserRoleChecked(x, userService));
            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("อีเมลซ้ำในระบบ ระบุอีกครั้ง!")
                .When(x => IsDuplicateEmailChecked(x, userService));

            //form fields
            RuleFor(x => x.IDCard)
                .NotEmpty()
                .WithMessage("ระบุเลขประจำตัวประชาชน!");

            RuleFor(x => x.IDCard)
                .Length(13)
                .WithMessage("ระบุเลขประจำตัวประชาชนจำนวน 13 หลัก!");

            RuleFor(x => x.IDCard)
                .NotEmpty()
                .WithMessage("เลขประจำตัวประชาชนซ้ำในระบบ กรุณาระบุอีกครั้ง!")
                .When(x => IsDuplicateIDCardChecked(x, userService) == true);



            SetDatabaseValidationRules<User>(dbContext);
        }


        private bool IsDuplicateEmailChecked(RegisterModel model, IUserService userService)
        {
            var user = userService.GetUserByEmail(model.Email);
            return user != null ? true: false;
        }

        private bool IsDuplicateIDCardChecked(RegisterModel model, IUserService userService)
        {
            var user = userService.GetUserByIDCard(model.IDCard);
            return user != null ? false : true;
        }
    }
}