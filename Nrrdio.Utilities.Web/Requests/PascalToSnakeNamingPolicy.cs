using Nrrdio.Utilities.Text;

namespace Nrrdio.Utilities.Web.Requests;

public class PascalToSnakeNamingPolicy : JsonNamingPolicy {
	public override string ConvertName(string name) => name.PascalToSnake();

	public static JsonSerializerOptions Options => new JsonSerializerOptions {
		PropertyNamingPolicy = new PascalToSnakeNamingPolicy()
	};
}
