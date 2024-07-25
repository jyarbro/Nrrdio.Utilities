using Microsoft.AspNetCore.Razor.TagHelpers;
using Nrrdio.Utilities.Extensions;

namespace Nrrdio.Utilities.Web.TagHelpers;
public class PassedTimeTagHelper : TagHelper {
	public DateTime Time { get; set; }

	public override void Process(TagHelperContext context, TagHelperOutput output) {
		output.TagName = "time";
		output.TagMode = TagMode.StartTagAndEndTag;
		output.Attributes.SetAttribute("datetime", Time.ToLocalTimeString());
		output.Attributes.SetAttribute("title", Time);

		if (output.Content.GetContent() is not { Length: > 0 }) {
			output.Content.SetContent(Time.ToPassedTimeString());
		}
	}
}
