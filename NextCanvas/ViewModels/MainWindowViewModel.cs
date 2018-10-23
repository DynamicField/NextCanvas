using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using Fluent;
using NextCanvas.Controls.Content;
using NextCanvas.Interactivity;
using NextCanvas.Interactivity.Progress;
using NextCanvas.Models;
using NextCanvas.Models.Content;
using NextCanvas.Serialization;
using NextCanvas.ViewModels.Content;

namespace NextCanvas.ViewModels
{
    public class MainWindowViewModel : ViewModelBase<MainWindowModel>
    {
        private DocumentViewModel document;
        private int selectedToolIndex;

        public MainWindowViewModel()
        {
            Initialize();
        }

        public MainWindowViewModel(MainWindowModel model) : base(model)
        {
            Initialize();
        }

        public DocumentViewModel CurrentDocument
        {
            get => document;
            set
            {
                if (document != null)
                {
                    document.Pages.CollectionChanged -= PagesChanged;
                    document.PropertyChanged -= DocumentPropertyChanged;
                }

                document = value;
                Model.Document = document.Model;
                Subscribe();
                OnPropertyChanged(nameof(CurrentDocument));
                UpdatePageManipulation();
                UpdatePageText();
            }
        }

        public ObservableCollection<Color> FavoriteColors => Model.FavouriteColors;
        public ObservableViewModelCollection<ToolViewModel, Tool> Tools { get; set; }
        public string SavePath { get; set; }
        public string OpenPath { get; set; }
        public string OpenImagePath { get; set; }

        public int SelectedToolIndex
        {
            get => selectedToolIndex;
            set
            {
                if (value >= 0 && value < Tools.Count || Tools.Count == 0)
                {
                    selectedToolIndex = value;
                    OnPropertyChanged(nameof(SelectedToolIndex));
                    OnPropertyChanged(nameof(SelectedTool));
                    ToolViewModel.UpdateCursorIfEraser(SelectedTool);
                }
            }
        }

        // better use this tho
        public ToolViewModel SelectedTool
        {
            get
            {
                var tool = Tools[SelectedToolIndex];
                var color = tool.DrawingAttributes.Color;
                if (!ColorGallery.StandardThemeColors.Contains(color) && !FavoriteColors.Contains(color))
                    FavoriteColors.Add(color);
                return tool;
            }
            set => SelectedToolIndex = Tools.IndexOf(value);
        }

        public DelegateCommand PreviousPageCommand { get; private set; }
        public DelegateCommand NextPageCommand { get; private set; }
        public DelegateCommand NewPageCommand { get; private set; }
        public DelegateCommand DeletePageCommand { get; private set; }
        public DelegateCommand ExtendPageCommand { get; private set; }
        public DelegateCommand SetToolByNameCommand { get; private set; }
        public DelegateCommand SaveCommand { get; private set; }
        public DelegateCommand OpenCommand { get; private set; }
        public DelegateCommand CreateTextBoxCommand { get; private set; }
        public DelegateCommand SwitchToSelectToolCommand { get; private set; }
        public DelegateCommand CreateImageCommand { get; private set; }
        public string PageDisplayText => CurrentDocument.SelectedIndex + 1 + "/" + CurrentDocument.Pages.Count;

        private bool CanDeletePage => CurrentDocument.Pages.Count > 1;

        private DocumentSerializer DocumentSerializer { get; } = new DocumentSerializer();

        private void Subscribe() // To my youtube channel XD
        {
            document.Pages.CollectionChanged += PagesChanged;
            document.PropertyChanged += DocumentPropertyChanged;
        }

        private void DocumentPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            UpdatePageText();
            UpdatePageManipulation();
        }

        private void UpdatePageManipulation()
        {
            PreviousPageCommand.RaiseCanExecuteChanged();
            NextPageCommand.RaiseCanExecuteChanged();
            DeletePageCommand.RaiseCanExecuteChanged();
        }

