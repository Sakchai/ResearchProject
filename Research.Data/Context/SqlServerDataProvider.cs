﻿using Research.Core.Data;
using Research.Core.Domain.Common;
using Research.Data.Extensions;
using Research.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;

namespace Research.Data
{
    /// <summary>
    /// Represents SQL Server data provider
    /// </summary>
    public partial class SqlServerDataProvider : IDataProvider
    {
        #region Methods

        /// <summary>
        /// Initialize database
        /// </summary>
        public virtual void InitializeDatabase()
        {
            var context = EngineContext.Current.Resolve<IDbContext>();

            //check some of table names to ensure that we have installed
            var tableNamesToValidate = new List<string> { "User", "Researcher", "Project", "Professor", "ResearchIssue" };
            var existingTableNames = context
                .QueryFromSql<StringQueryType>("SELECT table_name AS Value FROM INFORMATION_SCHEMA.TABLES WHERE table_type = 'BASE TABLE'")
                .Select(stringValue => stringValue.Value).ToList();
            var createTables = !existingTableNames.Intersect(tableNamesToValidate, StringComparer.InvariantCultureIgnoreCase).Any();
            if (!createTables)
                return;

            var fileProvider = EngineContext.Current.Resolve<IResearchFileProvider>();

            //create tables
            //EngineContext.Current.Resolve<IRelationalDatabaseCreator>().CreateTables();
            //(context as DbContext).Database.EnsureCreated();
           // context.ExecuteSqlScript(context.GenerateCreateScript());

            //create indexes
           // context.ExecuteSqlScriptFromFile(fileProvider.MapPath(ResearchDataDefaults.SqlServerIndexesFilePath));

            //create stored procedures 
           // context.ExecuteSqlScriptFromFile(fileProvider.MapPath(ResearchDataDefaults.SqlServerStoredProceduresFilePath));
        }

        /// <summary>
        /// Get a support database parameter object (used by stored procedures)
        /// </summary>
        /// <returns>Parameter</returns>
        public virtual DbParameter GetParameter()
        {
            return new SqlParameter();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets a value indicating whether this data provider supports backup
        /// </summary>
        public virtual bool BackupSupported => true;

        /// <summary>
        /// Gets a maximum length of the data for HASHBYTES functions, returns 0 if HASHBYTES function is not supported
        /// </summary>
        public virtual int SupportedLengthOfBinaryHash => 8000; //for SQL Server 2008 and above HASHBYTES function has a limit of 8000 characters.

        #endregion
    }
}