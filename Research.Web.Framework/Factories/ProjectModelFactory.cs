﻿using System;
using System.Linq;
using Research.Web.Models.Projects;
using Research.Web.Extensions;
using Research.Data;
using Research.Web.Models.Factories;
using Research.Services.Projects;
using Research.Enum;
using System.Runtime.Serialization;

namespace Research.Web.Factories
{
    public class ProjectModelFactory : IProjectModelFactory
    {
        private readonly IBaseAdminModelFactory _baseAdminModelFactory;
        private readonly IProjectService _projectService;

        public ProjectModelFactory(IBaseAdminModelFactory baseAdminModelFactory, 
            IProjectService projectService)
        {
            this._baseAdminModelFactory = baseAdminModelFactory;
            this._projectService = projectService;
        }

        public virtual ProjectResearcherSearchModel PrepareProjectResearcherSearchModel(ProjectResearcherSearchModel searchModel, Project project)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            if (project == null)
                throw new ArgumentNullException(nameof(project));

            searchModel.ProjectId = project.Id;

            //prepare page parameters
            searchModel.SetGridPageSize();

            return searchModel;
        }

        public virtual ProjectProfessorSearchModel PrepareProjectProfessorSearchModel(ProjectProfessorSearchModel searchModel, Project project)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            if (project == null)
                throw new ArgumentNullException(nameof(project));

            searchModel.ProjectId = project.Id;

            //prepare page parameters
            searchModel.SetGridPageSize();

