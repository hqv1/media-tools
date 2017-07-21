# media-tools
Tools for media creation, processing and management. All written in C# standard/core. So will work on Windows, Mac, and Linux.

# Components
## Create thumbnails
Create thumbnails every *x* seconds

## Create thumbnail sheet
Create a thumbnail sheet from a video file. 

## File Download
Download a file using FFmpeg. Usually used to download a streaming video. You can convert it into a different format at the same time.

## Get video file information
Get video file information (duration, video stream, etc...) from a video file. 

# Console application

The console app can do two things
* Create thumbnails every *x* seconds from a video file
* Create a thumbnail sheet

To build the console app...
* Clone or download the project
* Open up a command line and go to the root directory (it'll contain the ```MediaTools.sln``` file)
* Run ```dotnet clean```. You'll don't need this on a new clone, but it doesn't hurt.
* Run ```dotnet restore```. The command will go and grab all the required nuget dependency. We use a private feed for a few libraries, but the command should be able to find the feed (through the solution's nuget.config file).
* Run ```dotnet build -c Release``` to build the application in Release mode.
* Go to the built directory for the Console application. (In my case it is ```media-tools\Console\bin\Release\netcoreapp1.1```).
* You'll need to update ```appsettings.json``` to set up your paths. You can also update logging preferences there.

Run ```dotnet Hqv.MediaTools.Console.dll --help ``` to get information on how to run the application.

You are ready to go. Enjoy.

# Libraries
You can also use my libraries in your applications. But you'll need some things first.

## Requirements
You'll need to add my private nuget feed to get some common libraries. View the nuget.config to find the feed information.

The nuget feeds will have the latest builds. So you can use the nuget packages, or build them yourself.

## Types
Contains interfaces, entities, and other common files. Reference this library when you only need the interfaces and not the implemention. 

## File Download
Download a file using FFmpeg. Usually used to download a streaming video. You can convert it into a different format at the same time.

## Thumbnail
Create a thumbnail every *x* seconds for a video file. The process doesn't seem to be very accurate, but you'll get thumbnails.

## ThumbnailSheet
Create a thumbnail sheet. Requires FFmpeg to be on your system. You can download from [FFmpeg][ffmpeg-url]{:target="_blank"}. 

You can configure the following things
* Number of thumbnails in the sheet
* Whether you want a timestamp on each thumbnail

## Video file information
Get video file information (duration, video stream, etc...) from a video file. Requires FFprobe to be on your system. You can download from [FFmpeg][ffmpeg-url]{:target="_blank"}. 

[ffmpeg-url]:https://ffmpeg.org/
[nlog-url]:http://nlog-project.org/