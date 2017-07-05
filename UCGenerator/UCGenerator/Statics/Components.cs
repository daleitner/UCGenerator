using System;

namespace UCGenerator.Statics
{
	public enum TypeEnum
	{
		TextBox,
		Label,
		ComboBox,
		ListBox,
		Button,
		ContentPresenter,
		ItemsControl
	}
	public class Components
	{
		private static readonly string[] TextBoxBindings = {"Text", "Property-Changed-Trigger", "IsEnabled", "Visibility", "Key-Binding"};
		private static readonly string[] LabelBindings = {"Content", "IsEnabled", "Visibility"};
		private static readonly string[] ComboBoxBindings = { "ItemsSource", "SelectedItem", "IsEnabled", "Visibility"};
		private static readonly string[] ListBoxBindings = { "ItemsSource", "SelectedItem", "IsEnabled", "Visibility", "Key-Binding", "Doubleclick" };
		private static readonly string[] ButtonBindings = { "Content", "Command", "Visibility" };
		private static readonly string[] ContentPresenterBindings = { "Content", "Visibility" };
		private static readonly string[] ItemsControlBindings = { "ItemsSource", "Visibility" };

		public static string[] GetBindings(TypeEnum type)
		{
			switch (type)
			{
				case TypeEnum.TextBox:
					return TextBoxBindings;
				case TypeEnum.Label:
					return LabelBindings;
				case TypeEnum.ComboBox:
					return ComboBoxBindings;
				case TypeEnum.ListBox:
					return ListBoxBindings;
				case TypeEnum.Button:
					return ButtonBindings;
				case TypeEnum.ContentPresenter:
					return ContentPresenterBindings;
				case TypeEnum.ItemsControl:
					return ItemsControlBindings;
				default:
					throw new ArgumentOutOfRangeException(nameof(type), type, null);
			}
		}
	}
}
