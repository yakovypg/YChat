using Avalonia.Controls;
using Avalonia.Media;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace YChatDesktop.Models.Chat
{
    public class ChatMessage : IChatMessage, IEquatable<ChatMessage?>
    {
        public string? Text { get; }
        public string Sender { get; }
        
        public DateTime SentDate { get; }
        public string SentDatePresenter => SentDate.ToShortTimeString();

        public bool IsOwn { get; }
        public Dock Dock => IsOwn ? Dock.Right : Dock.Left;
        public IBrush Background => IsOwn ? GreenBrush : YellowBrush;

        protected static IBrush YellowBrush => Brushes.LightYellow;
        protected static IBrush GreenBrush => new SolidColorBrush(Color.FromUInt32(uint.Parse("ffe0ffe0", NumberStyles.HexNumber)));

        public ChatMessage(string? text, string sender, bool isOwn = false) : this(text, sender, DateTime.Now, isOwn)
        {
        }

        public ChatMessage(string? text, string sender, DateTime sentDate, bool isOwn = false)
        {
            Text = text;
            Sender = sender;
            SentDate = sentDate;
            IsOwn = isOwn;
        }

        public bool Equals(ChatMessage? other)
        {
            return other is not null &&
                   Text == other.Text &&
                   Sender == other.Sender &&
                   SentDate == other.SentDate &&
                   IsOwn == other.IsOwn;
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as ChatMessage);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Text, Sender, SentDate, IsOwn);
        }

        public static bool operator ==(ChatMessage? left, ChatMessage? right)
        {
            return EqualityComparer<ChatMessage>.Default.Equals(left, right);
        }

        public static bool operator !=(ChatMessage? left, ChatMessage? right)
        {
            return !(left == right);
        }
    }
}
