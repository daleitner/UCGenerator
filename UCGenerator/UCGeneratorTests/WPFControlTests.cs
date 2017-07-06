using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApprovalTests;
using ApprovalTests.Reporters;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UCGenerator;
using UCGenerator.Statics;

namespace UCGeneratorTests
{
	[TestClass]
	[UseReporter(typeof(DiffReporter))]
	public class WPFControlTests
	{
		[TestMethod]
		public void WhenCreateWPFControl_VerifyBindings()
		{
			var control = new WPFControl();
			Approvals.VerifyAll(control.Bindings.Select(x => x.PropertyName), "Binding");
		}

		[TestMethod]
		public void WhenChangeType_UpdateBindings()
		{
			var control = new WPFControl();
			control.Type = TypeEnum.Button;
			Approvals.VerifyAll(control.Bindings.Select(x => x.PropertyName), "Binding");
		}
	}
}
