# 🔐 Muragala Password Manager (C# Library and CLI)
![Poster](https://user-images.githubusercontent.com/46389631/170724038-d95d0555-cb9e-46ec-9d88-66a7c5d366d3.png)

---

        NOTICE:
        I also plan to turn Password Manager into a GUI Application. I will continue to improve
        ths library and CLI program as well. Make sure to check out the Python edition as well
        as it is the main program.
        
*Links: [Password Manager Python](https://github.com/asankaSovis/password-manager-Python), [Password Manager C# CLI and Library](https://github.com/asankaSovis/password-manager-CSharpLibrary)*

Muragala is a password manager designed to make managing passwords easy and secure. My goal is to make it a secure and reliable password manager available in the public domain for everyone to use. Privacy and security is for everyone, this is the core value behind this project. It uses [Fernet encryption](https://github.com/thangchung/fernet-dotnet) to encrypt passwords and store them on a database. It uses two factor authentication with a password and randomly generated salt to encrypt the database. The application is built to be as simple as possible to make sure it's secure. On the other hand, this project is an experiment to see how to make a better security application.

Please note that I am still learning and this project has also been a way to expand my knowledge. Any suggestions, issues and ideas are more than welcome. On the other hand, this application is still in its alpha phase. Therefore please don't use it solely for personal usage. It might have severe bugs and issues regarding functionality and security.

## Dependencies
- [Cryptography](https://github.com/thangchung/fernet-dotnet) - This module is used for all the cryptographic work. This is an important library.

## Setup
Go to the [Releases](#releases) section. Pick a prefered release and download it. Unpack the zip file. Optionally, you can hash the password-manager-CSharpCLI.exe file and password-manager-CSharpLibrary.dll file on a service and compare it with the provided hashes to validate. Make sure that the help.json file is in the right place as well *(In the same folder)*. If you don't have Visual Studio installed, you can install [Visual Studio](https://visualstudio.microsoft.com/). Once done, install the [Fernet version 0.1.7](https://www.nuget.org/packages/Fernet/).

## Initial Setup
On opening the application for the first time, the application will create the database and preference file. This includes setting  up the salt, which requires your password. As soon as you open up for the first time, it will ask for a password. Provide a prefered password. **Make sure to remember this as it is what keeps the data protected.** Once done, you will be loaded into the application. Now you can continue to use it. Optionally, you can also backup the preference.en file created in the same directory in case of an emergency.

*NOTE: The salt or password alone cannot be used to recover the data in the database. Make sure to preserve both. Also note that the application is still in the Alpha phase.*

## Usage
The application has several useful commands built in. These allow for adding, editing, deleting and also viewing passwords to and from the database.
- ❓ help - This command will list out help information for the application.
  - `help`    [List out all the commands and their details]
  - `help <command>`  [This will list out information for the specified command]
- ⛔ exit - This command will exit the application.
- ❗ about - This command will show the about information of the application.
- 🕓 version - This command will show the version number of the application that you're using.
- ➕ add - This command will add a new profile to the database.
  - `add`    [Loads the add command with auto-generated password of default size and character set]
  - `add <size> <arguements>`    [Loads the add command with auto-generated password of size provided and character set provided]
  - `add -m`    [Loads the add command with a manual password]
      > arguement set - -u(Include uppercase) -n(Include numbers) -c(Include special characters)
      > 
      > *ex: 'add 13 -c -u' will auto-generate a password 13 characters long with only lowercase, uppercase and special characters*
- ✏️ edit - This command will edit an existing profile in the database.
  - `edit <platform> <username>`    [Loads the edit command with auto-generated password of default size and character set for the provided platform and username]
  - `edit <platform> <username> <size> <arguements>`    [Loads the edit command with auto-generated password of size provided and character set provided]
  - `edit <platform> <username> -m`    [Loads the edit command with a manual password]
      > arguement set - -u(Include uppercase) -n(Include numbers) -c(Include special characters)
      > 
      > *ex: 'edit Instagram Frank 13 -c -u' will auto-generate a password 13 characters long with only lowercase, uppercase and special characters*
- 🗑️ delete - This command will delete an existing profile in the database.
  - `delete <platform> <username>`    [Loads the delete command for the platform and username]
- 👀 show - This command will show the password of an existing profile in the database.
  - `show <platform> <username>`    [Loads the show command for the platform and username]
- 🗒️ copy - This command will copy the password of an existing profile in the database. *(NOTE: Only work in the Python edition)*
  - `copy <platform> <username>`    [Loads the copy command for the platform and username]
- 🌏 platforms - This command will list out the platforms in the database.
  - `platforms`    [Will list all the platforms listed in the database]
  - `platforms <keyword>`    [Will list all the platforms listed in the database that match the keyword]
  - `platforms <keyword> <row>`    [Will list all the platforms listed in the database that match the keyword and will list them in the specified row count]
- 👩‍🦰 username - This command will list out the usernames in the database.
  - `username`    [Will list all the usernames listed in the database]
  - `username <keyword>`    [Will list all the usernames listed in the database that match the keyword]
  - `username <keyword> <platform>`    [Will list all the usernames listed in the database that match the keyword and platform name]
  - `username -a <platform>`    [Will list all the usernames listed in the database that match only the platform name]
  - `username <keyword or -a> <platform> <rows>`    [Will list all the usernames listed in the database that match the keyword (or all if -a is provided) and platform name and will list them in the specified row count]

## Implementation
The database is a basic text file and data is stored to it in JSON format. However, each item is stored as an encrypted string, encrypted using the Fernet encryption.

![database_structure](https://user-images.githubusercontent.com/46389631/149176881-a137705f-0d34-4845-a72d-d3b02b7c2fd3.png)

`{"Platform 01" : [{"User 01" : ["Password", "Modified Date"]}, ...], ...}`

Each of these parameters are encrypted individually. Fernet encryption requires a salt and password to decrypt. Thus, we can modify the password to store different items in different formats.

![encryption_algorithm](https://user-images.githubusercontent.com/46389631/149184992-509823a7-61f7-43a7-8d5c-781a982cd795.png)

## Releases

#### Version 1.0.0 Alpha *[27/05/2022]*
Based on [Password Manager (Python)](https://github.com/asankaSovis/password-manager-Python) [version 2.1.1 Alpha](https://github.com/asankaSovis/password-manager/blob/main/Releases/Python/password_manager_v2.1.1.zip)
**NOTE: This is the initial release**

[Password Manager CLI Application](https://github.com/asankaSovis/password-manager-CSharpLibrary/blob/main/Releases/C%23%20CLI%20Program/password_manager_CLI_v1.0.0.zip)
> MD5: `0de50a1fd43d7a2164942f164bf4d880`
>
> SHA1: `028eb7f76518b3cb2d4fb853116cf688d45569e1`
>
> SHA256: `c4a4fede4fe527832ab0f7656fa4678145737ebb6dc49b43e1218df17629c3d1`

[Password Manager Library](https://github.com/asankaSovis/password-manager-CSharpLibrary/blob/main/Releases/C%23%20Library/password_manager_library_v1.0.0.zip)
> MD5: `224029b61d35b2762827eb6bc771c018`
>
> SHA1: `ead87c0a40a08443bac62b5d32526f0870d23f4e`
>
> SHA256: `4bce6b8ad1d9c547e8be8e9beb649fea9f89660a33987ffa8fd76f31ede280b3`
- Support for Encryption
- Add, Edit, Delete functionality
- Viewing and copying functionality
- Help and About sections
- Setting up a passcode
- Add way to get a new line on command line by pressing enter 

## Fixes and Features for the Next Release
- *Suggest new features*

📝 *NOTE: Throughout the application, Passcode refers to the root password set for the password manager and Password refers to the password of the application.*

`© 2022 Asanka Sovis`
