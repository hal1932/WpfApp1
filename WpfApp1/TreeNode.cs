using Prism.Mvvm;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    class TreeNode : BindableBase
    {
        public ReactivePropertySlim<bool> IsSelected { get; } = new ReactivePropertySlim<bool>();
        public ReactivePropertySlim<bool> IsExpanded { get; } = new ReactivePropertySlim<bool>();

        public ReactiveCollection<TreeNode> Children { get; } = new ReactiveCollection<TreeNode>();

        public TreeNode(bool expandable)
        {
            if (expandable)
            {
                Children.Add(null);
            }
        }
    }
}
