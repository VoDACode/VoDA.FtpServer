
# Configuration

This document is divided into sections for ease of navigation:

- [Configuration principle](#configuration-principle)
- [ListenerSettings](#listenersettings)
- [Log](#log)
- [Certificate](#certificate)
- [Authorization](#authorization)
- [FileSystem](#filesystem)

## Configuration principle

To configure the server you must use [```FtpServerBuilder```](https://github.com/VoDACode/VoDA.FtpServer/blob/master/VoDA.FtpServer/FtpServerBuilder.cs). The configuration principle is as follows:

```c#
var server = new FtpServerBuilder()
    .[SECTION_NAME]((config) => {...})
    .[SECTION_NAME]((config) => {...})
    [...]
    .[SECTION_NAME]((config) => {...})
    .Build();
```

After calling the Build function, you will get an interface to interact with the FTP-server.

Sections are described below.

## **ListenerSettings**

*In this section, listening is configured.*

> ### ```Port```
>
>**Type:** ```System.Int32```\
>**Default:** ```21``` \
>**Description:** This parameter specifies on which port the server will be started.

> ### ```ServerIp```
>
>**Type:** ```System.Net.IPAddress```\
>**Default:** ```System.Net.IPAddress.Any``` \
>**Description:** Represents the local IP address.

> ### ```MaxConnections```
>
>**Type:** ```System.Int32```\
>**Default:** Unlimited \
>**Description:** Specifies the maximum number of connections.

## **Log**

*In this section, the log is configured.*

> ### ```Level```
>
>**Type:** ```VoDA.FtpServer.Interfaces.LogLevel```\
>**Default:** ```VoDA.FtpServer.Interfaces.LogLevel.Information``` \
>**Description:** Specifies the log level.

## **Certificate**

*This section configures the security certificate.*

> ### ```CertificatePath```
>
>**Type:** ```System.String```\
>**Description:** Specifies the path to the certificate.

> ### ```CertificateKey```
>
>**Type:** ```System.String```\
>**Description:** Specifies the path to the private key.

## **Authorization**

*In this section, authorization is configured.*

> ### ```UseAuthorization```
>
>**Type:** ```System.Boolean```\
>**Default:** ```false``` \
>**Description:** Enables and disables the use of authorization.

> ### ```UsernameVerification```
>
>**Type:** *event* ```VoDA.FtpServer.Delegates.AuthorizationUsernameDelegate```\
>**Parameters:** (```System.String``` username)\
>**Return:** ```System.Boolean```\
>**Description:** Called to validate the username.

> ### ```PasswordVerification```
>
>**Type:** *event* ```VoDA.FtpServer.Delegates.AuthorizationDelegate```\
>**Parameters:** (```System.String``` username, ```System.String``` password)\
>**Return:** ```System.Boolean```\
>**Description:** Called to validate the username and password.

## **FileSystem**

*In this section, the processing of requests for work with the file system is configured.*

> ### ```OnDownload```
>
>**Type:** *event* ```VoDA.FtpServer.Delegates.FileSystemDownloadDelegate```\
>**Parameters:** (```VoDA.FtpServer.Interfaces.IFtpClient``` client, ```System.String``` path)\
>**Return:** ```System.IO.FileStream```\
>**Description:** Called when a file download request is received.

> ### ```OnUpload```
>
>**Type:** *event* ```VoDA.FtpServer.Delegates.FileSystemUploadDelegate```\
>**Parameters:** (```VoDA.FtpServer.Interfaces.IFtpClient``` client, ```System.String``` path)\
>**Return:** ```System.IO.FileStream```\
>**Description:** Called when a file upload request is received.

> ### ```OnGetList```
>
>**Type:** *event* ```VoDA.FtpServer.Delegates.FileSystemGetListDelegate```\
>**Parameters:** (```VoDA.FtpServer.Interfaces.IFtpClient``` client, ```System.String``` path)\
>**Return:** (```IReadOnlyList<VoDA.FtpServer.Models.DirectoryModel>```, ```IReadOnlyList<VoDA.FtpServer.Models.FileModel>```)\
>**Description:** Called when requesting to retrieve content from a folder.

> ### ```OnExistFile```
>
>**Type:** *event* ```VoDA.FtpServer.Delegates.FileSystemExistDelegate```\
>**Parameters:** (```VoDA.FtpServer.Interfaces.IFtpClient``` client, ```System.String``` path)\
>**Return:** ```System.Boolean```\
>**Description:** Called to check for the existence of a file.

> ### ```OnExistFoulder```
>
>**Type:** *event* ```VoDA.FtpServer.Delegates.FileSystemExistDelegate```\
>**Parameters:** (```VoDA.FtpServer.Interfaces.IFtpClient``` client, ```System.String``` path)\
>**Return:** ```System.Boolean```\
>**Description:** Called to check for the existence of a folder.

> ### ```OnCreate```
>
>**Type:** *event* ```VoDA.FtpServer.Delegates.FileSystemCreateDelegate```\
>**Parameters:** (```VoDA.FtpServer.Interfaces.IFtpClient``` client, ```System.String``` path)\
>**Return:** ```System.Boolean```\
>**Description:** Called when a request to create a folder is made.

> ### ```OnAppend```
>
>**Type:** *event* ```VoDA.FtpServer.Delegates.FileSystemAppendDelegate```\
>**Parameters:** (```VoDA.FtpServer.Interfaces.IFtpClient``` client, ```System.String``` path)\
>**Return:** ```System.IO.FileStream```\
>**Description:** Called when a request is made to edit the contents of the file.

> ### ```OnRemoveDir```
>
>**Type:** *event* ```VoDA.FtpServer.Delegates.FileSystemDeleteDelegate```\
>**Parameters:** (```VoDA.FtpServer.Interfaces.IFtpClient``` client, ```System.String``` path)\
>**Return:** ```System.Boolean```\
>**Description:** Called when a request to delete a folder is received.

> ### ```OnDelete```
>
>**Type:** *event* ```VoDA.FtpServer.Delegates.FileSystemDeleteDelegate```\
>**Parameters:** (```VoDA.FtpServer.Interfaces.IFtpClient``` client, ```System.String``` path)\
>**Return:** ```System.Boolean```\
>**Description:** Called when a file is requested to be deleted.

> ### ```OnGetFileSize```
>
>**Type:** *event* ```VoDA.FtpServer.Delegates.FileSystemGetFileSizeDelegate```\
>**Parameters:** (```VoDA.FtpServer.Interfaces.IFtpClient``` client, ```System.String``` path)\
>**Return:** ```System.Int64```\
>**Description:** Called when a file size request is received.
