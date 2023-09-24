using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace EUniversity.Tests.Extensions
{
	public static class AssertionExtensions
	{
		public static int? ResponseCode(this Task<IStatusCodeActionResult> actionResultTask)
		{
			return actionResultTask.Result.StatusCode;
		}
	}
}
