using Bogus;
using EUniversity.Core.Dtos.Auth;
using EUniversity.Core.Models;
using EUniversity.Core.Models.University;
using EUniversity.Core.Models.University.Grades;
using EUniversity.Core.Policy;
using EUniversity.Core.Services.Auth;
using EUniversity.Infrastructure.Data;
using EUniversity.Infrastructure.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;

namespace EUniversity.Infrastructure.Services.Test;

/// <summary>
/// Service that creates test data. 
/// <b>Should not be used in the production!</b>
/// </summary>
public class TestDataService
{
    private readonly IAuthService _authService;
    private readonly ApplicationDbContext _dbContext;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<TestDataService> _logger;

    /// <summary>
    /// Password that is assigned to all test users(but not for fake users).
    /// </summary>
    public const string DefaultPassword = "Password1!";
    /// <summary>
    /// Seed for all fake data generations.
    /// </summary>
    public const int FakeDataGeneratorSeed = 70222500;

    public TestDataService(IAuthService authService, ApplicationDbContext dbContext,
        UserManager<ApplicationUser> userManager, ILogger<TestDataService> logger)
    {
        _authService = authService;
        _dbContext = dbContext;
        _userManager = userManager;
        _logger = logger;
        Randomizer.Seed = new(FakeDataGeneratorSeed);
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
    /// Creates many fake teachers and students.
    /// </summary>
    /// <param name="teachers">Number of teachers to be created.</param>
    /// <param name="students">Number of studetns to be created.</param>
    public async Task CreateFakeUsersAsync(int teachers = 50, int students = 200)
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
        _logger.LogInformation("Generating {students} students", students);
        await _authService.RegisterManyAsync(usersFaker.GenerateLazy(students), Roles.Student)
            .ToListAsync();

        _logger.LogInformation("Users generation has been finished");
    }

    private async Task CreateFakeEntitiesAsync<T>(Faker<T> faker, int count, bool skipable = true)
        where T : class
    {
        string typeName = typeof(T).Name;

        if (skipable)
        {
            // Do not generate entities if there are enough of them already
            int entitiesCount = await _dbContext.Set<T>().CountAsync();
            if (entitiesCount >= count)
            {
                _logger.LogInformation("Generation of '{typeName}' entities was skipped:" +
                    " There are {entitiesCount} entities of this type already",
                    typeName, entitiesCount);
                return;
            }
        }

        _dbContext.AddRange(faker.GenerateLazy(count));
        await _dbContext.SaveChangesAsync();

        _logger.LogInformation("{count} entities of type '{typeName}' have been generated",
            count, typeName);
    }

    /// <summary>
    /// Creates many fake classrooms.
    /// </summary>
    /// <param name="count">Number of classrooms to be created.</param>
    public async Task CreateFakeClassroomsAsync(int count = 70)
    {
        var classroomsFaker = new Faker<Classroom>()
            .RuleFor(c => c.Name, f =>
                new string(f.Random.Chars('A', 'Z', f.Random.Number(0, 3))) +
                new string(f.Random.Chars('0', '9', 5)))
            .RuleForDates();

        await CreateFakeEntitiesAsync(classroomsFaker, count);
    }

    /// <summary>
    /// Creates many fake grades within the range from 1 to count.
    /// </summary>
    public async Task CreateFakeGradesAsync(int count = 10)
    {
        int grade = 1;
        var gradesFaker = new Faker<Grade>()
            .RuleFor(g => g.Score, f => grade++)
            .RuleFor(g => g.Name, (f, g) => g.Score.ToString())
            .RuleForDates();

        await CreateFakeEntitiesAsync(gradesFaker, count);
    }

    /// <summary>
    /// Creates many fake courses.
    /// </summary>
    /// <param name="count">Number of courses to be created.</param>
    public async Task CreateFakeCoursesAsync(int count = 50)
    {
        // IDs of semesters
        var teachersIds = await _dbContext.Semesters
            .Select(c => c.Id)
            .ToArrayAsync();

        var coursesFaker = new Faker<Course>()
            .RuleFor(c => c.Name, f => f.Company.CatchPhrase())
            .RuleFor(c => c.Description, f => f.Lorem.Sentences(f.Random.Number(1, 3), " "))
            // Random teacher ID(90% probabilty) or null
            .RuleFor(c => c.SemesterId, f => f.Random.Bool(0.9f) ? f.Random.CollectionItem(teachersIds) : null)
            .RuleForDates();

        await CreateFakeEntitiesAsync(coursesFaker, count);
    }

