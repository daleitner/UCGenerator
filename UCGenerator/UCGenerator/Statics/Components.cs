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

	public enum BindingEnum
	{
		Text,
		PropertyChangedTrigger,
		IsEnabled,
		Visibility,
		KeyBinding,
		Content,
		ItemsSource,
		SelectedItem,
		DoubleClick,
		Command
	}
	public class Components
	{
		private static readonly BindingEnum[] TextBoxBindings = {BindingEnum.Text, BindingEnum.PropertyChangedTrigger, BindingEnum.IsEnabled, BindingEnum.Visibility, BindingEnum.KeyBinding};
		private static readonly BindingEnum[] LabelBindings = {BindingEnum.Content, BindingEnum.IsEnabled, BindingEnum.Visibility};
		private static readonly BindingEnum[] ComboBoxBindings = { BindingEnum.ItemsSource, BindingEnum.SelectedItem, BindingEnum.IsEnabled, BindingEnum.Visibility };
		private static readonly BindingEnum[] ListBoxBindings = { BindingEnum.ItemsSource, BindingEnum.SelectedItem, BindingEnum.IsEnabled, BindingEnum.Visibility, BindingEnum.KeyBinding, BindingEnum.DoubleClick };
		private static readonly BindingEnum[] ButtonBindings = { BindingEnum.Content, BindingEnum.Command, BindingEnum.Visibility };
		private static readonly BindingEnum[] ContentPresenterBindings = { BindingEnum.Content, BindingEnum.Visibility };
		private static readonly BindingEnum[] ItemsControlBindings = { BindingEnum.ItemsSource, BindingEnum.Visibility };

		public static BindingEnum[] GetBindings(TypeEnum type)
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
