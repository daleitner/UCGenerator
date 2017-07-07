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
			
			 
			str += shortEnd ? " />" : "</" + control.Type + ">";
			str += "\r\n";
			return str;
		}
	}
}
