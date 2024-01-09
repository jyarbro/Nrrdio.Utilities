using System.Collections.Specialized;

namespace Nrrdio.Utilities.Web.Requests;

public class FormDataUrlEncodedSerializer {
	static JsonSerializerOptions Options { get; set; } = new JsonSerializerOptions();

    /// <summary>
    /// Converts a simple object into form data.
    /// </summary>
    /// <param name="namingPolicy">Used especially to format the names of properties.</param>
    public static NameValueCollection? Serialize(object obj, JsonNamingPolicy namingPolicy) {
		Options.PropertyNamingPolicy = namingPolicy;

		var serialized = JsonSerializer.Serialize(obj, Options);
		var deserialized = JsonSerializer.Deserialize<IDictionary<string, object>>(serialized);

		return deserialized?.Aggregate(new NameValueCollection(),
			(collection, kvp) => {
				collection.Add(HttpUtility.UrlEncode(kvp.Key), HttpUtility.UrlEncode(kvp.Value.ToString()));
				return collection;
			});
	}
}
