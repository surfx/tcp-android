
# About

Server TCP para controle windows - mouse, click, som (sound), shutdown

## Create project

```
dotnet new console --name tcpserver_csharp
```

## Dependência externa

```
dotnet add package AudioSwitcher.AudioApi.CoreAudio --version 3.0.0.1
dotnet add package InputSimulator
```

### InputSimulator

```
InputSimulator sim = new InputSimulator();

// Press 0 key
sim.Keyboard.KeyPress(VirtualKeyCode.VK_0);
// Press 1
sim.Keyboard.KeyPress(VirtualKeyCode.VK_1);
// Press b
sim.Keyboard.KeyPress(VirtualKeyCode.VK_B);
// Press v
sim.Keyboard.KeyPress(VirtualKeyCode.VK_V);
// Press enter
sim.Keyboard.KeyPress(VirtualKeyCode.RETURN);
// Press Left CTRL button
sim.Keyboard.KeyPress(VirtualKeyCode.LCONTROL);
```

## Start

```
dotnet run
```

# urls

- [Audio C#](https://github.com/xenolightning/AudioSwitcher)
- [console c#](https://aka.ms/new-console-template)
- [tcp server](https://learn.microsoft.com/pt-br/dotnet/api/system.net.sockets.tcplistener?view=net-7.0)
- [mouse c#](https://stackoverflow.com/questions/2416748/how-do-you-simulate-mouse-click-in-c)
- [InputSimulator](https://ourcodeworld.com/articles/read/520/simulating-keypress-in-the-right-way-using-inputsimulator-with-csharp-in-winforms)

## TCP Ref

- [TcpListener](https://learn.microsoft.com/pt-br/dotnet/api/system.net.sockets.tcplistener?view=net-7.0)
- [TcpClient](https://learn.microsoft.com/pt-br/dotnet/api/system.net.sockets.tcpclient?view=net-7.0)
- [Multiple Clients](https://stackoverflow.com/questions/5339782/how-do-i-get-tcplistener-to-accept-multiple-connections-and-work-with-each-one-i)

## Ainda não 100%

- [Screen Size](https://stackoverflow.com/questions/43739898/c-sharp-screen-size-without-references-in-interactive)
