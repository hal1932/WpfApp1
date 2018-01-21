using Prism.Mvvm;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reactive.Disposables;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    class DirectoryTreeItem : TreeNode
    {
        public FileSystemInfo FileSystem { get; }
        public bool ShowFiles { get; set; }

        public FileInfo File => FileSystem as FileInfo;
        public DirectoryInfo Directory => FileSystem as DirectoryInfo;

        public bool IsFile => File != null;
        public bool IsDirectory => Directory != null;

        public Action<DirectoryTreeItem, bool> OnSelecetd { get; set; }
        public Action<DirectoryTreeItem, bool> OnExpanded { get; set; }

        public DirectoryTreeItem(DirectoryInfo directory, bool showFiles)
            : base(directory.EnumerateDirectories().Any() || (showFiles && directory.EnumerateFiles().Any()))
        {
            FileSystem = directory;
            ShowFiles = showFiles;
            SetupSubscription();
        }

        public DirectoryTreeItem(FileInfo file, bool showFiles)
            : base(false)
        {
            FileSystem = file;
            ShowFiles = showFiles;
            SetupSubscription();
        }

        private void SetupSubscription()
        {
            IsExpanded.Subscribe(x => Expand(x)).AddTo(_compositeDisposable);
            IsSelected.Subscribe(x => Select(x)).AddTo(_compositeDisposable);
        }

        private void Expand(bool expanded)
        {
            if (expanded)
            {
                Children.Clear();

                foreach (var child in Directory.EnumerateDirectories())
                {
                    Children.Add(new DirectoryTreeItem(child, ShowFiles));
                }

                if (ShowFiles)
                {
                    foreach (var child in Directory.EnumerateFiles())
                    {
                        Children.Add(new DirectoryTreeItem(child, ShowFiles));
                    }
                }
            }

            OnExpanded?.Invoke(this, expanded);
        }

        private void Select(bool selected)
        {
            OnSelecetd?.Invoke(this, selected);
        }

        private CompositeDisposable _compositeDisposable = new CompositeDisposable();
    }
}
