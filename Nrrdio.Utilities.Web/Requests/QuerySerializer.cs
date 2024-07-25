namespace Nrrdio.Utilities.Web.Requests;

public class QuerySerializer {
	/// <summary>
    /// Converts a simple object into a query string.
    /// </summary>
    /// <param name="namingPolicy">Used especially to format the names of properties.</param>
    public static string Serialize(object obj, JsonNamingPolicy? namingPolicy = null) {
        var options = new JsonSerializerOptions { PropertyNamingPolicy = namingPolicy };

		var serialized = JsonSerializer.Serialize(obj, options);
		var deserialized = JsonSerializer.Deserialize<IDictionary<string, object>>(serialized);
		var query = deserialized?.Select(o => $"{HttpUtility.UrlEncode(o.Key)}={HttpUtility.UrlEncode(o.Value.ToString())}");

        var value = "";

        if (query is not null) {
            value = string.Join("&", query);
        }

        return value;
	}
}
