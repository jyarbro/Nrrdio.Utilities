namespace Nrrdio.Utilities.Web.Models.Options;

public class GzipWebClientOptions {
	public const string Section = "GzipWebClient";

	public string UserAgent { get; set; }
	public int Timeout { get; set; }
}
