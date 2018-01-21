using Prism.Mvvm;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System;
using System.Diagnostics;
using System.IO;
using System.Reactive.Disposables;

namespace WpfApp1
{
    class MainWindowViewModel : BindableBase
    {
        public DirectoryInfo RootDirectory { get; } = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.Desktop));

        public ReactivePropertySlim<DirectoryInfo> CurrentDirectory { get; } = new ReactivePropertySlim<DirectoryInfo>();

        public MainWindowViewModel()
        {
            CurrentDirectory.Subscribe(x => Debug.WriteLine(x)).AddTo(_compositeDisposable);
        }

        private CompositeDisposable _compositeDisposable = new CompositeDisposable();
    }
}
