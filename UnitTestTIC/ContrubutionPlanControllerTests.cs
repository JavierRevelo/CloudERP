using AutoMapper;
using backendfepon.Controllers;
using backendfepon.Data;
using backendfepon.DTOs.ContributionPlanDTOs;
using backendfepon.ModelConfigurations.Profiles;
using backendfepon.Models;
using backendfepon.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestTIC
{
    public class ContrubutionPlanControllerTests
    {
        private readonly DbContextOptions<ApplicationDbContext> _options;
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _context;
        private readonly ContributionPlanController _controller;

        public ContrubutionPlanControllerTests()
        {
            _options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new ApplicationDbContext(_options);

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ContributionPlanProfile>();
            });

            _mapper = config.CreateMapper();

            // Configure logging
            var serviceProvider = new ServiceCollection()
                .AddLogging(builder => builder
                    .AddConsole()
                    .SetMinimumLevel(LogLevel.Information))
                .BuildServiceProvider();

            _controller = new ContributionPlanController(_context, _mapper);
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
        public async Task GetContributionPlans_ReturnsContributionPlans_WhenContributionPlansExist()
        {
            // Arrange
            InitializeDatabase();

            var contributionPlan1 = new ContributionPlan
            {
                Plan_Id = 1,
                Name = "Plan 1",
                Economic_Value = 100,
                AcademicPeriod = new AcademicPeriod { Academic_Period_Name = "2024" },
                State_Id = Constants.DEFAULT_STATE,
                Benefits = "Benefit 1"
            };

            var contributionPlan2 = new ContributionPlan
            {
                Plan_Id = 2,
                Name = "Plan 2",
                Economic_Value = 200,
                AcademicPeriod = new AcademicPeriod { Academic_Period_Name = "2024" },
                State_Id = Constants.DEFAULT_STATE,
                Benefits = "Benefit 2"
            };

            _context.ContributionPlans.AddRange(contributionPlan1, contributionPlan2);
            _context.SaveChanges();

            // Act
            var result = await _controller.GetContributionPlans();

            // Assert
            var actionResult = Assert.IsType<ActionResult<IEnumerable<ContributionPlanDTO>>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var returnedPlans = Assert.IsType<List<ContributionPlanDTO>>(okResult.Value);

            Assert.Equal(2, returnedPlans.Count);
        }

        [Fact]
        public async Task GetContributionPlan_ReturnsNotFound_WhenContributionPlanDoesNotExist()
        {
            // Arrange
            InitializeDatabase();

            // Act
            var result = await _controller.GetContributionPlan(100);

            // Assert
            var actionResult = Assert.IsType<ActionResult<ContributionPlanDTO>>(result);
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(actionResult.Result);

            Assert.Equal("Plan de contribución no encontrado.", ((dynamic)notFoundResult.Value).Message);
        }

        [Fact]
        public async Task PostContributionPlan_ReturnsCreatedContributionPlan()
        {
            // Arrange
            InitializeDatabase();

            var contributionPlanDTO = new CreateUpdateContributionPlanDTO
            {
                planName = "Plan 1",
                Academic_Period_Name = "2024",
                benefits = "benefits",
                price = 20
            };

            var academicPeriod = new AcademicPeriod { Academic_Period_Id = 1, Academic_Period_Name = "2024" };

            _context.AcademicPeriods.Add(academicPeriod);
            _context.SaveChanges();

            try
            {

                // Act
                var result = await _controller.PostContributionPlan(contributionPlanDTO);

                // Assert
                var actionResult = Assert.IsType<ActionResult<ContributionPlanDTO>>(result);
                if (!(actionResult.Result is CreatedAtActionResult))
                {
                    var objectResult = actionResult.Result as ObjectResult;
                    var errorMessage = objectResult?.Value?.ToString() ?? "No error message";
                    Assert.True(false, $"Unexpected result type: {actionResult.Result.GetType().Name}, Error: {errorMessage}");
                }
                Assert.IsType<CreatedAtActionResult>(actionResult.Result);

                var createdAtActionResult = (CreatedAtActionResult)actionResult.Result;
                var returnedPlan = Assert.IsType<ContributionPlanDTO>(createdAtActionResult.Value);

                Assert.Equal(contributionPlanDTO.planName, returnedPlan.planName);
                Assert.Equal(contributionPlanDTO.Academic_Period_Name, returnedPlan.academicPeriod);
                Assert.Equal(contributionPlanDTO.price, returnedPlan.price);
                Assert.Equal(contributionPlanDTO.benefits, returnedPlan.benefits);
            }
            catch (Exception ex) { 
                Assert.True(false, ex.Message);
            }
        }

        [Fact]
        public async Task PutContributionPlan_ReturnsNoContent()
        {
            // Arrange
            InitializeDatabase();

            var contributionPlan = new ContributionPlan
            {
                Plan_Id = 1,
                Name = "Plan 1",
                Economic_Value = 100,
                AcademicPeriod = new AcademicPeriod { Academic_Period_Name = "2024" },
                State_Id = Constants.DEFAULT_STATE,
                Benefits = "Benefit 1"
            };

            _context.ContributionPlans.Add(contributionPlan);
            _context.SaveChanges();

            var updatedContributionPlanDTO = new CreateUpdateContributionPlanDTO
            {
                planName = "Updated Plan",
                Academic_Period_Name = "2024",
                price = 150,
                benefits = "Updated Benefits"
            };

            var academicPeriod = new AcademicPeriod { Academic_Period_Id = 6, Academic_Period_Name = "2026" };

            _context.AcademicPeriods.Add(academicPeriod);
            _context.SaveChanges();

            // Act
            var result = await _controller.PutContributionPlan(1, updatedContributionPlanDTO);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task PatchContributionPlanState_UpdatesStateToInactive()
        {
            // Arrange
            InitializeDatabase();

            var contributionPlan = new ContributionPlan
            {
                Plan_Id = 1,
                Name = "Plan 1",
                Economic_Value = 100,
                AcademicPeriod = new AcademicPeriod { Academic_Period_Name = "2024" },
                State_Id = Constants.DEFAULT_STATE,
                Benefits = "Benefit 1"
            };

            _context.ContributionPlans.Add(contributionPlan);
            _context.SaveChanges();

            // Act
            var result = await _controller.PatchContributionPlanState(1);

            // Assert
            var actionResult = Assert.IsType<NoContentResult>(result);
            var updatedPlan = await _context.ContributionPlans.FindAsync(1);

            Assert.Equal(Constants.STATE_INACTIVE, updatedPlan.State_Id);
        }
    }
}
