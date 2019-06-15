using System.Collections.Generic;
using System.Linq;
using Research.Core.Domain.Users;
using Research.Data;
using Research.Services;
using Research.Web.Models.Users;
using FluentValidation;
using Research.Web.Models.Professors;
using Research.Services.FiscalSchedules;
using Research.Services.Professors;

namespace Research.Web.Validators.Professors
{

    public partial class ProfessorValidator : BaseResearchValidator<ProfessorModel>
    {
        public ProfessorValidator(IProfessorService professorService,
            IDbContext dbContext)
        {

            //ensure that valid email address is entered if Registered role is checked to avoid registered users with empty email address
            RuleFor(x => x.TitleId)
                .NotEqual(0)
                .WithMessage("ระบุรหัสคำนำชื่อ!");

 
            SetDatabaseValidationRules<Professor>(dbContext);
        }




    }
}