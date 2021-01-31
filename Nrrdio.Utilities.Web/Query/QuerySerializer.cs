using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Web;

namespace Nrrdio.Utilities.Web.Query {
    public class QuerySerializer {
        /// <summary>
        /// Converts a simple object into a query string.
        /// </summary>
        /// <param name="namingPolicy">Used especially to format the names of properties.</param>
        public static string Serialize(object obj, JsonNamingPolicy namingPolicy = null) {
            JsonSerializerOptions options = null;

            if (namingPolicy is not null) {
                options = new JsonSerializerOptions { PropertyNamingPolicy = namingPolicy };
            }

            var serialized = JsonSerializer.Serialize(obj, options);
            var deserialized = JsonSerializer.Deserialize<IDictionary<string, object>>(serialized);
            var query = deserialized.Select(o => $"{HttpUtility.UrlEncode(o.Key)}={HttpUtility.UrlEncode(o.Value.ToString())}");

            return string.Join("&", query);
        }
    }
}
