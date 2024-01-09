using HtmlAgilityPack;
using Microsoft.Extensions.Options;
using Nrrdio.Utilities.Web.Models.Errors;
using Nrrdio.Utilities.Web.Models.Options;
using Nrrdio.Utilities.Web.Requests;
using System.Net;

namespace Nrrdio.Utilities.Web;

public class GzipWebClient : WebClient {
	readonly GzipWebClientOptions Options;

	public GzipWebClient(
		IOptions<GzipWebClientOptions> options
	) {
		Options = options.Value;
	}

	public async Task<HtmlDocument?> DownloadDocument(string remoteUrl) {
		var data = await DownloadStringSafe(remoteUrl);

		HtmlDocument? returnObject = null;

		if (!string.IsNullOrEmpty(data)) {
			returnObject = new HtmlDocument();
			returnObject.LoadHtml(data);
		}

		return returnObject;
	}

	public async Task<T?> DownloadJSObject<T>(string remoteUrl, JsonSerializerOptions? options = null) {
		var data = await DownloadStringSafe(remoteUrl);

		var returnObject = default(T);

		if (data is { Length: > 0 }) {
			try {
				if (options is null) {
					returnObject = JsonSerializer.Deserialize<T>(data);
				}
				else {
					returnObject = JsonSerializer.Deserialize<T>(data, options);
				}
			}
			catch (JsonException) { }
			catch (NotSupportedException) { }
		}

		return returnObject;
	}

	public async Task<string> DownloadStringSafe(string remoteUrl) {
		remoteUrl = CleanUrl(remoteUrl);

		var data = string.Empty;

		try {
			data = await DownloadStringTaskAsync(remoteUrl);
		}
		catch (WebException e) when (e.Status == WebExceptionStatus.Timeout) {
			throw new HttpTimeoutError();
		}
		catch (UriFormatException) { }
		catch (AggregateException) { }

		return data;
	}

	public async Task<string> UploadJSObject(string url, string method, object data) {
		if (method is not { Length: > 0 }) {
			throw new ArgumentException($"{nameof(method)} must contain a value");
		}

		var remoteUrl = CleanUrl(url);
		var remoteUri = new Uri(remoteUrl);

		JsonSerializerOptions options = new JsonSerializerOptions { PropertyNamingPolicy = new PascalToSnakeNamingPolicy() };

		Headers[HttpRequestHeader.ContentType] = "application/json";

		var serialized = JsonSerializer.Serialize(data, options);

		return await Task.Run(() => UploadString(remoteUri, method, serialized));
	}

	protected override WebRequest GetWebRequest(Uri remoteUri) {
		ServicePointManager.SecurityProtocol = SecurityProtocolType.SystemDefault | SecurityProtocolType.Tls13;

		var request = (HttpWebRequest)base.GetWebRequest(remoteUri);

        request.UserAgent = Options.UserAgent;
		request.AllowAutoRedirect = true;
		request.MaximumAutomaticRedirections = 3;
		request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
		request.Timeout = Options.Timeout > 0 ? Options.Timeout : 5000;
		request.CookieContainer = new CookieContainer();

		return request;
	}

	string CleanUrl(string remoteUrl) {
		if (remoteUrl is not { Length: > 0 }) {
			throw new ArgumentException($"Argument {nameof(remoteUrl)} cannot be empty or null.");
		}

		return remoteUrl.Split('#')[0];
	}
}
