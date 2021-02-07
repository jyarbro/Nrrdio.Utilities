using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text.Json;
using System.Web;

namespace Nrrdio.Utilities.Web.Requests {
    public class FormDataUrlEncodedSerializer {
        /// <summary>
        /// Converts a simple object into form data.
        /// </summary>
        /// <param name="namingPolicy">Used especially to format the names of properties.</param>
        public static NameValueCollection Serialize(object obj, JsonNamingPolicy namingPolicy = null) {
            JsonSerializerOptions options = null;

            if (namingPolicy is not null) {
                options = new JsonSerializerOptions { PropertyNamingPolicy = namingPolicy };
            }

            var serialized = JsonSerializer.Serialize(obj, options);
            var deserialized = JsonSerializer.Deserialize<IDictionary<string, object>>(serialized);

            return deserialized.Aggregate(new NameValueCollection(),
                (collection, kvp) => {
                    collection.Add(HttpUtility.UrlEncode(kvp.Key), HttpUtility.UrlEncode(kvp.Value.ToString()));
                    return collection;
                });
        }
    }
}
