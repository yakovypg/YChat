# YChat

YChat is a cross-platform application for group communication. The application has both a CLI and a graphical version, 
so that it is convenient for both command-line lovers and people who prefer graphical shells.

## Start of use

### Basics

You can execute the command by sending a message with the command at the beginning. The command must start with the 
character '/' followed by the command name. If you want to send a message that starts with the character '/', but which 
is not a command, start the message with two consecutive characters '/'.

There is support for the following commands
- Exit from chat: /exit
- Clear chat: /clear
- Get chat participants: /list

### Development

The project is developed for the .NET 6.0 platform. To continue development, you will need the .NET SDK and .NET 
Runtime.

## How to use CLI client application

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