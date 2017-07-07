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
		private IGeneratorService generatorService;

		public MainViewModel(IDataService dataService, IGeneratorService generatorService)
		{
			this.generatorService = generatorService;
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
				this.Bindings = new List<BindingModel>(this.selectedControl.Bindings);
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
			get { return this.generateCommand ?? (this.generateCommand = new RelayCommand(param => Generate(), param => CanGenerate())); }
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
			var control = new WPFControl();
			control.TypeChanged += Control_TypeChanged;
			this.Controls.Add(control);
		}

		private void Control_TypeChanged(object sender, EventArgs e)
		{
			if (this.SelectedControl == null)
				return;
			this.Bindings = new List<BindingModel>(this.SelectedControl.Bindings);
		}

		private bool CanGenerate()
		{
			return this.Controls.Count > 0 && !this.Controls.Any(x => string.IsNullOrEmpty(x.PropertyName))
				&& !string.IsNullOrEmpty(this.NameSpace) && !string.IsNullOrEmpty(this.Name);
		}

		private void Generate()
		{
			this.generatorService.Generate(this.Controls, this.ProjectsFolder, this.NameSpace, this.Name);
		}
	}
}
