using Bogus;
using EUniversity.Core.Dtos.Auth;
using EUniversity.Core.Dtos.University;
using EUniversity.Core.Policy;
using EUniversity.Core.Services;
using EUniversity.Infrastructure.Services.University;

namespace EUniversity.Infrastructure.Services
{
    /// <summary>
    /// Service that creates test data. 
    /// <b>Should not be used in the production!</b>
    /// </summary>
    public class TestDataService
    {
        private readonly IAuthService _authService;
        private readonly IClassroomsService _classroomsService;

        /// <summary>
        /// Password that is assigned to all test users(but not for fake users).
        /// </summary>
        public const string DefaultPassword = "Password1!";
        /// <summary>
        /// Seed for all fake data generations.
        /// </summary>
        public const int FakeDataGeneratorSeed = 70222500;

        public TestDataService(IAuthService authService, IClassroomsService classroomsService)
        {
            _authService = authService;
            _classroomsService = classroomsService;
        }

        /// <summary>
        /// Creates one test student and one test teacher.
        /// </summary>
        public async Task CreateTestUsers()
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
        public async Task CreateRandomUsers(int teachers = 50, int students = 200)
        {
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
            await _authService.RegisterManyAsync(usersFaker.GenerateLazy(teachers), Roles.Teacher)
                .ToListAsync();
            // Register students
            await _authService.RegisterManyAsync(usersFaker.GenerateLazy(students), Roles.Student)
                .ToListAsync();
        }

        /// <summary>
        /// Creates many random classrooms.
        /// </summary>
        /// <param name="classrooms">Number of classrooms to be created.</param>
        public async Task CreateRandomClassrooms(int classrooms = 70)
        {
            Randomizer.Seed = new(FakeDataGeneratorSeed);
            var classroomsFaker = new Faker<CreateClassromDto>()
                .CustomInstantiator(f => new CreateClassromDto(
                    new string(f.Random.Chars('A', 'Z', f.Random.Number(0, 3))) +
                    new string(f.Random.Chars('0', '9', 5))
                    ));

            foreach (var classroom in classroomsFaker.GenerateLazy(classrooms))
            {
                await _classroomsService.CreateAsync(classroom);
            }
        }
    }
}
