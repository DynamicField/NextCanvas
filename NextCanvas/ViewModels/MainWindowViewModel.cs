#region

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
using NextCanvas.Interactivity;
using NextCanvas.Interactivity.Multimedia;
using NextCanvas.Interactivity.Progress;
using NextCanvas.Models;
using NextCanvas.Models.Content;
using NextCanvas.Serialization;
using NextCanvas.Utilities;
using NextCanvas.Utilities.Content;
using NextCanvas.ViewModels.Content;

#endregion

namespace NextCanvas.ViewModels
{
    public class MainWindowViewModel : ViewModelBase<MainWindowModel>
    {
        private DocumentViewModel document;

        private ElementCreationContext elementCreationContext;
        public IInteractionProvider<IErrorInteraction> ErrorProvider { get; set; }
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

        public ElementCreationContext ElementCreationContext
        {
            get => elementCreationContext;
            set
            {
                elementCreationContext = value;
                OnPropertyChanged(nameof(ElementCreationContext));
            }
        }

        // Pages
        public DelegateCommand PreviousPageCommand { get; private set; }
        public DelegateCommand NextPageCommand { get; private set; }
        public DelegateCommand NewPageCommand { get; private set; }
        public DelegateCommand DeletePageCommand { get; private set; }

        public DelegateCommand ExtendPageCommand { get; private set; }

        // Tools
        public DelegateCommand SetToolByNameCommand { get; private set; }

        // File saving
        public DelegateCommand SaveCommand { get; private set; }

        public DelegateCommand OpenCommand { get; private set; }

        // Content
        public DelegateCommand CreateTextBoxCommand { get; private set; }
        public DelegateCommand SwitchToSelectToolCommand { get; private set; }
        public DelegateCommand CreateImageCommand { get; private set; }
        public DelegateCommand CreateScreenshotCommand { get; private set; }
        public DelegateCommand CreateWebBrowserCommand { get; private set; }
        public string PageDisplayText => CurrentDocument.SelectedIndex + 1 + "/" + CurrentDocument.Pages.Count;

        private DocumentReader DocumentReader { get; } = new DocumentReader();
        private DocumentSaver DocumentSaver { get; } = new DocumentSaver();

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
            UpdatePageManipulation();
        }

        private void UpdatePageText()
        {
            OnPropertyChanged(nameof(PageDisplayText));
        }

