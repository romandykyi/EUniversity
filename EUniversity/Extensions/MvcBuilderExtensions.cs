using System.Text.Json.Serialization;

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
