using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using System.Transactions;
using Project.Entity.Context;
using Project.Entity.Repository;

namespace Project.Entity.UnitofWork
{

    public interface IUnitOfWork : IDisposable
    {

        IRepository<TEntity> GetRepository<TEntity>() where TEntity : class;
        IRoleRepository GetRoleRepository();
        IResearcherRepository GetResearcherRepository();
        IRepositoryAsync<TEntity> GetRepositoryAsync<TEntity>() where TEntity : class;

        ProjectContext Context { get; }
        bool Save();
        Task<bool> SaveAsync();
    }

    public interface IUnitOfWork<TContext> : IUnitOfWork where TContext : IDbConnectionProvider
    {
    }

    public class UnitOfWork : IUnitOfWork
    {
        public ProjectContext Context { get; }

        private Dictionary<Type, object> _repositoriesAsync;
        private Dictionary<Type, object> _repositories;
        private bool _disposed;

        public UnitOfWork(ProjectContext context)
        {
            Context = context;
            _disposed = false;
        }

        public IRepository<TEntity> GetRepository<TEntity>() where TEntity : class
        {
            if (_repositories == null) _repositories = new Dictionary<Type, object>();
            var type = typeof(TEntity);
            if (!_repositories.ContainsKey(type))
                switch (type.ToString())
                {
                    case "Project.Entity.Account":
                        _repositories[type] = new AccountRepository(Context);
                        break;
                    case "Project.Entity.User":
                        _repositories[type] = new UserRepository(Context);
                        break;
                    case "Project.Entity.Role":
                        _repositories[type] = new RoleRepository(Context);
                        break;
                    case "Project.Entity.Project":
                        _repositories[type] = new ProjectRepository(Context);
                        break;
                    case "Project.Entity.Researcher":
                        _repositories[type] = new ResearcherRepository(Context);
                        break;
                }
            return (IRepository<TEntity>)_repositories[type];
        }

        public IRepositoryAsync<TEntity> GetRepositoryAsync<TEntity>() where TEntity : class
        {
            if (_repositoriesAsync == null) _repositoriesAsync = new Dictionary<Type, object>();
            var type = typeof(TEntity);
            if (!_repositoriesAsync.ContainsKey(type))
                switch (type.ToString())
                {
                    case "Project.Entity.Account":
                        _repositoriesAsync[type] = new AccountRepositoryAsync(Context);
                        break;
                    case "Project.Entity.User":
                        _repositoriesAsync[type] = new UserRepositoryAsync(Context);
                        break;
                    case "Project.Entity.Role":
                        _repositoriesAsync[type] = new RoleRepositoryAsync(Context);
                        break;
                }
            return (IRepositoryAsync<TEntity>)_repositoriesAsync[type];
        }

        public bool Save()
        {
            if (Context.Transaction != null)
                Context.Transaction.Commit();
            return true;
        }
        public async Task<bool> SaveAsync()
        {
            await Task.Run(() =>
            {
                if (Context.Transaction != null)
                    Context.Transaction.Commit();
            });
            return true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        public void Dispose(bool isDisposing)
        {
            if (!_disposed)
            {
                if (isDisposing)
                {
                    if (Context.Connection != null) Context.Connection.Dispose();
                    if (Context.Transaction != null) Context.Transaction.Dispose();
                }
            }
            _disposed = true;
        }

        public IRoleRepository GetRoleRepository()
        {
            return new RoleRepository(Context);
        }

        public IResearcherRepository GetResearcherRepository()
        {
            return new ResearcherRepository(Context);
        }
    }
}
