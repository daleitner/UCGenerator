using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Base;
using UCGenerator.Services;

namespace UCGenerator
{
	public class MainViewModel : ViewModelBase
	{
		private string projectsFolder;
		private string nameSpace;
		private string name;
		
		private ObservableCollection<WPFControl> controls;
		private WPFControl selectedControl;
		private RelayCommand generateCommand;
		private RelayCommand addCommand;
		private RelayCommand removeCommand;
		public MainViewModel(IDataService dataService)
		{
			this.projectsFolder = dataService.GetDefaultProjectPath();
			this.controls = new ObservableCollection<WPFControl>();
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

		public List<BindingModel> Bindings
		{
			get { return this.SelectedControl?.Bindings; }
			set
			{
				if(this.SelectedControl != null)
					this.SelectedControl.Bindings = value;
				OnPropertyChanged(nameof(this.Bindings));
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

		public ICommand GenerateCommand
		{
			get { return this.generateCommand ?? (this.generateCommand = new RelayCommand(param => Generate())); }
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

		private void Generate()
		{
		}
	}
}
