using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Media3D;
using System.Windows.Threading;
using ECS.UI.Model;
using HelixToolkit.Wpf;
using Prism.Commands;
using INNO6.Core;
using System.Numerics;

namespace ECS.UI.ViewModel
{
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Reviewed. Suppression is OK here.")]
    public class Visual3DMainViewModel : Observable
    {
        private ICommand _ConvertToSpherical;

        public ICommand ConvertToSpherical {
            get
            {
                if (_ConvertToSpherical == null)
                {
                    _ConvertToSpherical = new DelegateCommand(ExecuteConvertToSpherical);
                }

                return _ConvertToSpherical;
            }
        }

        double _Radius;
        double _Theta;
        double _Phi;

        public double Radius { get { return _Radius; } set { _Radius = value; RaisePropertyChanged("Radius"); } }
        public double Theta { get { return _Theta; } set { _Theta = value; RaisePropertyChanged("Theta"); } }
        public double Phi { get { return _Phi; } set { _Phi = value; RaisePropertyChanged("Phi"); } }

        private const string OpenFileFilter = "3D model files (*.3ds;*.obj;*.lwo;*.stl;*.ply;)|*.3ds;*.obj;*.objz;*.lwo;*.stl;*.ply;";

        private const string TitleFormatString = "3D model viewer - {0}";

        private readonly IFileDialogService fileDialogService;

        private readonly IHelixViewport3D viewport;

        private readonly Dispatcher dispatcher;

        private string currentModelPath;

        private string applicationTitle;

        private double expansion;

        private Model3D currentModel;

        private void ExecuteConvertToSpherical()
        {
            Vector3 vector = new Vector3()
            {
                X = Convert.ToSingle(this.viewport.Camera.Position.X),
                Y = Convert.ToSingle(this.viewport.Camera.Position.Y),
                Z = Convert.ToSingle(this.viewport.Camera.Position.Z)
            };


            SphericalCoordinateCoverter.ToPolar(vector, out double radius, out double longitude, out double latitude);

            Radius = radius;
            Theta = longitude * (180 / Math.PI);
            Phi = latitude * (180 / Math.PI);
        }

        public Visual3DMainViewModel(IFileDialogService fds, HelixViewport3D viewport)
        {
            if (viewport == null)
            {
                throw new ArgumentNullException("viewport");
            }

            this.dispatcher = Dispatcher.CurrentDispatcher;
            this.Expansion = 1;
            this.fileDialogService = fds;
            this.viewport = viewport;
            this.FileOpenCommand = new DelegateCommand(this.FileOpen);
            this.FileExportCommand = new DelegateCommand(this.FileExport);
            this.FileExitCommand = new DelegateCommand(FileExit);
            this.ViewZoomExtentsCommand = new DelegateCommand(this.ViewZoomExtents);
            this.EditCopyXamlCommand = new DelegateCommand(this.CopyXaml);
            this.ApplicationTitle = "3D Model viewer";
            this.Elements = new List<Visual3DViewModel>();
            foreach (var c in viewport.Children)
            {
                this.Elements.Add(new Visual3DViewModel(c));
            }
        }

        public string CurrentModelPath
        {
            get
            {
                return this.currentModelPath;
            }

            set
            {
                this.currentModelPath = value;
                this.RaisePropertyChanged("CurrentModelPath");
            }
        }

        public string ApplicationTitle
        {
            get
            {
                return this.applicationTitle;
            }

            set
            {
                this.applicationTitle = value;
                this.RaisePropertyChanged("ApplicationTitle");
            }
        }

        public List<Visual3DViewModel> Elements { get; set; }

        public double Expansion
        {
            get
            {
                return this.expansion;
            }

            set
            {
                if (!this.expansion.Equals(value))
                {
                    this.expansion = value;
                    this.RaisePropertyChanged("Expansion");
                }
            }
        }

        public Model3D CurrentModel
        {
            get
            {
                return this.currentModel;
            }

            set
            {
                this.currentModel = value;
                this.RaisePropertyChanged("CurrentModel");
            }
        }

        public ICommand FileOpenCommand { get; set; }

        public ICommand FileExportCommand { get; set; }

        public ICommand FileExitCommand { get; set; }

        public ICommand HelpAboutCommand { get; set; }

        public ICommand ViewZoomExtentsCommand { get; set; }

        public ICommand EditCopyXamlCommand { get; set; }

        private static void FileExit()
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void FileExport()
        {
            var path = this.fileDialogService.SaveFileDialog(null, null, Exporters.Filter, ".png");
            if (path == null)
            {
                return;
            }

            this.viewport.Export(path);
        }

        private void CopyXaml()
        {
            var rd = XamlExporter.WrapInResourceDictionary(this.CurrentModel);
            Clipboard.SetText(XamlHelper.GetXaml(rd));
        }

        private void ViewZoomExtents()
        {
            this.viewport.ZoomExtents(500);
        }

        private async void FileOpen()
        {
            this.CurrentModelPath = this.fileDialogService.OpenFileDialog("models", null, OpenFileFilter, ".3ds");
            this.CurrentModel = await this.LoadAsync(this.CurrentModelPath, true);
            this.ApplicationTitle = string.Format(TitleFormatString, this.CurrentModelPath);
            this.viewport.ZoomExtents(0);
        }

        private async Task<Model3DGroup> LoadAsync(string model3DPath, bool freeze)
        {
            return await Task.Factory.StartNew(() =>
            {
                var mi = new ModelImporter();

                if (freeze)
                {
                    // Alt 1. - freeze the model 
                    return mi.Load(model3DPath, null, true);
                }

                // Alt. 2 - create the model on the UI dispatcher
                return mi.Load(model3DPath, this.dispatcher);
            });
        }
    }
}
