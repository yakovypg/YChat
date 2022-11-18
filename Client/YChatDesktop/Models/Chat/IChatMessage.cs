using Avalonia.Controls;
using Avalonia.Media;
using System;

namespace YChatDesktop.Models.Chat
{
    public interface IChatMessage
    {
        string? Text { get; }
        string Sender { get; }

        DateTime SentDate { get; }
        string SentDatePresenter { get; }

        bool IsOwn { get; }
        Dock Dock { get; }
        IBrush Background { get; }
    }
}