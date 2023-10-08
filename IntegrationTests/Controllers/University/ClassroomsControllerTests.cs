using EUniversity.Core.Dtos.University;
using Microsoft.AspNetCore.Http;
using System.Net;

namespace EUniversity.IntegrationTests.Controllers.University
{
    public class ClassroomsControllerTests : ControllersTest
    {
        public static readonly CreateClassromDto ValidCreateClassroomDto = new("300");

        public const string GetByIdRoute = "/api/classrooms/{0}";
        public const string CreateRoute = "/api/classrooms";
        public const string UpdateRoute = "/api/classrooms/{0}";
        public const string DeleteRoute = "/api/classrooms/{0}";

    }
}
