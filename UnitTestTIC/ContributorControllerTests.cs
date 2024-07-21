using AutoMapper;
using backendfepon.Controllers;
using backendfepon.Data;
using backendfepon.DTOs.ContributorDTO;
using backendfepon.ModelConfigurations.Profiles;
using backendfepon.Models;
using backendfepon.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace UnitTestTIC
{
    public class ContributorControllerTests
    {
        private readonly DbContextOptions<ApplicationDbContext> _options;
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _context;
        private readonly ContributorController _controller;

        public ContributorControllerTests()
        {
            _options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new ApplicationDbContext(_options);

            var config = new MapperConfiguration(cfg =>
            {
               cfg.AddProfile<ContributorProfile>();
            });

            _mapper = config.CreateMapper();
            // Configure logging
            var serviceProvider = new ServiceCollection()
                .AddLogging(builder => builder
                    .AddConsole()
                    .SetMinimumLevel(LogLevel.Information))
                .BuildServiceProvider();

            _controller = new ContributorController(_context, _mapper);
        }

        private void InitializeDatabase()
        {
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();
        }
        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Fact]
        public async Task GetContributors_ReturnsContributors_WhenContributorsExist()
        {
            // Arrange
            InitializeDatabase();

            var contributor1 = new Contributor
            {
                Contributor_Id = 1,
                Name = "John Doe",
                State_Id = Constants.DEFAULT_STATE,
                ContributionPlan = new ContributionPlan
                {
                    Name = "Plan 1",
                    Economic_Value = 100,
                    AcademicPeriod = new AcademicPeriod { Academic_Period_Name = "2024" },
                    Benefits = "Plan A"


                },
                Contributor_Date = DateTime.Now,
                Career = new Career { Career_Name = "Engineering" },
                Faculty = new Faculty { Faculty_Name = "Science" },
                Email = "john.doe@example.com"
            };


            _context.Contributors.Add(contributor1);
            _context.SaveChanges();

            // Act
            var result = await _controller.GetContributors();

            // Assert
            var actionResult = Assert.IsType<ActionResult<IEnumerable<ContributorDTO>>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var returnedContributors = Assert.IsType<List<ContributorDTO>>(okResult.Value);

            Assert.Equal(1, returnedContributors.Count);
        }



        [Fact]
        public async Task GetContributor_ReturnsNotFound_WhenContributorDoesNotExist()
        {
            // Arrange
            InitializeDatabase();

            // Act
            var result = await _controller.GetContributor(100);

            // Assert
            var actionResult = Assert.IsType<ActionResult<ContributorDTO>>(result);
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(actionResult.Result);

            Assert.Equal("Aportante no encontrado.", ((dynamic)notFoundResult.Value).Message);
        }

        [Fact]
        public async Task PostContributor_ReturnsCreatedContributor()
        {
            // Arrange
            InitializeDatabase();

            var contributorDTO = new CreateUpdateContributorDTO
            {
                Name = "John Doe",
                Faculty = "Science",
                Career = "Engineering",
                Plan = "Plan 1",
                Date = DateTime.Now,
                Email = "john.doe@example.com"
            };

            var faculty = new Faculty { Faculty_Id = 1, Faculty_Name = "Science" };
            var career = new Career { Career_Id = 1, Career_Name = "Engineering" };
            var plan = new ContributionPlan
            {
                Plan_Id = 1,
                Name = "Plan 1",
                Economic_Value = 100,
                AcademicPeriod = new AcademicPeriod { Academic_Period_Name = "2024" },
                Benefits = "Plan A"
            };

            _context.Faculties.Add(faculty);
            _context.Careers.Add(career);
            _context.ContributionPlans.Add(plan);
            _context.SaveChanges();

            try
            {

                // Act
                var result = await _controller.PostContributor(contributorDTO);

                // Assert
                var actionResult = Assert.IsType<ActionResult<ContributorDTO>>(result);
                if (!(actionResult.Result is CreatedAtActionResult))
                {
                    var objectResult = actionResult.Result as ObjectResult;
                    var errorMessage = objectResult?.Value?.ToString() ?? "No error message";
                    Assert.True(false, $"Unexpected result type: {actionResult.Result.GetType().Name}, Error: {errorMessage}");
                }
                Assert.IsType<CreatedAtActionResult>(actionResult.Result);

                var createdAtActionResult = (CreatedAtActionResult)actionResult.Result;
                var returnedContributor = Assert.IsType<ContributorDTO>(createdAtActionResult.Value);

                Assert.Equal(contributorDTO.Name, returnedContributor.name);
                Assert.Equal(contributorDTO.Faculty, returnedContributor.faculty);
                Assert.Equal(contributorDTO.Career, returnedContributor.career);
                Assert.Equal(contributorDTO.Plan, returnedContributor.plan);
                Assert.Equal(contributorDTO.Email, returnedContributor.email);
            }
            catch (Exception ex)
            {
                Assert.True(false, ex.Message);
            }
        }

        [Fact]
        public async Task PutContributor_ReturnsOk()
        {
            // Arrange
            InitializeDatabase();

            var contributor = new Contributor
            {
                Contributor_Id = 11,
                Name = "John Doe",
                State_Id = Constants.DEFAULT_STATE,
                ContributionPlan = new ContributionPlan
                {
                    Name = "Plan 1",
                    Economic_Value = 100,
                    AcademicPeriod = new AcademicPeriod { Academic_Period_Name = "2024" },
                    Benefits = "Plan A"
                },
                Contributor_Date = DateTime.Now,
                Career = new Career { Career_Name = "Engineering" },
                Faculty = new Faculty { Faculty_Name = "Science" },
                Email = "john.doe@example.com"
            };

            _context.Contributors.Add(contributor);
            _context.SaveChanges();

            var contributorDTO = new CreateUpdateContributorDTO
            {
                Name = "John Smith",
                Faculty = "Science",
                Career = "Engineering",
                Plan = "Plan 1",
                Date = DateTime.Now,
                Email = "john.smith@example.com"
            };

            var faculty = new Faculty { Faculty_Id = 3, Faculty_Name = "Science" };
            var career = new Career { Career_Id = 3, Career_Name = "Engineering" };
            var plan = new ContributionPlan
            {
                Plan_Id = 3,
                Name = "Plan 1",
                Economic_Value = 100,
                AcademicPeriod = new AcademicPeriod { Academic_Period_Name = "2024" },
                Benefits = "Plan A"
            };

            _context.Faculties.Add(faculty);
            _context.Careers.Add(career);
            _context.ContributionPlans.Add(plan);
            _context.SaveChanges();

            // Act
            var result = await _controller.PutContributor(11, contributorDTO);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task PatchContributorState_UpdatesStateToInactive()
        {
            // Arrange
            InitializeDatabase();

            var contributor = new Contributor
            {
                Contributor_Id = 2,
                Name = "John Doe",
                State_Id = Constants.DEFAULT_STATE,
                ContributionPlan = new ContributionPlan
                {
                    Name = "Plan 1",
                    Economic_Value = 100,
                    AcademicPeriod = new AcademicPeriod { Academic_Period_Name = "2024" },
                    Benefits = "Plan A"
                },
                Contributor_Date = DateTime.Now,
                Career = new Career { Career_Name = "Engineering" },
                Faculty = new Faculty { Faculty_Name = "Science" },
                Email = "john.doe@example.com"
            };

            _context.Contributors.Add(contributor);
            _context.SaveChanges();

            // Act
            var result = await _controller.PatchContributorState(2);

            // Assert
            var actionResult = Assert.IsType<NoContentResult>(result);
            var updatedContributor = await _context.Contributors.FindAsync(2);

            Assert.Equal(Constants.STATE_INACTIVE, updatedContributor.State_Id);
        }
    }
}
