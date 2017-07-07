using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UCGenerator.Services
{
	public interface IDataService
	{
		string GetDefaultProjectPath();
		void CreateFile(string content, string path);
	}
}
