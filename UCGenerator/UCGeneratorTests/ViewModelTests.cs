using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApprovalTests;
using ApprovalTests.Reporters;
using ApprovalTests.Wpf;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using UCGenerator;
using UCGenerator.Services;

namespace UCGeneratorTests
{
	[TestClass]
	[UseReporter(typeof(DiffReporter), typeof(ClipboardReporter))]
	public class ViewModelTests
	{
		private Mock<IDataService> dataService;
		private Mock<IGeneratorService> generatorService;

		[TestInitialize]
		public void Update()
		{
			this.dataService = new Mock<IDataService>();
			this.generatorService = new Mock<IGeneratorService>();
		}

		[TestMethod]
		public void VerifyDefaultGui()
		{
			var window = new MainWindow();
			WpfApprovals.Verify(window);
		}

		[TestMethod]
		public void WhenClickToPlusButton_ThenControlShouldBeAdded()
		{
			var viewModel = new MainViewModel(this.dataService.Object, this.generatorService.Object);
			viewModel.AddCommand.Execute(null);
			Approvals.Verify("Controls count before: 0\nControls count now:" + viewModel.Controls.Count);
		}

		[TestMethod]
		public void WhenClickToMinusButton_ThenControlShouldBeRemoved()
		{
			var viewModel = new MainViewModel(this.dataService.Object, this.generatorService.Object);
			viewModel.AddCommand.Execute(null);
			viewModel.SelectedControl = viewModel.Controls.First();
			viewModel.RemoveCommand.Execute(null);
			Approvals.Verify("Controls count before: 1\nControls count now:" + viewModel.Controls.Count);
		}

		[TestMethod]
		public void WhenNoControlSelected_ThenRemoveButtonShouldBeDisabled()
		{
			var viewModel = new MainViewModel(this.dataService.Object, this.generatorService.Object);
			Approvals.Verify("Is Remove Button Enabled: " + viewModel.RemoveCommand.CanExecute(null));
		}

		[TestMethod]
		public void WhenCreateMainViewModel_GetDefaultProjectPath()
		{
			var viewModel = new MainViewModel(this.dataService.Object, this.generatorService.Object);
			this.dataService.Verify(x => x.GetDefaultProjectPath(), Times.Once, "Project path is not set");
		}

		[TestMethod]
		public void WhenNoControls_GenerateButtonShouldBeDisabled()
		{
			var viewModel = new MainViewModel(this.dataService.Object, this.generatorService.Object);
			Approvals.Verify("Button enabled = " + viewModel.GenerateCommand.CanExecute(null));
		}

		[TestMethod]
		public void WhenNotAllControlsHaveGotName_GenerateButtonShouldBeDisabled()
		{
			var viewModel = new MainViewModel(this.dataService.Object, this.generatorService.Object);
			viewModel.AddCommand.Execute(null);
			viewModel.AddCommand.Execute(null);
			viewModel.AddCommand.Execute(null);
			viewModel.Controls[0].PropertyName = "control1";
			viewModel.Controls[1].PropertyName = "control2";
			Approvals.Verify("Button enabled = " + viewModel.GenerateCommand.CanExecute(null));
		}

		[TestMethod]
		public void WhenNamespaceIsEmpty_GenerateButtonShouldBeDisabled()
		{
			var viewModel = new MainViewModel(this.dataService.Object, this.generatorService.Object);
			viewModel.AddCommand.Execute(null);
			viewModel.Controls[0].PropertyName = "control1";
			viewModel.Name = "bla.xaml";
			Approvals.Verify("Button enabled = " + viewModel.GenerateCommand.CanExecute(null));
		}

		[TestMethod]
		public void WhenNamesIsEmpty_GenerateButtonShouldBeDisabled()
		{
			var viewModel = new MainViewModel(this.dataService.Object, this.generatorService.Object);
			viewModel.AddCommand.Execute(null);
			viewModel.Controls[0].PropertyName = "control1";
			viewModel.NameSpace = "bla";
			Approvals.Verify("Button enabled = " + viewModel.GenerateCommand.CanExecute(null));
		}

		[TestMethod]
		public void WhenAllControlsHaveGotNameAndNamespaceIsSet_GenerateButtonShouldBeEnabled()
		{
			var viewModel = new MainViewModel(this.dataService.Object, this.generatorService.Object);
			viewModel.AddCommand.Execute(null);
			viewModel.AddCommand.Execute(null);
			viewModel.AddCommand.Execute(null);
			viewModel.Controls[0].PropertyName = "control1";
			viewModel.Controls[1].PropertyName = "control2";
			viewModel.Controls[2].PropertyName = "control3";
			viewModel.NameSpace = "bla";
			viewModel.Name = "bla.xaml";
			Approvals.Verify("Button enabled = " + viewModel.GenerateCommand.CanExecute(null));
		}

		[TestMethod]
		public void WhenClickGenerate_ThenGeneratorServiceShouldGetCalled()
		{
			var viewModel = new MainViewModel(this.dataService.Object, this.generatorService.Object);
			viewModel.GenerateCommand.Execute(null);
			this.generatorService.Verify(x => x.Generate(viewModel.Controls, viewModel.ProjectsFolder, viewModel.NameSpace, viewModel.Name), Times.Once, "Generator was not Called");
		}
	}
}
