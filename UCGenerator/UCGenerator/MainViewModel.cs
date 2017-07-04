using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Base;

namespace UCGenerator
{
	public class MainViewModel : ViewModelBase
	{
		private string projectsFolder;
		private string nameSpace;
		private string name;

		private List<string> types;
		private ObservableCollection<WPFControl> controls;
		private WPFControl selectedControl;
		private RelayCommand createCommand;
		private RelayCommand addCommand;
		private RelayCommand removeCommand;
		public MainViewModel()
		{
			this.types = new List<string> {"Textbox", "Label", "Combobox"};
			this.controls = new ObservableCollection<WPFControl>();
		}
		public List<string> Types
		{
			get
			{
				return this.types;
			}
			set
			{
				this.types = value;
				OnPropertyChanged(nameof(this.Types));
			}
		}

		public string NameSpace
		{
			get { return this.nameSpace; }
			set
			{
				this.nameSpace = value;
				OnPropertyChanged(nameof(this.NameSpace));
			}
		}

		public string ProjectsFolder
		{
			get { return this.projectsFolder; }
			set
			{
				this.projectsFolder = value;
				OnPropertyChanged(nameof(this.ProjectsFolder));
			}
		}

		public string Name
		{
			get { return this.name; }
			set
			{
				this.name = value;
				OnPropertyChanged(nameof(this.Name));
			}
		}

		public WPFControl SelectedControl
		{
			get { return this.selectedControl; }
			set
			{
				this.selectedControl = value;
				OnPropertyChanged(nameof(this.SelectedControl));
			}
		}

		public ObservableCollection<WPFControl> Controls
		{
			get { return this.controls; }
			set
			{
				this.controls = value;
				OnPropertyChanged(nameof(this.Controls));
			}
		}

		public ICommand CreateCommand
		{
			get { return this.createCommand ?? (this.createCommand = new RelayCommand(param => Create())); }
		}

		public ICommand AddCommand
		{
			get { return this.addCommand ?? (this.addCommand = new RelayCommand(param => Add())); }
		}

		public ICommand RemoveCommand
		{
			get { return this.removeCommand ?? (this.removeCommand = new RelayCommand(param => Remove(), param => this.SelectedControl != null)); }
		}

		private void Remove()
		{
			this.Controls.Remove(this.SelectedControl);
		}

		private void Add()
		{
			this.Controls.Add(new WPFControl());
		}

		private void Create()
		{
		}
	}
}
