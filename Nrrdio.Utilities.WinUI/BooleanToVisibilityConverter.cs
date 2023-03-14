namespace Nrrdio.Utilities.WinUI;

public class BooleanToVisibilityConverter : IValueConverter {
	public object Convert(object value, Type targetType, object parameter, string language)
		=> value is bool castedValue
		   && castedValue ? Visibility.Visible : (object) Visibility.Collapsed;

	public object ConvertBack(object value, Type targetType, object parameter, string language)
		=> value is Visibility castedValue
		   && castedValue == Visibility.Visible;
}
