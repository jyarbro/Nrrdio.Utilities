namespace Nrrdio.Utilities.Web.Requests;

public class QuerySerializer {
	static JsonSerializerOptions Options { get; set; } = new JsonSerializerOptions();

    /// <summary>
    /// Converts a simple object into a query string.
    /// </summary>
    /// <param name="namingPolicy">Used especially to format the names of properties.</param>
    public static string Serialize(object obj, JsonNamingPolicy? namingPolicy = null) {
        Options.PropertyNamingPolicy = namingPolicy;

		var serialized = JsonSerializer.Serialize(obj, Options);
		var deserialized = JsonSerializer.Deserialize<IDictionary<string, object>>(serialized);
		var query = deserialized?.Select(o => $"{HttpUtility.UrlEncode(o.Key)}={HttpUtility.UrlEncode(o.Value.ToString())}");

        var value = "";

        if (query is not null) {
            value = string.Join("&", query);
        }

        return value;
	}
}