        private void PagesChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            UpdatePageText();
        }

        private void UpdatePageText()
        {
            OnPropertyChanged(nameof(PageDisplayText));
        }

        private void Initialize()
        {
            document = new DocumentViewModel(Model.Document);
            Tools = new ObservableViewModelCollection<ToolViewModel, Tool>(Model.Tools, t => new ToolViewModel(t));
            Subscribe();
            PreviousPageCommand = new DelegateCommand(o => ChangePage(Direction.Backwards),
                o => CanChangePage(Direction.Backwards));
            NextPageCommand = new DelegateCommand(o => ChangePage(Direction.Forwards),
                o => CanChangePage(Direction.Forwards));
            NewPageCommand = new DelegateCommand(o => CreateNewPage());
            DeletePageCommand = new DelegateCommand(o => DeletePage(CurrentDocument.SelectedIndex), o => CanDeletePage);
            ExtendPageCommand = new DelegateCommand(o => ExtendPage(o.ToString()));
            SetToolByNameCommand =
                new DelegateCommand(o => SetToolByName(o.ToString()), o => IsNameValid(o.ToString()));
            SwitchToSelectToolCommand = new DelegateCommand(o => SwitchToSelectTool(), o => IsThereAnySelectTools());
            SaveCommand = new DelegateCommand(async o => await SaveDocument(o));
            OpenCommand = new DelegateCommand(o => OpenDocument(o));
            CreateTextBoxCommand = new DelegateCommand(CreateTextBox);
            CreateImageCommand = new DelegateCommand(CreateImage);
        }

        private void CenterElement(ContentElementViewModel v, ElementCreationContext context)
        {
            v.Left = GetCenterLeft(context, v.Width);
            v.Top = GetCenterTop(context, v.Height);
        }

        private double GetCenterLeft(ElementCreationContext context, double width)
        {
            return context.ContentHorizontalOffset + context.VisibleWidth / 2 - width / 2;
        }

        private double GetCenterTop(ElementCreationContext context, double height)
        {
            return context.ContentVerticalOffset + context.VisibleHeight / 2 - height / 2;
        }

        private void CreateTextBox(object select = null)
        {
            var element = new TextBoxElementViewModel();
            if (select != null && select is ElementCreationContext context)
                NewItemRoutine(element, context);
            else
                CurrentDocument.SelectedPage.Elements.Add(element);
        }

        private void NewItemRoutine(ContentElementViewModel element, ElementCreationContext context)
        {
            CenterElement(element, context);
            CurrentDocument.SelectedPage.Elements.Add(element);
            SelectedTool = GetSelectTool();
            context.Selection.Select(element);
        }

        private void CreateImage(object select = null)
        {
            if (OpenImagePath == null) return;
            ResourceViewModel resource;
            using (var fileStream = File.Open(OpenImagePath, FileMode.Open, FileAccess.Read))
            {
                resource = CurrentDocument.AddResource(fileStream);
            } // Release the file, we already copied it. 

            var element = new ImageElementViewModel(new ImageElement(resource.Model));
            if (select != null && select is ElementCreationContext context) NewItemRoutine(element, context);
        }

        private void SwitchToSelectTool()
        {
            if (SelectedTool.Mode == InkCanvasEditingMode.Select) return;
            var tool = GetSelectTool();
            if (tool is null) throw new InvalidOperationException("There isn't any select tool in the list. Wait why?");

            SelectedTool = tool;
        }

        public void SelectionHandler(object sender, InkCanvasSelectionChangingEventArgs e)
        {
            if (e.GetSelectedElements().Count + e.GetSelectedStrokes().Count > 0) SwitchToSelectTool();
        }

        private ToolViewModel GetSelectTool()
        {
            return !IsThereAnySelectTools() ? null : Tools.First(t => t.Mode == InkCanvasEditingMode.Select);
        }

        private bool IsThereAnySelectTools()
        {
            return Tools.Any(t => t.Mode == InkCanvasEditingMode.Select);
        }

        // The following shall be replaced with some zippy archives and resources and isf and blah and wow and everything you need to be gud
        // Oh wait, it is now XD
        private async Task SaveDocument(object progress = null)
        {
            if (SavePath == null) return;
            try
            {
                IProgressInteraction progressInteractionProcessed = null;
                if (progress is IInteractionProvider<IProgressInteraction> provider)
                {
                    progressInteractionProcessed = provider.CreateInteraction();
                }
                else
                {
                    throw new ArgumentNullException(nameof(progress));
                }
                await DocumentSerializer.SaveCompressedDocument(CurrentDocument.Model, SavePath,
                    progressInteractionProcessed);
                progressInteractionProcessed?.CloseAsync();
            }
            finally
            {
                SavePath = null;
            }
        }

        private void OpenDocument(object progress = null)
        {
            if (OpenPath == null) return;
            try
            {
                using (var fileStream = File.Open(OpenPath, FileMode.Open))
                {
                    CurrentDocument = new DocumentViewModel(DocumentSerializer.TryOpenDocument(fileStream));
                }
            }
            finally
            {
                OpenPath = null;
            }
        }


        private void SetToolByName(string name)
        {
            if (!IsNameValid(name)) return;
            SelectedTool = Tools.First(t => t.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
        }

        private bool IsNameValid(string name)
        {
            return Tools.Any(t => t.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
        }

        private void DeletePage(int index)
        {
            if (CanDeletePage)
            {
                CurrentDocument.Pages.RemoveAt(index);
                UpdatePageManipulation();
                if (index == 0)
                    CurrentDocument.SelectedIndex = CurrentDocument.SelectedIndex; // Update the deleted page :v
            }
        }

        private void ExtendPage(string direction)
        {
            if (direction.Equals("Right", StringComparison.InvariantCultureIgnoreCase))
                CurrentDocument.SelectedPage.Width += 350;
            if (direction.Equals("Bottom", StringComparison.InvariantCultureIgnoreCase))
                CurrentDocument.SelectedPage.Height += 350;
        }

        private void ChangePage(Direction direction)
        {
            if (CanChangePage(direction)) document.SelectedIndex += (int) direction;
        }

        private void ChangePage(int index)
        {
            if (CanChangePage(index)) document.SelectedIndex = index;
        }

        private void CreateNewPage()
        {
            CurrentDocument.Pages.Add(new PageViewModel());
            ChangePage(Direction.Forwards);
        }

        private bool CanChangePage(Direction direction)
        {
            return direction == Direction.Forwards && document.SelectedIndex + 1 != document.Pages.Count ||
                   direction == Direction.Backwards && document.SelectedIndex - 1 >= 0;
        }

        private bool CanChangePage(int index)
        {
            return document.Pages.Count > index;
        }

        private enum Direction
        {
            Forwards = 1,
            Backwards = -1
        }
    }
}