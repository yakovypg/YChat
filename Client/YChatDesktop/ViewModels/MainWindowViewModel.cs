using Avalonia.Threading;
using ChatEngine.ClientSide;
using ChatEngine.Configuration;
using ChatEngine.Models;
using MessageBox.Avalonia;
using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Reflection;
using System.Threading.Tasks;
using YChatDesktop.Models;
using YChatDesktop.Models.Chat;
using YChatDesktop.Models.Loggers;

namespace YChatDesktop.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        #region Commands

        public ReactiveCommand<Unit, Task> ConnectToServerCommand { get; }
        public ReactiveCommand<Unit, Unit> DisconnectFromServerCommand { get; }

        public ReactiveCommand<string, Unit> SendMessageCommand { get; }
        public ReactiveCommand<Unit, Unit> ClearChatBoardCommand { get; }

        #endregion

        #region InfoProperties

        public static string AppVersion
        {
            get
            {
                AssemblyName assemblyName = Assembly.GetExecutingAssembly().GetName();
                string? name = assemblyName.Name?.ToString();
                string? version = assemblyName.Version?.ToString();

                return $"{name} {version}";
            }
        }

        #endregion

        #region ConnectionProperties

        private string _nick = "User";
        private int _port = SharedConfig.NetConfig.DEFAULT_PORT;
        private string _host = SharedConfig.NetConfig.DEFAULT_HOST;

        private bool _isConnectedToServer = false;

        public string Nick
        {
            get => _nick;
            set => this.RaiseAndSetIfChanged(ref _nick, value);
        }

        public int Port
        {
            get => _port;
            set => this.RaiseAndSetIfChanged(ref _port, value);
        }

        public string Host
        {
            get => _host;
            set => this.RaiseAndSetIfChanged(ref _host, value);
        }

        public bool IsConnectedToServer
        {
            get => _isConnectedToServer;
            private set => this.RaiseAndSetIfChanged(ref _isConnectedToServer, value);
        }

        #endregion

        #region ChatProperties

        private string _message = string.Empty;

        public string Message
        {
            get => _message;
            set => this.RaiseAndSetIfChanged(ref _message, value);
        }

        public ObservableCollection<IChatMessage> ChatMessages { get; }

        #endregion

        private TextBoxInputBoard? _inputBoard;

        public MainWindowViewModel()
        {
            ChatMessages = new ObservableCollection<IChatMessage>();

            ConnectToServerCommand = ReactiveCommand.Create(ConnectToServerAsync);
            DisconnectFromServerCommand = ReactiveCommand.Create(DisconnectFromServer);

            SendMessageCommand = ReactiveCommand.Create<string>(SendMessage);
            ClearChatBoardCommand = ReactiveCommand.Create(ClearChatBoard);
        }

        private async Task ConnectToServerAsync()
        {
            ResetProperties();

            var chatUser = new ChatUser(Nick);
            var netConfig = new NetConfig(Port, Host);
            var clientConfig = new ClientConfig(chatUser, netConfig);

            var logger = new UILogger(AddMessage, Dispatcher.UIThread);
            var inputBoard = new TextBoxInputBoard(ClearChatBoard);
            var client = new Client(clientConfig, inputBoard, logger);

            client.ConnectionError += delegate (Exception ex)
            {
                IsConnectedToServer = false;
                Dispatcher.UIThread.InvokeAsync(() => MessageBoxManager.GetMessageBoxStandardWindow("Error", ex.Message).Show());
            };

            client.EndOfMessageReceiving += delegate
            {
                IsConnectedToServer = false;
            };

            _inputBoard = inputBoard;
            IsConnectedToServer = true;

            await client.ConnentToServerAsync();
        }

        private void DisconnectFromServer()
        {
            SendMessage("/exit");
            IsConnectedToServer = false;
        }

        private void SendMessage(string message)
        {
            _inputBoard?.AddLine(message);

            Message = string.Empty;
            ChatMessages.Add(new ChatMessage(message, Nick, true));
        }

        private void ClearChatBoard()
        {
            ChatMessages.Clear();
        }

        private void ResetProperties()
        {
            Message = string.Empty;
            ChatMessages.Clear();
        }

        private void AddMessage(string? message)
        {
            const string unknownUserNick = "*";
            int separatorIndex = message?.IndexOf(':') ?? -1;

            if (separatorIndex <= 0 || message is null)
            {
                ChatMessages.Add(new ChatMessage(message, unknownUserNick, false));
                return;
            }

            string userNick = message.Remove(separatorIndex);

            string userMessage = separatorIndex + 2 < message.Length
                ? message[(separatorIndex + 2)..]
                : string.Empty;

            ChatMessages.Add(new ChatMessage(userMessage, userNick, false));
        }
    }
}
