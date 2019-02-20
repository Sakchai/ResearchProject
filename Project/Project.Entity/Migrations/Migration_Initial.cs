using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using FluentMigrator;

namespace Project.Entity.Migrations
{
    [Migration(0, "Create database and tables")]
    public class Migration_Initial : Migration
    {
        public override void Up()
        {

            //Execute.Sql(@"
            //            USE Project.
            //            IF OBJECT_ID('VersionInfo') IS NULL 
            //            BEGIN
            //                CREATE TABLE [dbo].[VersionInfo](
	           //                 [Version] [int] NOT NULL,
	           //                 [AppliedOn] [datetime2](7) NOT NULL,
	           //                 [Description] [nvarchar](255) NULL
            //                   ) 
            //            END
            //         ");

            //Execute.Sql(@"
            //            USE Project.
            //            IF OBJECT_ID('Accounts') IS NULL 
            //            BEGIN
            //                CREATE TABLE [dbo].[Accounts](
	           //                 [Id] [int] IDENTITY(1,1) NOT NULL,
	           //                 [Created] [datetime2](7) NOT NULL,
	           //                 [Modified] [datetime2](7) NOT NULL,
	           //                 [Name] [nvarchar](30) NOT NULL,
	           //                 [Email] [nvarchar](30) NOT NULL,
	           //                 [Description] [nvarchar](255) NULL,
	           //                 [IsTrial] [bit] NOT NULL,
	           //                 [IsActive] [bit] NOT NULL,
	           //                 [SetActive] [datetime2](7) NOT NULL,
            //                 CONSTRAINT [PK_Accounts] PRIMARY KEY CLUSTERED 
            //                (
	           //                 [Id] ASC
            //                )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
            //            ) ON [PRIMARY]
            //            END
            //         ");

            //Execute.Sql(@"
            //                USE Project.
            //                IF OBJECT_ID('Accounts') IS NOT NULL AND  OBJECT_ID('Users') IS NULL 
            //                BEGIN
            //                    CREATE TABLE [dbo].[Users](
	           //                     [Id] [int] IDENTITY(1,1) NOT NULL,
	           //                     [Created] [datetime2](7) NOT NULL,
	           //                     [Modified] [datetime2](7) NOT NULL,
	           //                     [FirstName] [nvarchar](20) NOT NULL,
	           //                     [LastName] [nvarchar](20) NOT NULL,
	           //                     [UserName] [nvarchar](30) NULL,
	           //                     [Email] [nvarchar](30) NOT NULL,
	           //                     [Description] [nvarchar](255) NULL,
	           //                     [IsAdminRole] [bit] NOT NULL,
	           //                     [Roles] [nvarchar](255) NULL,
	           //                     [IsActive] [bit] NOT NULL,
	           //                     [Password] [nvarchar](255) NULL,
	           //                     [AccountId] [int] NOT NULL,
            //                     CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
            //                    (
	           //                     [Id] ASC
            //                    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
            //                    ) ON [PRIMARY]
            //                    ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [FK_Users_Accounts_AccountId] FOREIGN KEY([AccountId])
            //                    REFERENCES [dbo].[Accounts] ([Id])
            //                    ON DELETE CASCADE
            //                    ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_Users_Accounts_AccountId]
            //                END
            //         ");


            ////or using a fluent .NET API to generate SQL Data Definition Language
            //Create.Table("Accounts")
            // .WithColumn("Id").AsInt32().NotNullable().PrimaryKey().Identity()
            // .WithColumn("Created").AsDateTime().NotNullable()
            // .WithColumn("Modified").AsDateTime().NotNullable()
            // .WithColumn("Name").AsString(30).NotNullable()
            // .WithColumn("Email").AsString(30).NotNullable()
            // .WithColumn("Description").AsString(255)
            // .WithColumn("IsTrial").AsBoolean().NotNullable()
            // .WithColumn("IsActive").AsBoolean().NotNullable()
            // .WithColumn("SetActive").AsDateTime().NotNullable();

            //Create.Table("Users")
            //.WithColumn("Id").AsInt32().NotNullable().PrimaryKey().Identity()
            //.WithColumn("Created").AsDateTime().NotNullable()
            //.WithColumn("Modified").AsDateTime().NotNullable()
            //.WithColumn("FirstName").AsString(30).NotNullable()
            //.WithColumn("LastName").AsString(20).NotNullable()
            //.WithColumn("UserName").AsString(30)
            //.WithColumn("Email").AsString(30).NotNullable()
            //.WithColumn("Description").AsString(255)
            //.WithColumn("IsAdminRole").AsBoolean().NotNullable()
            //.WithColumn("Roles").AsString(255)
            //.WithColumn("IsActive").AsBoolean().NotNullable()
            //.WithColumn("Password").AsString(255)
            //.WithColumn("AccountId").AsInt32().NotNullable();

            //Create.ForeignKey("FK_Users_Accounts_AccountId")
            // .FromTable("Users").ForeignColumn("AccountId")
            // .ToTable("Accounts").PrimaryColumn("Id");

            Seed();

        }

