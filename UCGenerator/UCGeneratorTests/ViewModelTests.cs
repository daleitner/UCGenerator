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

		[TestInitialize]
		public void Update()
		{
			this.dataService = new Mock<IDataService>();
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
			var viewModel = new MainViewModel(this.dataService.Object);
			viewModel.AddCommand.Execute(null);
			Approvals.Verify("Controls count before: 0\nControls count now:" + viewModel.Controls.Count);
		}

		[TestMethod]
		public void WhenClickToMinusButton_ThenControlShouldBeRemoved()
		{
			var viewModel = new MainViewModel(this.dataService.Object);
			viewModel.AddCommand.Execute(null);
			viewModel.SelectedControl = viewModel.Controls.First();
			viewModel.RemoveCommand.Execute(null);
			Approvals.Verify("Controls count before: 1\nControls count now:" + viewModel.Controls.Count);
		}

		[TestMethod]
		public void WhenNoControlSelected_ThenRemoveButtonShouldBeDisabled()
		{
			var viewModel = new MainViewModel(this.dataService.Object);
			Approvals.Verify("Is Remove Button Enabled: " + viewModel.RemoveCommand.CanExecute(null));
		}

		[TestMethod]
		public void WhenCreateMainViewModel_GetDefaultProjectPath()
		{
			var viewModel = new MainViewModel(this.dataService.Object);
			this.dataService.Verify(x => x.GetDefaultProjectPath(), Times.Once, "Project path is not set");
		}
	}
}
