# YChat

YChat is a cross-platform application for group communication. The application has both a CLI and a graphical version, 
so that it is convenient for both command-line lovers and people who prefer graphical shells.

## Start of use

### Release usage

To use the built project, follow these steps:
1. Choose the release.
2. Download and unzip the archive with the version for your operating system and architecture.
3. Move the folder to any place convenient for you. The tool does not create any files outside 
of its directory.
4. Go to this folder and run the executable file.

You can also create a shortcut to the executable file and put it in a convenient place for you.

Please note that if you want to run the CLI version of the tool, run the executable file from 
the console. For example, you can find out the version of the tool using the on of the command
below.

On Windows.

```
ypdf --version
```

On Linux and Mac.

```
./ypdf --version
```

### Build from source

We will build the project using [dotnet](https://dotnet.microsoft.com/en-us/). But you can 
also use the IDE tools, then the project will be built in one click of the corresponding 
button.

First, clone the repository.

```
git clone https://github.com/yakovypg/YChat.git
```

Go to the project folder.

```
cd YChat
```

[Build](https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-build) the project.

```
dotnet build -c Release
```

Now you can already run, for example, the server.

```
dotnet run --project Server/YChatServer
```

You can also [publish](https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-publish) 
the project. For example, we will publish the client CLI application.

```
dotnet publish Client/YChatDesktop -c Release -o pathToPublishFolder
```

The published project is runtime-dependent. That is, to run it, it is necessary that .NET 
runtime is installed on your system. You can run the tool using the following command:

```
dotnet pathToPublishFolder/YChatDesktop.dll
```

You can also publish the project with the --self-contained parameter. Then, to run the tool, 
it is not required that .NET runtime is installed on your system. To publish a project with 
this parameter, you should also specify the target platform and architecture using -r 
parameter. For example, we will publish a project for linux-x64.

```
dotnet publish YChat/YChatDesktop -c Release -o pathToPublishFolder --self-contained True -r linux-x64
```

A published project can be run without dotnet. To do this, navigate to the folder where you 
published the project and enter one of the commands below.

On Windows.

```
YChatDesktop
```

On Linux and Mac.

```
./YChatDesktop
```

### Development

The project is developed for the .NET 6.0 platform. To continue development, you will need the .NET SDK and .NET 
Runtime.

## How to use CLI client application

### Basics

You can execute the command by sending a message with the command at the beginning. The command must start with the 
character '/' followed by the command name. If you want to send a message that starts with the character '/', but which 
is not a command, start the message with two consecutive characters '/'.

There is support for the following commands
- Exit from chat: /exit
- Clear chat: /clear
- Get chat participants: /list

### Get help

Getting the list of parameters with explanations.

```
YChat --help
```

### Get version

Getting the current application version.

```
YChat --version
```

### Connect to the chat

First, make sure that the server is running.

To connect to the chat, you need to specify the host and port using the -h and -p parameters. You can also 
specify your nickname using the -n parameter.

```
YChat -p 5000 -h 127.0.0.1 -n nick
```

By default, the host is 127 and the port is 5000. If you have started the server with default parameters on the 
local host, you can not explicitly specify the host and port.

```
YChat -n nick
```

## How to use CLI server application

### Start the server

To start the server, run the following command. The only argument is the port on which the server will be 
started.

```
YChatServer 5000
```

By default, the port is 5000. You can not explicitly specify it.

```
YChatServer
```