        public override void Down()
        {
            //Execute.Sql("DROP TABLE IF EXISTS [dbo].[Users]");
            //Execute.Sql("DROP TABLE IF EXISTS [dbo].[Accounts]");

            ////code option but than why not use EF?
            //Delete.Table("Users");
            //Delete.Table("Accounts");
        }

        private void Seed()
        {

            //Execute.Sql(@"
            //            USE Project.
            //            IF NOT EXISTS(SELECT 1 FROM Accounts)
            //            BEGIN
            //                INSERT INTO [dbo].[Accounts]
            //               ([Created]
            //               ,[Modified]
            //               ,[Name]
            //               ,[Email]
            //               ,[Description]
            //               ,[IsTrial]
            //               ,[IsActive]
            //               ,[SetActive])
            //                VALUES
            //               ('1/1/2018',
            //                '1/1/2018',
            //                'Account 1',
            //                'apincore@anasoft.net',
            //                ' ',
            //                0,
            //                1,
            //                '1/1/2018')
            //            END
            //         ");

            //Execute.Sql(@"
            //            USE Project.
            //            IF NOT EXISTS(SELECT 1 FROM Users)
            //            BEGIN
            //                INSERT INTO [dbo].[Users]
            //               ([Created]
            //               ,[Modified]
            //               ,[FirstName]
            //               ,[LastName]
            //               ,[UserName]
            //               ,[Email]
            //               ,[Description]
            //               ,[IsAdminRole]
            //               ,[Roles]
            //               ,[IsActive]
            //               ,[Password]
            //               ,[AccountId])
            //                VALUES
            //               ('1/1/2018',
            //                '1/1/2018',
            //                'TestF',
            //                'TestF',
            //                'my@email.com',
            //                'my@email.com',
            //                ' ',
            //                1,
            //                'Administrator',
            //                1,
            //                'peaRlg4IIkOWvH2ZF0NjIJHr/CrykSgt5u6wsolkFNCwYoJJEPJ1fZryeBaxcpLZ',
            //                '1')
            //            END
            //         ");

        }


    }


    /// <summary>
    /// Not used
    /// </summary>
    public class DatabaseHelper
    {
        public string ConnectionString { get; protected set; }

        public DatabaseHelper(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public static void CreateIfNotExists(string connectionString)
        {
            new DatabaseHelper(connectionString).CreateIfNotExists();
        }

        public void CreateIfNotExists()
        {
            var connectionStringBuilder = new SqlConnectionStringBuilder(ConnectionString);
            var databaseName = connectionStringBuilder.InitialCatalog;

            connectionStringBuilder.InitialCatalog = "master";

            using (var connection = new SqlConnection(connectionStringBuilder.ToString()))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = string.Format("SELECT db_id(N'{0}')", databaseName);
                    using (var reader = command.ExecuteReader())
                    {
                        reader.Read();
                        if (reader[0] != DBNull.Value) // exists
                            return;
                    }

                    command.CommandText = string.Format("CREATE DATABASE {0}", databaseName);
                    command.ExecuteNonQuery();
                }
            }
        }
    }

}