    /// <summary>
    /// Creates many fake groups.
    /// </summary>
    /// <param name="count">Number of groups to be created.</param>
    /// <param name="minStudentsInGroup">Minimum number of students in one group.</param>
    /// <param name="maxStudentsInGroup">Maximum number of students in one group.</param>
    public async Task CreateFakeGroupsAsync(int count = 150,
        int minStudentsInGroup = 15, int maxStudentsInGroup = 30)
    {
        Faker faker = new();

        // IDs of teachers
        var teachersIds = (await _userManager.GetUsersInRoleAsync(Roles.Teacher))
            .Select(u => u.Id)
            .ToArray();
        // IDs of courses
        var coursesIds = _dbContext.Courses
            .Select(c => c.Id)
            .ToArray();

        // Generate groups
        var groupsFaker = new Faker<Group>()
            .RuleFor(g => g.Name, f => f.Random.AlphaNumeric(6))
            .RuleFor(g => g.CourseId, f => f.Random.CollectionItem(coursesIds))
            .RuleFor(g => g.TeacherId, f => f.Random.CollectionItem(teachersIds))
            .RuleForDates();
        await CreateFakeEntitiesAsync(groupsFaker, count);

        // Get all groups
        var groupsIds = _dbContext.Groups
            .Select(g => g.Id)
            .ToArray();

        // Assign random students to groups
        var studentsIds = (await _userManager.GetUsersInRoleAsync(Roles.Student))
            .Select(u => u.Id)
            .ToArray();
        foreach (var groupId in groupsIds)
        {
            // Do not assign students if group has at least one assigned students
            // or with 10% probability
            if (await _dbContext.StudentGroups.AnyAsync(sg => sg.GroupId == groupId) ||
                faker.Random.Bool(0.1f))
            {
                continue;
            }

            var shuffledStudentsIds = faker.Random
                .Shuffle(studentsIds)
                .ToArray();
            int i = 0;
            var studentGroupFaker = new Faker<StudentGroup>()
                .RuleFor(s => s.GroupId, _ => groupId)
                .RuleFor(s => s.StudentId, _ => shuffledStudentsIds[i++])
                .RuleFor(s => s.EnrollmentDate, f => f.Date.Recent(180));
            int studentsCount = faker.Random.Int(minStudentsInGroup, maxStudentsInGroup);
            await CreateFakeEntitiesAsync(studentGroupFaker, studentsCount, false);
        }
    }

    /// <summary>
    /// Creates many fake semesters.
    /// </summary>
    /// <param name="count">Number of semesters to be created.</param>
    /// <param name="minStudentsInSemester">Minimum number of students in one semester.</param>
    /// <param name="maxStudentsInSemester">Maximum number of students in one semester.</param>
    public async Task CreateFakeSemestersAsync(int count = 8,
        int minStudentsInSemester = 20, int maxStudentsInSemester = 40)
    {
        Faker faker = new();

        // Generate semesters
        int number = 1;
        DateTime minDate = DateTime.Now - TimeSpan.FromDays(180);
        DateTime maxDate = DateTime.Now + TimeSpan.FromDays(180);
        var semestersFaker = new Faker<Semester>()
            .RuleFor(s => s.DateFrom, f => f.Date.Between(minDate, maxDate))
            .RuleFor(s => s.DateTo, (f, s) => f.Date.Between(s.DateFrom.DateTime,
            s.DateFrom.DateTime + TimeSpan.FromDays(180)))
            .RuleFor(s => s.Name, (f, s) => $"Semester {number++} {s.DateFrom:yyyy-MM-dd}-{s.DateTo:yyyy-MM-dd}")
            .RuleForDates();
        await CreateFakeEntitiesAsync(semestersFaker, count);

        // Get all semesters
        var semestersIds = _dbContext.Semesters
            .Select(s => s.Id)
            .ToArray();

        // Assign random students to semesters
        var studentsIds = (await _userManager.GetUsersInRoleAsync(Roles.Student))
            .Select(u => u.Id)
            .ToArray();
        foreach (var semesterId in semestersIds)
        {
            // Do not assign students if semester has at least one assigned students
            if (await _dbContext.StudentSemesters.AnyAsync(sg => sg.SemesterId == semesterId) ||
                faker.Random.Bool(0.1f))
            {
                continue;
            }

            var shuffledStudentsIds = faker.Random
                .Shuffle(studentsIds)
                .ToArray();
            int i = 0;
            var studentSemesterFaker = new Faker<StudentSemester>()
                .RuleFor(s => s.SemesterId, _ => semesterId)
                .RuleFor(s => s.StudentId, _ => shuffledStudentsIds[i++])
                .RuleFor(s => s.EnrollmentDate, f => f.Date.Recent(180));
            int studentsCount = faker.Random.Int(minStudentsInSemester, maxStudentsInSemester);
            await CreateFakeEntitiesAsync(studentSemesterFaker, studentsCount, false);
        }
    }

