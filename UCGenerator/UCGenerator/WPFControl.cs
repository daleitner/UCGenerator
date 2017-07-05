using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UCGenerator.Statics;

namespace UCGenerator
{
	public class WPFControl
	{
		public WPFControl()
		{
			this.Bindings = new List<BindingModel>();
		}
		public string PropertyName { get; set; }
		public TypeEnum Type { get; set; }
		public List<BindingModel> Bindings { get; set; }
	}
}
