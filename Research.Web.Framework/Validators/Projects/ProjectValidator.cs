using System.Collections.Generic;
using System.Linq;
using Research.Core.Domain.Users;
using Research.Data;
using Research.Services;
using Research.Web.Models.Users;
using FluentValidation;
using Research.Web.Models.Projects;
using Research.Services.FiscalSchedules;

namespace Research.Web.Validators.Projects
{

    public partial class ProjectValidator : BaseResearchValidator<ProjectModel>
    {
        public ProjectValidator(IFiscalScheduleService fiscalScheduleService,
            IDbContext dbContext)
        {

            //ensure that valid email address is entered if Registered role is checked to avoid registered users with empty email address
            RuleFor(x => x.FiscalYear)
                .NotEqual(0)
                .WithMessage("ระบุปีโครงการวิจัย!");

            RuleFor(x => x.FundAmount)
                .NotEqual(0)
                .WithMessage("ระบุงบประมาณ!");

            RuleFor(x => x.ResearchIssueId)
                           .NotEqual(0)
                           .WithMessage("ระบุประเด็นการวิจัย!");

            //RuleFor(x => x.FiscalYear).Must((x, context) =>
            //{
            //    return IsFiscalYearChecked(x, fiscalScheduleService);
            //}).WithMessage("ปี ไม่ได้อยู่วันรับข้อเสนอโครงการวิจัย!");

            //RuleFor(x => x.ProjectStartDate).Must((x, context) =>
            //{
            //    return IsFiscalYearChecked(x, fiscalScheduleService);
            //}).WithMessage("วันที่ยื่นข้อเสนอ ไม่อยู่ระหว่างวันรับข้อเสนอโครงการวิจัย!");

            //RuleFor(x => x.ProjectEndDate).Must((x, context) =>
            //{
            //    return IsFiscalYearChecked(x, fiscalScheduleService);
            //}).WithMessage("วันสิ้นสุด ไม่อยู่ระหว่างวันรับข้อเสนอโครงการวิจัย!");

  
            SetDatabaseValidationRules<Project>(dbContext);
        }


        private bool IsDuplicateEmailChecked(RegisterModel model, IUserService userService)
        {
            var user = userService.GetUserByEmail(model.Email);
            return user != null ? false : true;
        }

        private bool IsFiscalYearChecked(ProjectModel model, IFiscalScheduleService fiscalScheduleService)
        {
            var user = fiscalScheduleService.GetAllFiscalSchedules(fiscalScheduleName:string.Empty,fiscalYear: model.FiscalYear,openingDate:model.ProjectStartDate,closingDate:model.ProjectEndDate).FirstOrDefault();
            return user != null ? false : true;
        }

    }
}