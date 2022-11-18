using Avalonia.Controls;
using Avalonia.Controls.Templates;
using System;
using YChatDesktop.ViewModels;

namespace YChatDesktop
{
    public class ViewLocator : IDataTemplate
    {
        public IControl Build(object data)
        {
            var name = data.GetType().FullName!.Replace("ViewModel", "View");
            var type = Type.GetType(name);

            return type is not null
                ? (Control)Activator.CreateInstance(type)!
                : (IControl)new TextBlock { Text = "Not Found: " + name };
        }

        public bool Match(object data)
        {
            return data is ViewModelBase;
        }
    }
}
