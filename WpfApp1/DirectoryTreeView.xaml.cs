using Reactive.Bindings;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace WpfApp1
{
    /// <summary>
    /// DirectoryTreeView.xaml の相互作用ロジック
    /// </summary>
    public partial class DirectoryTreeView : UserControl
    {
        #region ItemsSource
        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register(
                "ItemsSource",
                typeof(IEnumerable),
                typeof(DirectoryTreeView),
                new PropertyMetadata(null));
        #endregion

        #region ItemContextMenu
        public ContextMenu ItemContextMenu
        {
            get { return (ContextMenu)GetValue(ItemContextMenuProperty); }
            set { SetValue(ItemContextMenuProperty, value); }
        }

        public static readonly DependencyProperty ItemContextMenuProperty =
            DependencyProperty.Register(
                "ItemContextMenu",
                typeof(ContextMenu),
                typeof(DirectoryTreeView),
                new PropertyMetadata(null));
        #endregion

        #region SelectedDirectory
        public DirectoryInfo SelectedDirectory
        {
            get { return (DirectoryInfo)GetValue(SelectedDirectoryProperty); }
            set { SetValue(SelectedDirectoryProperty, value); }
        }

        public static readonly DependencyProperty SelectedDirectoryProperty =
            DependencyProperty.Register(
                "SelectedDirectory",
                typeof(DirectoryInfo),
                typeof(DirectoryTreeView),
                new PropertyMetadata(null));
        #endregion

        #region ExpandedDirectory
        public DirectoryInfo ExpandedDirectory
        {
            get { return (DirectoryInfo)GetValue(ExpandedDirectoryProperty); }
            set { SetValue(ExpandedDirectoryProperty, value); }
        }

        public static readonly DependencyProperty ExpandedDirectoryProperty =
            DependencyProperty.Register(
                "ExpandedDirectory",
                typeof(DirectoryInfo),
                typeof(DirectoryTreeView),
                new PropertyMetadata(null));
        #endregion

        #region RootDirectory
        public DirectoryInfo RootDirectory
        {
            get { return (DirectoryInfo)GetValue(RootDirectoryProperty); }
            set { SetValue(RootDirectoryProperty, value); }
        }

        public static readonly DependencyProperty RootDirectoryProperty =
            DependencyProperty.Register(
                "RootDirectory",
                typeof(DirectoryInfo),
                typeof(DirectoryTreeView),
                new PropertyMetadata(
                    null,
                    (d, e) =>
                    {
                        var control = (d as DirectoryTreeView);
                        if (control == null)
                        {
                            return;
                        }

                        var directory = e.NewValue as DirectoryInfo;
                        if (directory != null)
                        {
                            var rootItem = new DirectoryTreeItem(directory, control.ShowFiles)
                            {
                                OnExpanded = control.OnItemExpanded,
                                OnSelecetd = control.OnItemSelected,
                            };
                            control.ItemsSource = new[] { rootItem };
                        }
                        else
                        {
                            control.ItemsSource = null;
                        }
                    }));
        #endregion

        #region ShowFiles
        public bool ShowFiles
        {
            get { return (bool)GetValue(ShowFilesProperty); }
            set { SetValue(ShowFilesProperty, value); }
        }

        public static readonly DependencyProperty ShowFilesProperty =
            DependencyProperty.Register(
                "ShowFiles",
                typeof(bool),
                typeof(DirectoryTreeView),
                new PropertyMetadata(
                    false,
                    new PropertyChangedCallback((d, e) =>
                    {
                        var control = (d as DirectoryTreeView);
                        if (control == null)
                        {
                            return;
                        }

                        var showFiles = (bool)e.NewValue;
                        if (showFiles)
                        {
                            control.FileImageSource = _fileImageSource ?? (_fileImageSource = GetImageSourceFromResource("WpfApp1.FileSystemEditor_5852.png"));
                        }
                        else
                        {
                            control.FileImageSource = null;
                        }

                        if (control.ItemsSource != null)
                        {
                            foreach (DirectoryTreeItem item in control.ItemsSource)
                            {
                                item.ShowFiles = showFiles;
                            }
                        }
                    })));
        #endregion

        #region ItemExpandedCommand
        public ICommand ItemExpandedCommand
        {
            get { return (ICommand)GetValue(ItemExpandedCommandProperty); }
            set { SetValue(ItemExpandedCommandProperty, value); }
        }

        public static readonly DependencyProperty ItemExpandedCommandProperty =
            DependencyProperty.Register(
                "ItemExpandedCommand",
                typeof(ICommand),
                typeof(DirectoryTreeView),
                null);
        #endregion

        #region ItemSelectedCommand
        public ICommand ItemSelectedCommand
        {
            get { return (ICommand)GetValue(ItemSelectedCommandProperty); }
            set { SetValue(ItemSelectedCommandProperty, value); }
        }

        public static readonly DependencyProperty ItemSelectedCommandProperty =
            DependencyProperty.Register(
                "ItemSelectedCommand",
                typeof(ICommand),
                typeof(DirectoryTreeView),
                null);
        #endregion

        public ImageSource DirectoryImageSource { get; }
        public ImageSource FileImageSource { get; private set; }

        public DirectoryTreeView()
        {
            InitializeComponent();
            _rootControl.DataContext = this;

            DirectoryImageSource = _directoryImageSource ?? (_directoryImageSource = GetImageSourceFromResource("WpfApp1.Folder_6221.png"));
        }

        private void OnItemExpanded(DirectoryTreeItem item, bool value)
        {
            if (ItemExpandedCommand?.CanExecute(this) ?? false)
            {
                ItemExpandedCommand.Execute(this);
            }
        }

        private void OnItemSelected(DirectoryTreeItem item, bool value)
        {
            if (ItemSelectedCommand?.CanExecute(this) ?? false)
            {
                ItemSelectedCommand.Execute(this);
            }
        }

        private static ImageSource GetImageSourceFromResource(string name)
        {
            var bmp = new BitmapImage();
            bmp.BeginInit();
            bmp.CacheOption = BitmapCacheOption.OnLoad;
            bmp.StreamSource = Assembly.GetExecutingAssembly().GetManifestResourceStream(name);
            bmp.EndInit();
            bmp.Freeze();
            return bmp;
        }

        private static ImageSource _directoryImageSource;
        private static ImageSource _fileImageSource;
    }
}
