using System.Collections.Generic;
using System.Linq;
using Research.Core.Domain.Users;
using Research.Data;
using Research.Services;
using Research.Web.Models.Users;
using FluentValidation;
using Research.Web.Models.Researchers;
using Research.Services.FiscalSchedules;
using Research.Services.Researchers;

namespace Research.Web.Validators.Researchers
{

    public partial class ResearcherValidator : BaseResearchValidator<ResearcherModel>
    {
        public ResearcherValidator(IResearcherService researcherService,
            IDbContext dbContext)
        {

            //ensure that valid email address is entered if Registered role is checked to avoid registered users with empty email address
            RuleFor(x => x.TitleId)
                .NotEqual(0)
                .WithMessage("ระบุรหัสคำนำชื่อ!");

            RuleFor(x => x.PersonalTypeId)
                .NotEqual(0)
                .WithMessage("ระบุรหัสประเภทบุคลากร!");

            RuleFor(x => x.AgencyId)
                .NotEqual(0)
                .WithMessage("ระบุหน่วยงาน!");

            RuleFor(x => x.AcademicRankId)
                .NotEqual(0)
                .WithMessage("ระบุตำแหน่งทางวิชาการ!");


            RuleFor(x => x.DateOfBirthDay)
                .NotEqual(0)
                .WithMessage("ระบุวันเกิด!");

            RuleFor(x => x.DateOfBirthMonth)
                .NotEqual(0)
                .WithMessage("ระบุเดือนเกิด!");

            RuleFor(x => x.DateOfBirthYear)
                .NotEqual(0)
                .WithMessage("ระบุปีเกิด!");


            SetDatabaseValidationRules<Researcher>(dbContext);
        }




    }
}