        private void Initialize()
        {
            document = new DocumentViewModel(Model.Document);
            Tools = new ObservableViewModelCollection<ToolViewModel, Tool>(Model.Tools, ToolViewModel.GetViewModel);
            Subscribe();
            // Create commands
            PreviousPageCommand = new DelegateCommand(o => ChangePage(Direction.Backwards),
                o => CanChangePage(Direction.Backwards));
            NextPageCommand = new DelegateCommand(o => ChangePage(Direction.Forwards),
                o => CanChangePage(Direction.Forwards));
            NewPageCommand = new DelegateCommand(CreateNewPage);
            DeletePageCommand = new DelegateCommand(o => DeletePage(CurrentDocument.SelectedIndex), o => CurrentDocument.CanDeletePage);
            ExtendPageCommand = new DelegateCommand(ExtendPage);
            SetToolByNameCommand = new DelegateCommand(SetToolByName, IsNameValid);
            SwitchToSelectToolCommand = new DelegateCommand(SwitchToSelectTool, IsThereAnySelectTools);
            SaveCommand = new DelegateCommand(async o => await SaveDocument(o));
            OpenCommand = new DelegateCommand(OpenDocument);
            CreateTextBoxCommand = new DelegateCommand(CreateTextBox);
            CreateImageCommand = new DelegateCommand(CreateImage);
            CreateScreenshotCommand = new DelegateCommand(CreateScreenShot);
            CreateWebBrowserCommand = new DelegateCommand(CreateWebBrowser);
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

        private void CreateTextBox()
        {
            var element = new TextBoxElementViewModel();
            ProcessItem(element);
        }

        private void ProcessItem(ContentElementViewModel element)
        {
            if (ElementCreationContext == null)
            {
                AddElementToSelectedPage(element);
                return;
            }
            CenterElement(element, ElementCreationContext);
            AddElementToSelectedPage(element);
            SelectedTool = GetSelectTool();
            ElementCreationContext.Selection.Select(element);
        }

        private void CreateImage()
        {
            if (OpenImagePath == null) return;

            ResourceViewModel resource;
            using (var fileStream = File.Open(OpenImagePath, FileMode.Open, FileAccess.Read))
            {
                resource = CurrentDocument.AddResource(fileStream);
            } // Release the file, we already copied it. 

            AddImage(resource);
        }

        private void AddImage(ResourceViewModel resource)
        {
            var element = new ImageElementViewModel(new ImageElement(resource.Model));
            if (ElementCreationContext != null)
            {
                // Sets the width and the height to the image's default dimensions. Resizes if the page or visible height is too small.
                // Tries to :
                // 1. Stretch the whole visible height.
                // 2. Keeps image's max dimensions.
                // 3. Tries to not exceed the page's dimensions.
                var cappedHeight = (ElementCreationContext.VisibleHeight - 25).Cap(100);
                var cappedWidth = (ElementCreationContext.VisibleWidth - 25).Cap(100);
                element.Width = Math.Min(Math.Min(cappedWidth, document.SelectedPage.Width), element.Image.PixelWidth);
                element.Height = Math.Min(Math.Min(cappedHeight, document.SelectedPage.Height),
                    element.Image.PixelHeight);
            }
            ProcessItem(element);
        }

        private void CreateScreenShot(object interaction)
        {
            if (!(interaction is IInteractionProvider<IScreenshotInteraction> interact)) return;
            var screenshotter = interact.CreateInteraction();
            screenshotter.ShowInteraction();
            screenshotter.ActionComplete += Screenshotter_ActionComplete;
            // yes.
        }

        private void Screenshotter_ActionComplete(object sender, ScreenshotTakenEventArgs e)
        {
            var resource = CurrentDocument.AddResource(e.ImageData, "Screenshot" + DateTime.Now.ToString("s").Replace(":", ".") + e.ImageExtension);
            AddImage(resource);
        }

        private void CreateWebBrowser(object interaction)
        {
            if (!(interaction is IInteractionProvider<IModifyObjectInteraction> interact)) return;
            var editor = interact.CreateInteraction();
            var browserInstance = new WebBrowserElementViewModel();
            editor.ObjectToModify = browserInstance;
            editor.HeaderText = "Creating a web browser...";
            editor.ActionComplete += (sender, args) => { ProcessItem(browserInstance); };
            editor.ShowInteraction();
        }

        // Pages things

        private void AddElementToSelectedPage(ContentElementViewModel element)
        {
            CurrentDocument.SelectedPage.Elements.Add(element);
        }

        private void SwitchToSelectTool()
        {
            if (SelectedTool.Mode == InkCanvasEditingMode.Select) return;

            var tool = GetSelectTool();

            SelectedTool =
                tool ?? throw new InvalidOperationException("There isn't any select tool in the list. Wait why?");
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
                var progressInteractionProcessed = GetProgressInteraction(progress);
                await DocumentSaver.SaveCompressedDocument(CurrentDocument.Model, SavePath,
                    progressInteractionProcessed);
            }
            catch (Exception e)
            {
                if (ErrorProvider == null) throw;
                var error = ErrorProvider.CreateInteraction();
                error.Content = "Sorry, we weren't able to save your file: " + e.Message;
                error.Title = "Oops :(";
                error.ShowInteraction();
            }
            finally
            {
                SavePath = null;
            }
        }

        private static IProgressInteraction GetProgressInteraction(object progress)
        {
            IProgressInteraction progressInteractionProcessed;
            if (progress is IInteractionProvider<IProgressInteraction> provider)
                progressInteractionProcessed = provider.CreateInteraction();
            else
                throw new ArgumentNullException(nameof(progress));

            return progressInteractionProcessed;
        }

        private void OpenDocument(object progress = null)
        {
            if (OpenPath == null) return;
            try
            {
                if (!(progress is IInteractionProvider<IProgressInteraction> provider)) return;
                using (var fileStream = File.Open(OpenPath, FileMode.Open))
                {
                    document.Dispose();
                    CurrentDocument =
                        new DocumentViewModel(DocumentReader.TryOpenDocument(fileStream,
                            provider.CreateInteraction()));
                }
            }
            catch (Exception e)
            {
                if (ErrorProvider == null) throw;
                var error = ErrorProvider.CreateInteraction();
                error.Content = "Sorry, we weren't able to open your file: " + e.Message;
                error.Title = "Oops :(";
                error.ShowInteraction();
            }
            finally
            {
                OpenPath = null;
            }
        }

        private void SetToolByName(object name) => SetToolByName(name.ToString());
        private void SetToolByName(string name)
        {
            if (!IsNameValid(name)) return;

            SelectedTool = Tools.First(t => t.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
        }

        private bool IsNameValid(object name) => IsNameValid(name.ToString());
        private bool IsNameValid(string name)
        {
            return Tools.Any(t => t.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
        }

        private void DeletePage(int index)
        {
            if (CurrentDocument.CanDeletePage)
            {
                CurrentDocument.Pages.RemoveAt(index);
            }
        }

        private void ExtendPage(object direction) => ExtendPage(direction.ToString());
        private void ExtendPage(string direction)
        {
            if (direction.Equals("Right", StringComparison.InvariantCultureIgnoreCase))
                CurrentDocument.SelectedPage.Width += 350;

            if (direction.Equals("Bottom", StringComparison.InvariantCultureIgnoreCase))
                CurrentDocument.SelectedPage.Height += 350;
        }

        private void ChangePage(Direction direction)
        {
            if (CanChangePage(direction)) document.SelectedIndex += (int)direction;
        }

        public void ChangePage(int index)
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