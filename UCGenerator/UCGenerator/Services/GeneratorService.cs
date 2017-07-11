using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UCGenerator.Statics;

namespace UCGenerator.Services
{
	public class GeneratorService : IGeneratorService
	{
		private readonly IDataService dataService;

		public GeneratorService(IDataService dataService)
		{
			this.dataService = dataService;
		}

		public void Generate(IList<WPFControl> controls, string projectsFolder, string nameSpace, string name)
		{
			throw new NotImplementedException();
		}

		public string GenerateAbsolutePath(string projectFolder, string nameSpace)
		{
			if (projectFolder.Last() != '\\')
				projectFolder += "\\";
			var path = projectFolder + string.Join("\\", nameSpace.Split('.')) + "\\";
			return path;
		}

		public string GenerateUserControl(List<WPFControl> controls, string nameSpace, string name)
		{
			var content = "<UserControl x:Class=\"" + nameSpace + "." + name + "UserControl\"\r\n";
			content += "             xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\"\r\n";
			content += "             xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\"\r\n";
			content += "             xmlns:mc=\"http://schemas.openxmlformats.org/markup-compatibility/2006\"\r\n";
			content += "             xmlns:d=\"http://schemas.microsoft.com/expression/blend/2008\"\r\n";
			content += "             xmlns:local=\"clr-namespace:" + nameSpace + "\"\r\n";
			content += "             mc: Ignorable=\"d\"\r\n";
			content += "             d:DesignHeight = \"300\" d:DesignWidth=\"300\">\r\n";
			content += "\t<Grid>\r\n";

			controls.ForEach(x => content += GenerateControl(x));

			content += "\t</Grid>\r\n";
			content += "</UserControl>";
			return content;
		}

		private string GenerateControl(WPFControl control)
		{
			var shortEnd = true;
			var str = "\t\t<" + control.Type + " ";
			var shortName = "";
			var horizontalAlignment = "Left";
			var verticalAlignment = "Top";
			var margin = "2,2,0,0";
			var attributes = control.Bindings.Where(x => x.IsBound
			&& x.PropertyName != BindingEnum.KeyBinding
			&& x.PropertyName != BindingEnum.DoubleClick
			&& x.PropertyName != BindingEnum.PropertyChangedTrigger)
				.Select(x => x.PropertyName).ToList();

			switch (control.Type)
			{
				case TypeEnum.TextBox:
					shortName = "tb";
					break;
				case TypeEnum.Label:
					shortName = "l";
					break;
				case TypeEnum.ComboBox:
					shortName = "cb";
					break;
				case TypeEnum.ListBox:
					shortName = "lb";
					horizontalAlignment = "Stretch";
					verticalAlignment = "Stretch";
					break;
				case TypeEnum.Button:
					shortName = "btn";
					break;
				case TypeEnum.ContentPresenter:
					shortName = "cp";
					horizontalAlignment = "Stretch";
					verticalAlignment = "Stretch";
					break;
				case TypeEnum.ItemsControl:
					shortName = "ic";
					horizontalAlignment = "Stretch";
					verticalAlignment = "Stretch";
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}


			str += "Name=\"" + shortName + control.PropertyName + "\" ";
			str += "HorizontalAlignment=\"" + horizontalAlignment + "\" ";
			str += "VerticalAlignment=\"" + verticalAlignment + "\" ";
			str += "Margin=\"" + margin + "\" ";
			if (horizontalAlignment != "Stretch")
				str += "Width=\"100\" ";

			attributes.ForEach(attr => str += attr + "=\"{Binding Path=" + GetNamePrefix(attr) + 
			(attr == BindingEnum.SelectedItem ? control.PropertyName.Substring(0, control.PropertyName.Length-1) : control.PropertyName) + GetNamePostfix(attr) + 
			(attr == BindingEnum.Text && control.Bindings.Any(x => x.IsBound && x.PropertyName == BindingEnum.PropertyChangedTrigger) ? ", UpdateSourceTrigger=PropertyChanged" : "") + "}\" ");

			if (control.Bindings.Any(x => x.IsBound && x.PropertyName == BindingEnum.KeyBinding))
			{
				shortEnd = false;
				str += ">\r\n";
				str += "\t\t\t<" + control.Type + ".InputBindings>\r\n";
				str += "\t\t\t\t<KeyBinding Key=\"Enter\", Command=\"{Binding Path=" + control.PropertyName + "EnterCommand}\" />\r\n";
				if (control.Bindings.Any(x => x.IsBound && x.PropertyName == BindingEnum.DoubleClick))
					str += "\t\t\t\t<MouseBinding MouseAction=\"LeftDoubleClick\", Command=\"{Binding Path=" + control.PropertyName + "EnterCommand}\" />\r\n";
				str += "\t\t\t</" + control.Type + ".InputBindings>\r\n";
			}
			else if (control.Bindings.Any(x => x.IsBound && x.PropertyName == BindingEnum.DoubleClick))
			{
				shortEnd = false;
				str += ">\r\n";
				str += "\t\t\t<" + control.Type + ".InputBindings>\r\n";
				str += "\t\t\t\t<MouseBinding MouseAction=\"LeftDoubleClick\", Command=\"{Binding Path=" + control.PropertyName + "DoubleClickCommand}\" />\r\n";
				str += "\t\t\t</" + control.Type + ".InputBindings>\r\n";
			}
			str += shortEnd ? " />" : "\t\t</" + control.Type + ">";
			str += "\r\n";
			return str;
		}


