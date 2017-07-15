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
			var cList = controls.ToList();
			var path = GenerateAbsolutePath(projectsFolder, nameSpace);
			this.dataService.CreateFile(GenerateUserControl(cList, nameSpace, name), path + name + "UserControl.xaml");
			this.dataService.CreateFile(GenerateBackgroundFile(nameSpace, name), path + name + "UserControl.xaml.cs");
			this.dataService.CreateFile(GenerateViewModel(cList, nameSpace, name), path + name + "ViewModel.cs");
			this.dataService.CreateFile(GenerateControllerInterface(nameSpace, name), path + "I" + name + "Controller.cs");
			this.dataService.CreateFile(GenerateController(nameSpace, name), path + name + "Controller.cs");
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

			attributes.ForEach(attr => str += attr + "=\"{Binding Path=" + GetPropertyName(control, attr)+ 
			(attr == BindingEnum.Text && control.Bindings.Any(x => x.IsBound && x.PropertyName == BindingEnum.PropertyChangedTrigger) ? ", UpdateSourceTrigger=PropertyChanged" : "") + "}\" ");

			if (control.Bindings.Any(x => x.IsBound && (x.PropertyName == BindingEnum.KeyBinding || x.PropertyName == BindingEnum.KeyBinding)))
			{
				shortEnd = false;
				str += ">\r\n";
				str += "\t\t\t<" + control.Type + ".InputBindings>\r\n";
				if (control.Bindings.Any(x => x.IsBound && x.PropertyName == BindingEnum.KeyBinding))
					str += "\t\t\t\t<KeyBinding Key=\"Enter\", Command=\"{Binding Path=" + GetPropertyName(control, BindingEnum.KeyBinding) + "}\" />\r\n";
				else
					str += "\t\t\t\t<MouseBinding MouseAction=\"LeftDoubleClick\", Command=\"{Binding Path=" + GetPropertyName(control, BindingEnum.DoubleClick) + "}\" />\r\n";
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
				case BindingEnum.DoubleClick:
					return "EnterCommand";
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
			if(controls.Any(x => x.Bindings.Any(y => y.IsBound && y.PropertyName == BindingEnum.Visibility)))
				libraries.Add("System.Windows");
			libraries.ForEach(x => file += "using " + x + ";\r\n");
			file += "\r\n";
			file += "namespace " + nameSpace + "\r\n";
			file += "{\r\n";
			file += "\tpublic class " + name + "ViewModel : ViewModelBase\r\n";
			file += "\t{\r\n";
			controls.ForEach(control => control.Bindings.Where(binding => binding.IsBound).ToList().ForEach(binding => file += "\t\t" + GenerateMember(control, binding.PropertyName) + "\r\n"));
			file += "\t\tpublic " + name + "()\r\n";
			file += "\t\t{\r\n";
			controls.ForEach(x => x.Bindings.Where(y => y.IsBound && y.PropertyName == BindingEnum.ItemsSource).ToList().ForEach(y => file += "\t\t\tthis." + 
			GetMemberName(x,y.PropertyName) + " = new ObservableCollection<object>();\r\n"));
			file += "\t\t}\r\n\r\n";
			controls.ForEach(control => control.Bindings.Where(x => x.IsBound).ToList().ForEach(binding => file += GenerateProperty(control, binding.PropertyName)));
			controls.ForEach(control => control.Bindings.Where(x => x.IsBound && (x.PropertyName == BindingEnum.Command ||
			x.PropertyName == BindingEnum.DoubleClick || x.PropertyName == BindingEnum.KeyBinding
			)).ToList().ForEach(binding => file += GenerateMethod(control, binding.PropertyName)));
			file += "\t}\r\n";
			file += "}";
		return file;
		}

		public string GenerateControllerInterface(string nameSpace, string name)
		{
			var file = "namespace " + nameSpace + "\r\n";
			file += "{\r\n";
			file += "\tpublic interface I" + name + "Controller\r\n";
			file += "\t{\r\n";
			file += "\t}\r\n";
			file += "}\r\n";
			return file;
		}

		public string GenerateController(string nameSpace, string name)
		{
			var libraries = new List<string>
			{
				"System",
				"System.Collections.Generic",
				"System.Linq",
				"System.Text",
				"System.Threading.Tasks",
			};

			var file = "";
			libraries.ForEach(x => file += "using " + x + ";\r\n");
			file += "\r\nnamespace " + nameSpace + "\r\n";
			file += "{\r\n";
			file += "\tpublic class " + name + "Controller : I" + name + "Controller\r\n";
			file += "\t{\r\n";
			file += "\t}\r\n";
			file += "}\r\n";
			return file;
		}

		private string GenerateMethod(WPFControl control, BindingEnum propertyName)
		{
			var file = "\t\t\tprivate void " + control.PropertyName + (propertyName == BindingEnum.DoubleClick || propertyName == BindingEnum.KeyBinding ? "Enter" : "") + "()\r\n";
			file += "\t\t\t{\r\n";
			file += "\t\t\t}\r\n\r\n";
			return file;
		}

		private string GenerateMember(WPFControl control, BindingEnum binding)
		{
			if (binding == BindingEnum.PropertyChangedTrigger)
				return "";
			var member = "private ";
			member += GetMemberType(control, binding) + " ";
			member += GetMemberName(control, binding);
			member += ";";
			return member;
		}

		private string GenerateProperty(WPFControl control, BindingEnum binding)
		{
			var file = "";
			var type = GetPropertyType(control, binding);
			var memberName = GetMemberName(control, binding);
			file += "\t\tpublic " + type + " " +
						GetPropertyName(control, binding) + "\r\n";
			file += "\t\t{\r\n";
			file += "\t\t\tget\r\n";
			file += "\t\t\t{\r\n";
			if (type == "ICommand")
			{
				file += "\t\t\t\treturn this." + memberName +" ?? (this." + memberName + " = new RelayCommand(param => " + control.PropertyName + "()));\r\n";
			}
			else
			{
				file += "\t\t\t\treturn this." + memberName + "\r\n";
			}
			file += "\t\t\t}\r\n";
			if (type != "ICommand")
			{
				file += "\t\t\tset\r\n";
				file += "\t\t\t{\r\n";
				file += "\t\t\t\tthis." + memberName + " = value;\r\n";
				file += "\t\t\t\tOnPropertyChanged(nameof(this." + GetPropertyName(control, binding) + "));\r\n";
				file += "\t\t\t}\r\n";
			}
			file += "\t\t}\r\n\r\n";
			return file;
		}

		private string GetMemberName(WPFControl control, BindingEnum binding)
		{
			var membername = GetNamePrefix(binding) +
							 (binding == BindingEnum.SelectedItem
								 ? control.PropertyName.Substring(0, control.PropertyName.Length - 1)
								 : control.PropertyName) + GetNamePostfix(binding);
			return char.ToLower(membername[0]) + membername.Substring(1);
		}

		private string GetPropertyName(WPFControl control, BindingEnum binding)
		{
			var membername = GetNamePrefix(binding) +
			                 (binding == BindingEnum.SelectedItem
				                 ? control.PropertyName.Substring(0, control.PropertyName.Length - 1)
				                 : control.PropertyName) + GetNamePostfix(binding);
			return char.ToUpper(membername[0]) + membername.Substring(1);
		}

		private string GetPropertyType(WPFControl control, BindingEnum binding)
		{
			switch (binding)
			{
				case BindingEnum.Text:
					return "string";
				case BindingEnum.IsEnabled:
					return "bool";
				case BindingEnum.Visibility:
					return "Visibility";
				case BindingEnum.ItemsSource:
					return "List<object>";
				case BindingEnum.SelectedItem:
					return "object";
				case BindingEnum.Command:
				case BindingEnum.KeyBinding:
				case BindingEnum.DoubleClick:
					return "ICommand";
				case BindingEnum.Content:
					return control.Type == TypeEnum.ContentPresenter ? "object" : "string";
			}
			return "";
		}

		private string GetMemberType(WPFControl control, BindingEnum binding)
		{
			switch (binding)
			{
				case BindingEnum.Text:
					return "string";
				case BindingEnum.IsEnabled:
					return "bool";
				case BindingEnum.Visibility:
					return "Visibility";
				case BindingEnum.ItemsSource:
					return "List<object>";
				case BindingEnum.SelectedItem:
					return "object";
				case BindingEnum.Command:
				case BindingEnum.KeyBinding:
				case BindingEnum.DoubleClick:
					return "RelayCommand";
				case BindingEnum.Content:
					return control.Type == TypeEnum.ContentPresenter ? "object" : "string";
			}
			return "";
		}
	}
}
