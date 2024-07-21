using AutoMapper;
using backendfepon.Controllers;
using backendfepon.Data;
using backendfepon.DTOs.AdmnistrativeMemberDTOs;
using backendfepon.ModelConfigurations.Profiles;
using backendfepon.Models;
using backendfepon.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestTIC
{
    public class AdministrativeMemberControllerTests
    {
        private readonly DbContextOptions<ApplicationDbContext> _options;
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _context;
        private readonly AdministrativeMembersController _controller;

        public AdministrativeMemberControllerTests()
        {
            _options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new ApplicationDbContext(_options);

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<AdministrativeMemberProfile>();
            });

            _mapper = config.CreateMapper();

            // Configure logging
            var serviceProvider = new ServiceCollection()
                .AddLogging(builder => builder
                    .AddConsole()
                    .SetMinimumLevel(LogLevel.Information))
                .BuildServiceProvider();


            _controller = new AdministrativeMembersController(_context, _mapper);
            InitializeDatabase();
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
        public async Task GetAdministrativesMembers_ReturnsAdministrativeMembers_WhenAdministrativeMembersExist()
        {
            // Arrange
            InitializeDatabase();

            var adminMember1 = new AdministrativeMember
            {
                Administrative_Member_Id = 1,
                Name = "John",
                Last_Name = "Doe",
                Birth_Date = DateTime.ParseExact("01/01/2000", "dd/MM/yyyy", CultureInfo.InvariantCulture),
                Phone = "1234567890",
                Faculty = new Faculty { Faculty_Name = "Science" },
                Career = new Career { Career_Name = "Engineering" },
                Semester = new Semester { Semester_Name = "First" },
                Email = "john.doe@example.com",
                Role = new Role { Role_Name = "Admin" },
                State_Id = Constants.DEFAULT_STATE
            };

            var adminMember2 = new AdministrativeMember
            {
                Administrative_Member_Id = 2,
                Name = "Jane",
                Last_Name = "Smith",
                Birth_Date = DateTime.ParseExact("02/02/2001", "dd/MM/yyyy", CultureInfo.InvariantCulture),
                Phone = "0987654321",
                Faculty = new Faculty { Faculty_Name = "Arts" },
                Career = new Career { Career_Name = "History" },
                Semester = new Semester { Semester_Name = "Second" },
                Email = "jane.smith@example.com",
                Role = new Role { Role_Name = "Admin" },
                State_Id = Constants.DEFAULT_STATE
            };

            _context.AdministrativeMembers.AddRange(adminMember1, adminMember2);
            _context.SaveChanges();

            // Act
            var result = await _controller.GetAdministrativesMembers();

            // Assert
            var actionResult = Assert.IsType<ActionResult<IEnumerable<AdministrativeMemberDTO>>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var returnedMembers = Assert.IsType<List<AdministrativeMemberDTO>>(okResult.Value);

            Assert.Equal(2, returnedMembers.Count);
        }

        [Fact]
        public async Task GetAdministrativeMember_ReturnsNotFound_WhenAdministrativeMemberDoesNotExist()
        {
            // Arrange
            InitializeDatabase();

            // Act
            var result = await _controller.GetAdministrativeMember(100);

            // Assert
            var actionResult = Assert.IsType<ActionResult<AdministrativeMemberDTO>>(result);
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(actionResult.Result);

            Assert.Equal("Miembro administrativo no encontrado.", ((dynamic)notFoundResult.Value).Message);
        }

        [Fact]
        public async Task PostAdministrativeMember_ReturnsCreatedAdministrativeMember()
        {
            // Arrange
            InitializeDatabase();

            var adminMemberDTO = new CreateUpdateAdministrativeMemberDTO
            {
                FirstName = "John",
                LastName = "Doe",
                BirthDate = DateTime.ParseExact("01/01/2000", "dd/MM/yyyy", CultureInfo.InvariantCulture),
                Cellphone = "1234567890",
                Faculty = "Science",
                Career = "Engineering",
                Semester = "First",
                Email = "john.doe@example.com",
                Position = "Admin"
            };

            var faculty = new Faculty { Faculty_Id = 1, Faculty_Name = "Science" };
            var career = new Career { Career_Id = 1, Career_Name = "Engineering" };
            var semester = new Semester { Semester_Id = 1, Semester_Name = "First" };
            var role = new Role { Role_Id = 1, Role_Name = "Admin" };

            _context.Faculties.Add(faculty);
            _context.Careers.Add(career);
            _context.Semesters.Add(semester);
            _context.Roles.Add(role);
            _context.SaveChanges();

            try
            {

                // Act
                var result = await _controller.PostAdministrativeMember(adminMemberDTO);

                // Assert
                var actionResult = Assert.IsType<ActionResult<AdministrativeMemberDTO>>(result);
                if (!(actionResult.Result is CreatedAtActionResult))
                {
                    var objectResult = actionResult.Result as ObjectResult;
                    var errorMessage = objectResult?.Value?.ToString() ?? "No error message";
                    Assert.True(false, $"Unexpected result type: {actionResult.Result.GetType().Name}, Error: {errorMessage}");
                }
                Assert.IsType<CreatedAtActionResult>(actionResult.Result);

                var createdAtActionResult = (CreatedAtActionResult)actionResult.Result;
                var returnedMember = Assert.IsType<AdministrativeMemberDTO>(createdAtActionResult.Value);

                Assert.Equal(adminMemberDTO.FirstName, returnedMember.firstName);
                Assert.Equal(adminMemberDTO.LastName, returnedMember.lastName);
                Assert.Equal(adminMemberDTO.Email, returnedMember.email);
            }
            catch (Exception ex)
            {
                Assert.True(false, ex.Message);
            }
        }

        [Fact]
        public async Task PutAdministrativeMember_ReturnsNoContent()
        {
            // Arrange
            InitializeDatabase();

            var adminMember = new AdministrativeMember
            {
                Administrative_Member_Id = 1,
                Name = "John",
                Last_Name = "Doe",
                Birth_Date = DateTime.ParseExact("01/01/2000", "dd/MM/yyyy", CultureInfo.InvariantCulture),
                Phone = "1234567890",
                Faculty = new Faculty { Faculty_Name = "Science" },
                Career = new Career { Career_Name = "Engineering" },
                Semester = new Semester { Semester_Name = "First" },
                Email = "john.doe@example.com",
                Role = new Role { Role_Name = "Admin" },
                State_Id = Constants.DEFAULT_STATE
            };

            _context.AdministrativeMembers.Add(adminMember);
            _context.SaveChanges();

            var updatedAdminMemberDTO = new CreateUpdateAdministrativeMemberDTO
            {
                FirstName = "John Updated",
                LastName = "Doe Updated",
                BirthDate = DateTime.ParseExact("01/01/2000", "dd/MM/yyyy", CultureInfo.InvariantCulture),
                Cellphone = "1234567890",
                Faculty = "Science",
                Career = "Engineering",
                Semester = "First",
                Email = "john.doe.updated@example.com",
                Position = "Admin"
            };

            var faculty = new Faculty { Faculty_Id = 5, Faculty_Name = "Medicine" };
            var career = new Career { Career_Id = 4, Career_Name = "Biology" };
            var semester = new Semester { Semester_Id = 5, Semester_Name = "First" };
            var role = new Role { Role_Id = 6, Role_Name = "Normal" };

            _context.Faculties.Add(faculty);
            _context.Careers.Add(career);
            _context.Semesters.Add(semester);
            _context.Roles.Add(role);
            _context.SaveChanges();

            // Act
            var result = await _controller.PutAdministrativeMember(1, updatedAdminMemberDTO);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteAdministrativeMember_ReturnsNoContent()
        {
            // Arrange
            InitializeDatabase();

            var adminMember = new AdministrativeMember
            {
                Administrative_Member_Id = 4,
                Name = "John",
                Last_Name = "Doe",
                Birth_Date = DateTime.ParseExact("01/01/2000", "dd/MM/yyyy", CultureInfo.InvariantCulture),
                Phone = "1234567890",
                Faculty = new Faculty { Faculty_Name = "Science" },
                Career = new Career { Career_Name = "Engineering" },
                Semester = new Semester { Semester_Name = "First" },
                Email = "john.doe@example.com",
                Role = new Role { Role_Name = "Admin" },
                State_Id = Constants.DEFAULT_STATE
            };

            _context.AdministrativeMembers.Add(adminMember);
            _context.SaveChanges();

            // Act
            var result = await _controller.DeleteAdministrativeMember(4);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task PatchAdministrativeMemberState_UpdatesStateToInactive()
        {
            // Arrange
            InitializeDatabase();

            var adminMember = new AdministrativeMember
            {
                Administrative_Member_Id = 1,
                Name = "John",
                Last_Name = "Doe",
                Birth_Date = DateTime.ParseExact("01/01/2000", "dd/MM/yyyy", CultureInfo.InvariantCulture),
                Phone = "1234567890",
                Faculty = new Faculty { Faculty_Name = "Science" },
                Career = new Career { Career_Name = "Engineering" },
                Semester = new Semester { Semester_Name = "First" },
                Email = "john.doe@example.com",
                Role = new Role { Role_Name = "Admin" },
                State_Id = Constants.DEFAULT_STATE
            };

            _context.AdministrativeMembers.Add(adminMember);
            _context.SaveChanges();

            // Act
            var result = await _controller.PatchProductState(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
            var updatedMember = await _context.AdministrativeMembers.FindAsync(1);

            Assert.Equal(Constants.STATE_INACTIVE, updatedMember.State_Id);
        }
    }
}
