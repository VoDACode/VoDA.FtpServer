# VoDA.FtpServer

[![nuget](https://img.shields.io/static/v1?label=NuGet&message=VoDA.FtpServer&color=blue&logo=nuget)](https://www.nuget.org/packages/VoDA.FtpServer)

# Description

VoDA.FtpServer is a simple FTP server library. This library simplifies interaction with the FTP protocol down to the events level. All requests to the server related to authorization or working with data cause events that you must implement.

## Quick start

To start the server, you need to create an [FtpServer](https://github.com/VoDACode/VoDA.FtpServer/blob/master/VoDA.FtpServer/FtpServer.cs) object and call the StartAsync function.

Server configuration takes place in the [FtpServer](https://github.com/VoDACode/VoDA.FtpServer/blob/master/VoDA.FtpServer/FtpServer.cs) constructor.

An example of an FTP server for working with the file system is given in the [Test](https://github.com/VoDACode/VoDA.FtpServer/tree/master/Test) project.

## Example

```c#
var server = new FtpServer(
(config) =>
{
    config.Port = 21; // enter the port
    config.Address = System.Net.IPAddress.Any; // 
    config.Certificate.CertificatePath = ".\\server.crt";
    config.Certificate.CertificateKey = ".\\server.key";
},
(fs) =>
{
    fs.OnDelete += (client, path) => {...}; // delete file event
    fs.OnRename += (client, from, to) => {...}; // rename item event
    fs.OnDownload += (client, path) => {...};   // download file event
    fs.OnGetList += (client, path) => {...};    // get items in folder event
    fs.OnExistFile += (client, path) => {...};  // file check event
    fs.OnExistFoulder += (client, path) => {...};   // folder check event
    fs.OnCreate += (client, path) => {...}; // file creation event
    fs.OnAppend += (client, path) => {...}; // append file event
    fs.OnRemoveDir += (client, path) => {...};  // remove folder event
    fs.OnUpload += (client, path) => {...}; // upload file event
},
(auth) =>
{
    auth.UseAuthorization = true; // enable or disable authorization
    auth.UsernameVerification += (username) => {...}; // username verification
    auth.PasswordVerification += (username, password) => {...}; //verification of username and password
});
// Start FTP-serer
server.StartAsync(System.Threading.CancellationToken.None).Wait();
```