		private string GetNamePrefix(BindingEnum attr)
		{
			if (attr == BindingEnum.IsEnabled)
				return "Is";
			if (attr == BindingEnum.SelectedItem)
				return "Selected";
			return "";
		}

		private string GetNamePostfix(BindingEnum attr)
		{
			switch(attr)
			{
				
				case BindingEnum.IsEnabled:
					return "Enabled";
				case BindingEnum.Visibility:
					return "Visibility";
				case BindingEnum.Command:
					return "Command";
				case BindingEnum.KeyBinding:
				case BindingEnum.Content:
				case BindingEnum.ItemsSource:
				case BindingEnum.SelectedItem:
				case BindingEnum.Text:
				case BindingEnum.PropertyChangedTrigger:
					return "";
				default:
					throw new ArgumentOutOfRangeException(nameof(attr), attr, null);
			}
		}

		public string GenerateBackgroundFile(string nameSpace, string name)
		{
			var file = "using System.Windows.Controls\r\n";
			file += "namespace " + nameSpace + "\r\n";
			file += "{\r\n";
			file += "\t/// <summary>\r\n";
			file += "\t/// Interactionlogic for " + name + "UserControl.xaml\r\n";
			file += "\t/// </summary>\r\n";
			file += "\tpublic partial class " + name + "UserControl\r\n";
			file += "\t{\r\n";
			file += "\t\tpublic " + name + "UserControl()\r\n";
			file += "\t\t{\r\n";
			file += "\t\t\tInitializeComponent();\r\n";
			file += "\t\t}\r\n";
			file += "\t}\r\n";
			file += "}";
			return file;
		}

		public string GenerateViewModel(List<WPFControl> controls, string nameSpace, string name)
		{
			var file = "";
			var libraries = new List<string>
			{
				"System",
				"System.Collections.Generic",
				"System.Collections.ObjectModel",
				"System.Linq",
				"System.Text",
				"System.Threading.Tasks",
				"System.Windows.Input",
				"Base"
			};
			libraries.ForEach(x => file += "using " + x + ";\r\n\r\n");

			file += "namespace " + nameSpace + "\r\n";
			file += "{\r\n";
			file += "\tpublic class " + name + "ViewModel : ViewModelBase\r\n";
			file += "\t{\r\n";

			file += "\t}\r\n";
			file += "}";
		return file;
		}
	}
}