    /// <summary>
    /// Creates test classes types.
    /// </summary>
    /// <param name="count">Number of classes to be created.</param>
    public async Task CreateTestClassesTypesAsync()
    {
        // Names of class types
        string[] names =
        {
            "Lecture", "Practical", "Laboratory", "Tutorial",
            "Seminar", "Workshop"
        };

        int entitiesCount = await _dbContext.ClassTypes.CountAsync();
        if (entitiesCount >= names.Length)
        {
            _logger.LogInformation("Generation of 'ClassType' entities was skipped:" +
                " There are {entitiesCount} entities of this type already",
                entitiesCount);
            return;
        }

        // Some random date to fill creation and update dates of entities
        DateTimeOffset date = new(2005, 3, 4, 12, 49, 12, 696, 3, TimeSpan.FromHours(2));
        foreach (string name in names)
        {
            _dbContext.Add(new ClassType { Name = name, CreationDate = date, UpdateDate = date });
        }
        await _dbContext.SaveChangesAsync();

        _logger.LogInformation("{count} entities of type 'ClassType' have been added",
            names.Length);
    }

    /// <summary>
    /// Creates many fake classes.
    /// </summary>
    /// <param name="count">Number of classes to be created.</param>
    public async Task CreateFakeClassesAsync(int count = 200)
    {
        Faker faker = new();

        // All possible classes durations
        TimeSpan[] durations =
        {
            TimeSpan.FromMinutes(45),
            TimeSpan.FromHours(1.0),
            TimeSpan.FromHours(1.5),
            TimeSpan.FromHours(2)
        };
        // All possible start minutes of classes(without date)
        int[] startMinutes = { 0, 15, 30, 45 };

        // IDs of classes types
        var classTypesIds = _dbContext.ClassTypes
            .Select(c => c.Id)
            .ToArray();
        // IDs of teachers
        var teachersIds = (await _userManager.GetUsersInRoleAsync(Roles.Teacher))
            .Select(u => u.Id)
            .ToArray();
        // IDs of classrooms
        var classroomsIds = _dbContext.Classrooms
            .Select(c => c.Id)
            .ToArray();
        // IDs of groups
        var groupsIds = _dbContext.Groups
            .Select(c => c.Id)
            .ToArray();

        // Generate classes
        var classesFaker = new Faker<Class>()
            .RuleFor(g => g.ClassTypeId, f => f.Random.CollectionItem(classTypesIds))
            .RuleFor(g => g.ClassroomId, f => f.Random.CollectionItem(classroomsIds))
            .RuleFor(g => g.GroupId, f => f.Random.CollectionItem(groupsIds))
            // Set a substitute teacher with 5% probability
            .RuleFor(g => g.SubstituteTeacherId,
                f => f.Random.Bool(0.05f) ? f.Random.CollectionItem(teachersIds) : null)
            .RuleFor(g => g.Duration, f => f.Random.CollectionItem(durations))
            .RuleFor(g => g.StartDate, f =>
                f.Date.SoonOffset(180) +
                TimeSpan.FromHours(f.Random.Number(8, 18)) +
                TimeSpan.FromMinutes(f.Random.CollectionItem(startMinutes)))
            .RuleForDates();
        await CreateFakeEntitiesAsync(classesFaker, count);
    }
}
