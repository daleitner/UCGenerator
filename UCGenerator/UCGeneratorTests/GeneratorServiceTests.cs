﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApprovalTests;
using ApprovalTests.Reporters;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using UCGenerator;
using UCGenerator.Services;
using UCGenerator.Statics;

namespace UCGeneratorTests
{
	[TestClass]
	[UseReporter(typeof(DiffReporter))]
	public class GeneratorServiceTests
	{
		private Mock<IDataService> dataService;

		[TestInitialize]
		public void Setup()
		{
			this.dataService = new Mock<IDataService>();
		}

		[TestMethod]
		public void VerifyGenerateMethod()
		{
			var service = new GeneratorService(this.dataService.Object);
			var projectFolder = "C:\\Projects\\TestProject";
			var nameSpace = "TestProject";
			var name = "Person";
			var controls = GenerateControls();
			service.Generate(controls, projectFolder, nameSpace, name);
			this.dataService.Verify(x => x.CreateFile(service.GenerateUserControl(controls, nameSpace, name), 
				service.GenerateAbsolutePath(projectFolder, nameSpace) + name + "UserControl.xaml"), Times.Once, "Usercontrol was not created");
		}

		[TestMethod]
		public void VerifyGenerateAbsolutePath()
		{
			var service = new GeneratorService(this.dataService.Object);
			var projectFolder = "C:\\Projects\\TestProject";
			var nameSpace = "TestProject";
			var path = service.GenerateAbsolutePath(projectFolder, nameSpace);
			Approvals.Verify("Path= " + path);
		}

		[TestMethod]
		public void VerifyGenerateAbsolutePathWithSubNameSpace()
		{
			var service = new GeneratorService(this.dataService.Object);
			var projectFolder = "C:\\Projects\\TestProject\\";
			var nameSpace = "TestProject.TestAssembly";
			var path = service.GenerateAbsolutePath(projectFolder, nameSpace);
			Approvals.Verify("Path= " + path);
		}

		[TestMethod]
		public void VerifyGenerateUserControl()
		{
			var service = new GeneratorService(this.dataService.Object);
			var nameSpace = "TestProject";
			var name = "Person";
			var file = service.GenerateUserControl(GenerateControls(), nameSpace, name);
			Approvals.Verify(file);
		}

		private List<WPFControl> GenerateControls()
		{
			var viewModel = new MainViewModel(this.dataService.Object, new GeneratorService(this.dataService.Object));
			viewModel.AddCommand.Execute(null);
			viewModel.Controls.Last().PropertyName = "Name";
			viewModel.Controls.Last().Type = TypeEnum.TextBox;
			viewModel.Controls.Last().Bindings.ForEach(x => x.IsBound = true);
			viewModel.AddCommand.Execute(null);
			viewModel.Controls.Last().PropertyName = "Path";
			viewModel.Controls.Last().Type = TypeEnum.Label;
			viewModel.Controls.Last().Bindings.ForEach(x => x.IsBound = true);
			viewModel.AddCommand.Execute(null);
			viewModel.Controls.Last().PropertyName = "Players";
			viewModel.Controls.Last().Type = TypeEnum.ListBox;
			viewModel.Controls.Last().Bindings.ForEach(x => x.IsBound = true);
			viewModel.AddCommand.Execute(null);
			viewModel.Controls.Last().PropertyName = "Start";
			viewModel.Controls.Last().Type = TypeEnum.Button;
			viewModel.Controls.Last().Bindings.ForEach(x => x.IsBound = true);
			viewModel.AddCommand.Execute(null);
			viewModel.Controls.Last().PropertyName = "Colors";
			viewModel.Controls.Last().Type = TypeEnum.ComboBox;
			viewModel.Controls.Last().Bindings.ForEach(x => x.IsBound = true);
			viewModel.AddCommand.Execute(null);
			viewModel.Controls.Last().PropertyName = "Image";
			viewModel.Controls.Last().Type = TypeEnum.ContentPresenter;
			viewModel.Controls.Last().Bindings.ForEach(x => x.IsBound = true);
			viewModel.AddCommand.Execute(null);
			viewModel.Controls.Last().PropertyName = "Players";
			viewModel.Controls.Last().Type = TypeEnum.ItemsControl;
			viewModel.Controls.Last().Bindings.ForEach(x => x.IsBound = true);
			return viewModel.Controls.ToList();
		}
	}
}
