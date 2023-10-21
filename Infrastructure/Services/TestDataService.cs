using Bogus;
using EUniversity.Core.Dtos.Auth;
using EUniversity.Core.Dtos.University;
using EUniversity.Core.Models;
using EUniversity.Core.Models.University;
using EUniversity.Core.Models.University.Grades;
using EUniversity.Core.Policy;
using EUniversity.Core.Services;
using EUniversity.Infrastructure.Data;
using EUniversity.Infrastructure.Services.University;
using Microsoft.Extensions.Logging;

namespace EUniversity.Infrastructure.Services
{
    /// <summary>
    /// Service that creates test data. 
    /// <b>Should not be used in the production!</b>
    /// </summary>
    public class TestDataService
    {
        private readonly IAuthService _authService;
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<TestDataService> _logger;

        /// <summary>
        /// Password that is assigned to all test users(but not for fake users).
        /// </summary>
        public const string DefaultPassword = "Password1!";
        /// <summary>
        /// Seed for all fake data generations.
        /// </summary>
        public const int FakeDataGeneratorSeed = 70222500;

        public TestDataService(IAuthService authService,
            ApplicationDbContext dbContext, ILogger<TestDataService> logger)
        {
            _authService = authService;
            _dbContext = dbContext;
            _logger = logger;
        }

        /// <summary>
        /// Creates one test student and one test teacher.
        /// </summary>
        public async Task CreateTestUsersAsync()
        {
            const string studentUserName = "student";
            const string teacherUserName = "teacher";
            RegisterDto studentRegisterDto = new("test-student@e-university.com", "Jesse", "Pinkman", "Bruce");
            RegisterDto teacherRegisterDto = new("test-teacher@e-university.com", "Walter", "White", "Hartwell");

            await _authService.RegisterAsync(studentRegisterDto,
                studentUserName, DefaultPassword, Roles.Student);

            await _authService.RegisterAsync(teacherRegisterDto,
                teacherUserName, DefaultPassword, Roles.Teacher);
        }

        /// <summary>
        /// Creates many random teachers and students.
        /// </summary>
        /// <param name="teachers">Number of teachers to be created.</param>
        /// <param name="students">Number of studetns to be created.</param>
        public async Task CreateRandomUsersAsync(int teachers = 50, int students = 200)
        {
            // Do not generate users if there are enough of them already
            int usersCount = await _dbContext.Users.CountAsync();
            if (usersCount >= teachers + students)
            {
                _logger.LogInformation("Users generation was skipped: There are {usersCount} users already", usersCount);
                return;
            }

            _logger.LogInformation("Users generation has been started");

            static RegisterDto GenerateUser(Faker faker)
            {
                string firstName = faker.Name.FirstName();
                string lastName = faker.Name.LastName();
                string email = faker.Internet.Email(firstName, lastName);
                // Add middle name with 15% probabilty:
                string? middleName = faker.Random.Bool(0.15f) ?
                    faker.Name.FirstName() : null;

                return new(email, firstName, lastName, middleName);
            }

            Randomizer.Seed = new(FakeDataGeneratorSeed);
            var usersFaker = new Faker<RegisterDto>()
                .CustomInstantiator(GenerateUser);

            // Register teachers
            _logger.LogInformation("Generating {teachers} teachers", teachers);
            await _authService.RegisterManyAsync(usersFaker.GenerateLazy(teachers), Roles.Teacher)
                .ToListAsync();
            // Register students
            _logger.LogInformation("Generating {students} teachers", students);
            await _authService.RegisterManyAsync(usersFaker.GenerateLazy(students), Roles.Student)
                .ToListAsync();

            _logger.LogInformation("Users were generated");
        }

        private async Task CreateRandomEntitiesAsync<T>(Faker<T> faker, int count) where T : class
        {
            string typeName = typeof(T).Name;

            // Do not generate entities if there are enough of them already
            int entitiesCount = await _dbContext.Set<T>().CountAsync();
            if (entitiesCount >= count)
            {
                _logger.LogInformation("Generation of '{typeName}' entities was skipped:" +
                    " There are {entitiesCount} entities of this type already",
                    typeName, entitiesCount);
                return;
            }

            Randomizer.Seed = new(FakeDataGeneratorSeed);

            foreach (var entity in faker.GenerateLazy(count))
            {
                _dbContext.Add(entity);
            }
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation("{count} entities of type '{typeName}' have been generated",
                count, typeName);
        }

        /// <summary>
        /// Creates many random classrooms.
        /// </summary>
        /// <param name="count">Number of classrooms to be created.</param>
        public async Task CreateRandomClassroomsAsync(int count = 70)
        {
            var classroomsFaker = new Faker<Classroom>()
                .RuleFor(c => c.Name, f =>
                    new string(f.Random.Chars('A', 'Z', f.Random.Number(0, 3))) +
                    new string(f.Random.Chars('0', '9', 5)));

            await CreateRandomEntitiesAsync(classroomsFaker, count);
        }

        /// <summary>
        /// Creates many test grades within the range from 1 to count.
        /// </summary>
        public async Task CreateTestGradesAsync(int count = 10)
        {
            int grade = 1;
            var gradesFaker = new Faker<Grade>()
                .RuleFor(g => g.Score, f => grade++)
                .RuleFor(g => g.Name, (f, g) => g.Score.ToString());

            await CreateRandomEntitiesAsync(gradesFaker, count);
        }
    }
}
