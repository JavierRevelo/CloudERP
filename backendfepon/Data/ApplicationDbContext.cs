using backendfepon.ModelConfigurations;
using backendfepon.Models;
using Microsoft.EntityFrameworkCore;

namespace backendfepon.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<ContributionPlan> ContributionPlans { get; set; }
        public DbSet<AcademicPeriod> AcademicPeriods { get; set; }
        public DbSet<Career> Careers { get; set; }
        public DbSet<Semester> Semesters { get; set; }
        public DbSet<Contributor> Contributors { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<AdministrativeMember> AdministrativeMembers { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<Responsible> Responsibles { get; set; }
        public DbSet<EventIncome> EventIncomes { get; set; }
        public DbSet<EventExpense> EventExpenses { get; set; }
        public DbSet<Provider> Providers { get; set; }
        //public DbSet<AccountingAccount> AccountingAccounts { get; set; }
        public DbSet<AccountType> AccountTypes { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<InventoryMovementType> InventoryMovementTypes { get; set; }
        public DbSet<InventoryMovement> InventoryMovements { get; set; }
        public DbSet<FinancialRequestState> FinancialRequestStates { get; set; }
        public DbSet<FinancialRequest> FinancialRequests { get; set; }
        public DbSet<Association> Associations { get; set; }
        public DbSet<CAccountingAccount> CAccountinngAccounts { get; set; }
        public DbSet<Faculty> Faculties { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<TransactionType> TransactionTypes { get; set; }
        public DbSet<EventState> EventStates { get; set; }







        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            //Set up configuration models 
            modelBuilder.ApplyConfiguration(new ProductConfiguration());
            modelBuilder.ApplyConfiguration(new StateConfiguration());
            modelBuilder.ApplyConfiguration(new ContributionPlanConfiguration());
            modelBuilder.ApplyConfiguration(new AcademicPeriodConfiguration());
            modelBuilder.ApplyConfiguration(new CareerConfiguration());
            modelBuilder.ApplyConfiguration(new SemesterConfiguration());
            modelBuilder.ApplyConfiguration(new TransactionConfiguration());
            modelBuilder.ApplyConfiguration(new StudentConfiguration());
            modelBuilder.ApplyConfiguration(new ContributorConfiguration());
            modelBuilder.ApplyConfiguration(new AdministrativeMemberConfiguration());
            modelBuilder.ApplyConfiguration(new EventConfiguration());
            modelBuilder.ApplyConfiguration(new PermissionConfiguration());
            modelBuilder.ApplyConfiguration(new ResponsibleConfiguration());
            modelBuilder.ApplyConfiguration(new EventIncomeConfiguration());
            modelBuilder.ApplyConfiguration(new EventExpenseConfiguration());
            modelBuilder.ApplyConfiguration(new ProviderConfiguration());
            //modelBuilder.ApplyConfiguration(new AccountingAccountConfiguration());
            modelBuilder.ApplyConfiguration(new AccountTypeConfiguration());
            modelBuilder.ApplyConfiguration(new CategoryConfiguration());
            modelBuilder.ApplyConfiguration(new InventoryMovementTypeConfiguration());
            modelBuilder.ApplyConfiguration(new InventoryMovementConfiguration());
            modelBuilder.ApplyConfiguration(new FinantialRequestStateConfiguration());
            modelBuilder.ApplyConfiguration(new FinancialRequestConfiguration());
            modelBuilder.ApplyConfiguration(new AssociationConfiguration());
            modelBuilder.ApplyConfiguration(new TransactionTypeConfiguration());
            modelBuilder.ApplyConfiguration(new EventStateConfiguration());

            //cypher
            modelBuilder.ApplyConfiguration(new CAccountingAccountConfiguration());
            modelBuilder.ApplyConfiguration(new FacultyConfiguration());
            modelBuilder.ApplyConfiguration(new RoleConfiguration());



        }
    }
}
