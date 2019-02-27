using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Project.Entity;

namespace Project.Domain.Mapping
{
    public class MappingProfile : Profile
    {
        /// <summary>
        /// Create automap mapping profiles
        /// </summary>
        public MappingProfile()
        {
            CreateMap<AccountViewModel, Account>();
            CreateMap<Account, AccountViewModel>();
            CreateMap<UserViewModel, User>()
                .ForMember(dest => dest.DecryptedPassword, opts => opts.MapFrom(src => src.Password))
                .ForMember(dest => dest.Roles, opts => opts.MapFrom(src => string.Join(";", src.Roles)));
            CreateMap<User, UserViewModel>()
                .ForMember(dest => dest.Password, opts => opts.MapFrom(src => src.DecryptedPassword))
                .ForMember(dest => dest.Roles, opts => opts.MapFrom(src => src.Roles.Split(";", StringSplitOptions.RemoveEmptyEntries)));
            CreateMap<RoleViewModel, Role>();
            CreateMap<Role, RoleViewModel>();
            CreateMap<ProjectModel, Entity.Project>();
            CreateMap<Entity.Project, ProjectModel>();
            CreateMap<Entity.Project, ProjectGridModel>();
            CreateMap<ResearcherViewModel, Researcher>();
            CreateMap<Researcher, ResearcherViewModel>();
            CreateMap<ProjectResearcherViewModel, ProjectResearcher>();
            CreateMap<ProjectResearcher, ProjectResearcherViewModel>();
            //CreateMap<AnswerSetViewModel, AnswerSet>()
            //    .ForMember(dest => dest.Date, opts => opts.MapFrom(src => src.DateAnswersSubmitted))
            //    .ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.AnswerSetId))
            //    .ForMember(dest => dest.Answers, opts => opts.MapFrom(src => src.AnswerSetAnswers))
            //    .ForMember(dest => dest.IsActive, opts => opts.Ignore());
            //CreateMap<AnswerSet, AnswerSetViewModel>()
            //    .ForMember(dest => dest.DateAnswersSubmitted, opts => opts.MapFrom(src => src.Date))
            //    .ForMember(dest => dest.AnswerSetId, opts => opts.MapFrom(src => src.Id))
            //    .ForMember(dest => dest.AnswerSetAnswers, opts => opts.MapFrom(src => src.Answers));

            CreateMissingTypeMaps = true;
        }

    }





}
