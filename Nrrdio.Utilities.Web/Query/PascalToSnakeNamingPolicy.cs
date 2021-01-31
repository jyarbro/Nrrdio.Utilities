using Nrrdio.Utilities.Text;
using System.Text.Json;

namespace Nrrdio.Utilities.Web.Query {
    public class PascalToSnakeNamingPolicy : JsonNamingPolicy {
        public override string ConvertName(string name) => name.PascalToSnake();

        public static JsonSerializerOptions Options => new JsonSerializerOptions {
            PropertyNamingPolicy = new PascalToSnakeNamingPolicy()
        };
    }
}
