using Microsoft.EntityFrameworkCore;
using ProcessManager.App.Wpf.Core.Settings.Dictionaries;
using ProcessManager.App.Wpf.Core.Settings.Documents;
using ProcessManager.App.Wpf.Core.Settings.Users;
using ProcessManager.App.Wpf.Core.Settings.UserWorkflows;
using ProcessManager.App.Wpf.Core.Settings.Workflows;
using UserWorkflowData = ProcessManager.App.Wpf.Core.Settings.UserWorkflows.UserWorkflowData;

namespace ProcessManager.App.Wpf.Core.Data
{
    public sealed class ApplicationDbContext : DbContext
    {
        private string _connectionString;
        public ApplicationDbContext()
            : base()
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if(string.IsNullOrEmpty(_connectionString))
                optionsBuilder.UseSqlServer("Data source=(localdb)\\MSSQLLocalDB;User=DESKTOP-RL9TP82\\Ldvs;Database=ProcessManager;Integrated Security=SSPI");
            else
                optionsBuilder.UseSqlServer(_connectionString);

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Workflow>()
                .Ignore(x => x.CanCreateUserGroups);

            modelBuilder.Entity<WorkflowStage>()
                .Ignore(x => x.AssigneeDisplayName);

            modelBuilder.Entity<UserWorkflowData>()
                .HasIndex(x => x.Code)
                .IsUnique();


            base.OnModelCreating(modelBuilder);
        }

        public void SetDbConnectionCreds(DbConnectionCreds creds)
        { 
            _connectionString = creds.ToConnectionString();
        }

        public void InitDb()
        {
            this.Database.EnsureCreated();

            InitUsers();
            InitBaseDictionaries();
        }

        private void InitUsers()
        {
            var areUsersCreated = this.Users.Any();

            if (!areUsersCreated)
            {
                var adminUser = new User()
                {
                    Login = "admin",
                    PasswordHash = PasswordHelper.HashPassword("admin"),
                    Status = Shared.Enums.EntityStatus.Active
                };

                var availablePermissions = Enum.GetValues(typeof(PermissionType));
                foreach (PermissionType permission in availablePermissions)
                {
                    var userPermission = new UserPermission()
                    {
                        Permission = permission
                    };

                    adminUser.Permissions.Add(userPermission);
                }
                

                this.Users.Add(adminUser);
                this.SaveChanges();
            }
        }

        private void InitBaseDictionaries()
        {
            //var areDictionariesCreated = this.Dictionaries.Any();

            //if(!areDictionariesCreated)
            //{
            //    var userGroups = new DataDictionary()
            //    {
            //        Name = "Grupy użytkowników",
            //        Code = "USER_GROUPS",
            //        ItemsValueTypes = DictionaryItemValueType.Int,
            //        DictionaryItems = new List<DictionaryItem>()
            //        {

            //        }
            //    };
            //    this.Dictionaries.Add(userGroups);
            //}


            //this.SaveChanges();
        }

        public DbSet<User> Users { get; set; }
        public DbSet<UserPermission> UserPermisssions { get; set; }
        public DbSet<UserGroup> UserGroups { get; set; }
        public DbSet<DataDictionary> Dictionaries { get; set; }
        public DbSet<DictionaryItem> DictionaryItems { get; set; }
        public DbSet<Workflow> Workflows { get; set; }
        public DbSet<WorkflowUserGroup> WorkflowUserGroups { get; set; }
        public DbSet<WorkflowStage> WorkflowStages { get; set; }
        public DbSet<WorkflowStageFieldConfiguration> WorkflowStageFieldConfigurations { get; set; }
        public DbSet<UserWorkflowData> UserWorkflows { get; set; }
        public DbSet<UserWorkflowStageFieldValueData> UserWorkflowFieldValues { get; set; }
        public DbSet<DocumentTemplate> DocumentTemplates { get; set; }
    }
}
