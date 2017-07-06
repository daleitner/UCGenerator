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
		private TypeEnum type;
		public event EventHandler TypeChanged = null;
		public WPFControl()
		{
			UpdateBindings();
		}
		public string PropertyName { get; set; }

		public TypeEnum Type
		{
			get { return this.type; }
			set
			{
				this.type = value;
				UpdateBindings();
				TypeChanged?.Invoke(null, null);
			}
		}

		public List<BindingModel> Bindings { get; set; }

		private void UpdateBindings()
		{
			this.Bindings = Components.GetBindings(this.Type).Select(x => new BindingModel { PropertyName = x }).ToList();
		}
	}


}
