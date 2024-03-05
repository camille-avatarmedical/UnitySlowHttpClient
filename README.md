# Unity slow HttpClient repro case

This project is a reproduction project to highlight a performance issue in the HttpClient implementation that comes with Unity.

## Requirements
To have a working environment you need:
* Unity 2022.3.4f1
* A .Net core development environment or Visual Studio 22

## Setup
First you need to create a binary file and place it in Server/binary.content. We advise a file between 300 and 400MB. It is better is content is random.

Then you can start the server by doing
````
cd Server
dotnet restore
dotnet build
dotnet run --launch-profile https
`````

Then you can launch the command line download test
````
cd Client
dotnet restore
dotnet build
dotnet run
````

You should have something like this:
````
HTTPS
download time using HttpClient 2238 ms
sha1 using HttpClient 990B3A5A6F1C9ACC333E305A0E879125A9B699BD
HTTP
download time using HttpClient 1581 ms
sha1 using HttpClient 990B3A5A6F1C9ACC333E305A0E879125A9B699BD
````

Then you can open the Unity project in the Unity directory. By opening the scene named MainScene and playing it you should have something like this appear on the logs:

````
HTTPS
Download time using HttpClient 32261 ms
SHA1 using HttpClient 990B3A5A6F1C9ACC333E305A0E879125A9B699BD
HTTP
Download time using HttpClient 5120 ms
SHA1 using HttpClient 990B3A5A6F1C9ACC333E305A0E879125A9B699BD
HTTPS
Download time using UnityWebRequest 4408 ms
SHA1 using UnityWebRequest 990B3A5A6F1C9ACC333E305A0E879125A9B699BD
HTTP
Download time using UnityWebRequest 2548 ms
SHA1 using UnityWebRequest 990B3A5A6F1C9ACC333E305A0E879125A9B699BD
````


## Performance analysis

The previous logs are output from the programs on this repository run on the same machine. As we can see the performance difference is big:

* UnityWebRequest is 2x slower than HttpClient running on .Net Core.
* HttpClient running in Unity is about 10x slower than the equivalent version running on .Net Core.
* Performance varies widly between using HTTPS and HTTP.