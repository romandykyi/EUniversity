using System.Reflection;
using System.Text.Json.Serialization;
using FluentValidation.AspNetCore;

namespace EUniversity.Extensions
{
	public static class MvcBuilderExtensions
	{
		public static IMvcBuilder ConfigureControllers(this IMvcBuilder builder)
		{
			return builder.Services
				.AddControllers()
				.AddJsonOptions(options =>
				{
					options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
				})
				.ConfigureApiBehaviorOptions(x => { x.SuppressMapClientErrors = true; });
		}
	}
}