            return searchModel;
        }
        /// <summary>
        /// Prepare paged project list model
        /// </summary>
        /// <param name="searchModel">Project search model</param>
        /// <returns>Project list model</returns>
        public ProjectListModel PrepareProjectListModel(ProjectSearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            var projects = _projectService.GetAllProjects(pageIndex: searchModel.Page - 1, pageSize: searchModel.PageSize);

            //prepare list model
            var model = new ProjectListModel
            {
                Data = projects.Select(project =>
                {
                    // fill in model values from the entity
                    var projectModel = new ProjectModel
                    {
                        Id = project.Id,
                        FiscalYear = project.FiscalYear,
                        ProjectCode = project.ProjectCode,
                        ProjectNameTh = project.ProjectNameTh,
                        StartContractDateName = ConvertToThaiDate(project.ProjectStartDate),
                        ProgressStatusName = project.ProjectProgresses.LastOrDefault() != null ? project.ProjectProgresses.LastOrDefault().ProgressStatus.ToString() : string.Empty,
                        ProjectStatusName = project.ProjectStatus.GetAttributeOfType<EnumMemberAttribute>().Value,
                    };

                    return projectModel;
                }),
                Total = projects.TotalCount
            };
            return model;
        }

        private string ConvertToThaiDate(DateTime date)
        {
            string[] months = new string[] { "มกราคม", "กุมภาพันธ์", "มีนาคม", "เมษายน", "พฤษภาคม", "มิถุนายน", "กรกฎาคม", "สิงหาคม", "กันยายน", "ตุลาคม", "พฤศจิกายน", "ธันวาคม" };
            int day = date.Day;
            int year = date.Year + 543;
            string month = months[date.Month - 1];
            return $"{day} {month} {year}";
        }
        public ProjectSearchModel PrepareProjectSearchModel(ProjectSearchModel searchModel)
        {

            _baseAdminModelFactory.PrepareAgencies(searchModel.AvailableAgencies,true, "--หน่วยงาน--");
            _baseAdminModelFactory.PrepareFiscalSchedules(searchModel.AvailableFiscalYears,true, "--ปีงบประมาณ--");
            _baseAdminModelFactory.PrepareProjectStatuses(searchModel.AvailableProjectStatuses,true, "--สถานะผลการพิจารณา--");
            _baseAdminModelFactory.PrepareProgressStatuses(searchModel.AvailableProgressStatuses,true, "--สถานะโครงการวิจัย--");
            //prepare page parameters
            searchModel.SetGridPageSize();
            return searchModel;
        }

        public ProjectResearcherListModel PrepareProjectResearcherListModel(ProjectResearcherSearchModel searchModel, Project project)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            if (project == null)
                throw new ArgumentNullException(nameof(project));

            //get researcher educations
            //chai
            //var researcherEducations = researcher.ResearcherEducations.OrderByDescending(edu => edu.Degree).ToList();
            var projectResearchers = _projectService.GetAllProjectResearchers(project.Id).ToList();
            //prepare list model
            var model = new ProjectResearcherListModel
            {
                Data = projectResearchers.PaginationByRequestModel(searchModel).Select(x =>
                {
                    //fill in model values from the entity       
                    var projectResearcherModel = new ProjectResearcherModel
                    {
                        Id = x.Id,
                        Portion = x.Portion,
                        ProjectId = x.ProjectId,
                        RoleName = x.ProjectRole.GetAttributeOfType<EnumMemberAttribute>().Value,
                        ResearcherId = x.ResearcherId,
                        ProjectRoleId = x.ProjectRoleId,
                        ResearcherName = $"{x.Researcher.FirstName} {x.Researcher.LastName}"
                    };



                    return projectResearcherModel;
                }),
                Total = projectResearchers.Count
            };

            return model;
        }

        public ProjectModel PrepareProjectModel(ProjectModel model, Project project)
        {
            if (project != null)
            {
                //fill in model values from the entity

                model = model ?? project.ToModel<ProjectModel>();
                model.Id = project.Id;
                model.ProjectCode = project.ProjectCode;
                model.FiscalYear = project.FiscalYear;
                model.ProjectNameTh = project.ProjectNameTh;
                model.ProjectNameEn = project.ProjectNameEn;
                model.PlanNameTh = project.PlanNameTh;
                model.PlanNameEn = project.PlanNameEn;
                model.ProjectType = project.ProjectType;
                model.FundAmount = project.FundAmount;
                model.StrategyGroupId = project.StrategyGroupId;
                model.ProjectStatusId = project.ProjectStatusId.Value;
                model.StartContractDate = project.ProjectStartDate;
                model.EndContractDate = project.ProjectEndDate;
                model.LastUpdateBy = project.LastUpdateBy;
                model.Comment = project.Comment;
            }
            model.AddProjectProgressStartDate = DateTime.Today;
            model.AddProjectProgressEndDate = model.StartContractDate.AddMonths(5);

            _baseAdminModelFactory.PrepareResearchIssues(model.AvailableResearchIssues, true, "--ระบุประเด็นการวิจัย--");
            _baseAdminModelFactory.PrepareProjectStatuses(model.AvailableProjectStatuses, true, "--ระบุผลการพิจารณา--");

            _baseAdminModelFactory.PrepareResearchers(model.AvailableResearchers, true, "--ระบุผู้วิจัย--");
            _baseAdminModelFactory.PrepareProjectRoles(model.AvailableProjectRoles, true, "--ระบุบทบาทในโครงการ--");

            _baseAdminModelFactory.PrepareProfessors(model.AvailableProfessors, true, "--ระบุผู้ทรงคุณวุฒิ--");
            _baseAdminModelFactory.PrepareProfessorTypes(model.AvailableProfessorTypes, true, "--ระบุประเภทผู้ทรงคุณวุฒิ--");

            _baseAdminModelFactory.PrepareProgressStatuses(model.AvailableProgressStatuses, true, "--ระบุสถานะโครงการวิจัย--");
            return model;
        }

        public ProjectProfessorListModel PrepareProjectProfessorListModel(ProjectProfessorSearchModel searchModel, Project project)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            if (project == null)
                throw new ArgumentNullException(nameof(project));

            //get researcher educations
            //chai
            //var researcherEducations = researcher.ResearcherEducations.OrderByDescending(edu => edu.Degree).ToList();
            var projectProfessors = _projectService.GetAllProjectProfessors(project.Id).ToList();
            //prepare list model
            var model = new ProjectProfessorListModel
            {
                Data = projectProfessors.PaginationByRequestModel(searchModel).Select(x =>
                {
                    //fill in model values from the entity       
                    var projectProfessorModel = new ProjectProfessorModel
                    {
                        Id = x.Id,
                        ProjectId = x.ProjectId,
                        ProfessorId = x.ProfessorId,
                        ProfessorTypeId = x.ProfessorTypeId,
                        ProfessorTyperName = x.ProfessorType.GetAttributeOfType<EnumMemberAttribute>().Value,
                    };



                    return projectProfessorModel;
                }),
                Total = projectProfessors.Count
            };

            return model;
        }

        public ProjectProgressSearchModel PrepareProjectProgressSearchModel(ProjectProgressSearchModel searchModel, Project project)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            if (project == null)
                throw new ArgumentNullException(nameof(project));

            searchModel.ProjectId = project.Id;
            //prepare page parameters
            searchModel.SetGridPageSize();

            return searchModel;
        }

        public ProjectProgressListModel PrepareProjectProgressListModel(ProjectProgressSearchModel searchModel, Project project)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            if (project == null)
                throw new ArgumentNullException(nameof(project));

            //get researcher educations
            //chai
            //var researcherEducations = researcher.ResearcherEducations.OrderByDescending(edu => edu.Degree).ToList();
            var projectProgresses = _projectService.GetAllProjectProgresses(project.Id).ToList();
            //prepare list model
            var model = new ProjectProgressListModel
            {
                Data = projectProgresses.PaginationByRequestModel(searchModel).Select(x =>
                {
                    //fill in model values from the entity       
                    var projectProfessorModel = new ProjectProgressModel
                    {
                        Id = x.Id,
                        ProjectId = x.ProjectId,
                        Comment = x.Comment,
                        LastUpdateBy = x.LastUpdateBy,
                        ModifiedName = ConvertToThaiDate(x.Modified),
                        ProgressStartDateName = ConvertToThaiDate(x.ProgressStartDate),
                        ProgressEndDateName = ConvertToThaiDate(x.ProgressEndDate),
                        ProgressStatusName = x.ProgressStatus.GetAttributeOfType<EnumMemberAttribute>().Value,
                    };

                    return projectProfessorModel;
                }),
                Total = projectProgresses.Count
            };

            return model;
        }
    }
}