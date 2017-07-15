using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileIO.FileWorker;

namespace UCGenerator.Services
{
	public class DataService : IDataService
	{
		private string path = "ProjectPath.txt";
		public string GetDefaultProjectPath()
		{
			FileWorker.GetInstance(this.path, false);
			return FileWorker.ReadFile(this.path);
		}

		public void CreateFile(string content, string path)
		{
			var worker = FileWorker.GetInstance(path, true);
			worker.WriteLine(content);
		}
	}
}
