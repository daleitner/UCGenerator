using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UCGenerator.Services
{
	public interface IGeneratorService
	{
		void Generate(IList<WPFControl> controls, string projectsFolder, string nameSpace, string name);
	}
}
