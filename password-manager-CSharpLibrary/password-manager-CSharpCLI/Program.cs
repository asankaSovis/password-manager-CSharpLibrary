using System;
using System.Text.Json; // JSON Handling
using System.Collections.Generic; // Access Dictionary and List data types
using password_manager_CSharpLibrary; // Muragala Library

/// <summary>
///                       %% 🔐 MURAGALA PASSWORD MANAGER 🔐 %%
///                                © 2022 Asanka Sovis
///
///                  This is a basic password manager made in C#.
///                                      NOTE:
///                  This is still under development and must not be
///                         used as primary password manager.
///                           *Made with ❤️ in Sri Lanka
///
///    - Author: Asanka Sovis
///    - Project start: 08/01/2022 6:00am
///    - Public release: 27/05/2022
///    - Version: 1.0.2 Alpha
///    - Current release: 07/07/2022
///    - License: MIT Open License
/// </summary>

namespace password_manager_CSharpCLI
{
    class Program
    {
        /// <summary>
        /// This is the CLI application of Muragala
        /// </summary>

        // Accessing the main Library
        public static MuragalaLibrary library = new MuragalaLibrary();

        ///////////////////////////////////////////////////////////////////
        ////// GLOBAL VARIABLES

        // Global information needed
        public static Dictionary<string, string> managerInfo = new Dictionary<string, string>(); // Application information
        public static Dictionary<string, string> strVals = new Dictionary<string, string>(); // Output strings

        // Location of core files
        public static string myLocation = AppDomain.CurrentDomain.BaseDirectory;
        // Location of user data
        public static string dataLocation = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Muragala Password Manager";

        /////////////////////////////////////////////////////////////////
        ////// MAIN

        /// <summary>
        /// Main function that run on launch
        /// </summary>

        static void Main(string[] args)
        {
            //First we initialize the application
            initialize();
            //Then we enter into the entry point to continue with the application
            entryPoint();

        }

        /////////////////////////////////////////////////////////////////
        ////// ENTRY POINT OF THE APPLICATION

        /// <summary>
        /// Entry point of the application
        /// </summary>

        static void entryPoint()
        {
            // This function runs in order to parse commands issued by the user
            // It will run until an error occur or user enter the exit command
            // Accepts none / Return null
            printf(strVals["ready"]);
            while (true)
            {
                Console.Write(">>> ");
                string[] command = Console.ReadLine().Split(" ");
                string response = command[0];
                List<string> args = new List<string>();

                for (int i = 1; i < command.Length; i++)
                {
                    args.Add(command[i]);
                }

                if (response == "exit")
                {
                    printf(strVals["exit"]);
                    library.dumpDatabase();
                    Environment.Exit(0);
                }
                else if (response == "help")
                {
                    // Help command. Opens the help section of the application
                    // >>> help
                    // >>> help <command>
                    showHelp(args);
                }
                else if (response == "about")
                {
                    // About command. Opens the about section of the application
                    // >>> about
                    showAbout();
                }
                else if (response == "version")
                {
                    // Version command. Shows the version details of the application
                    // >>> version
                    showVersion();
                }
                else if (response == "copyright")
                {
                    // Copyright command. Shows the copyright details of the application
                    // >>> copyright
                    showCopyright();
                }
                else if (response == "encrypt")
                {
                    // Encrypt command. Simply encrypts a provided string. Only
                    // used for debug purposes
                    // >>> encrypt
                    string userPasscode = passwordReader(strVals["password_string"]);
                    bool pass = checkPassword(userPasscode);
                    passcode Passcode = new passcode(pass, userPasscode);

                    if (Passcode.progress)
                    {
                        printf("Message: ");
                        string message = Console.ReadLine();
                        printf(library.encrypt(message, Passcode.password));
                    }

                }
                else if (response == "decrypt")
                {
                    // Decrypt command. Simply decrypts a provided string. Only
                    // used for debug purposes
                    // >>> decrypt
                    string userPasscode = passwordReader(strVals["password_string"]);
                    bool pass = checkPassword(userPasscode);
                    passcode Passcode = new passcode(pass, userPasscode);

                    if (Passcode.progress)
                    {
                        printf("Fernet: ");
                        string message = Console.ReadLine();
                        printf(library.decrypt(message, Passcode.password));
                    }
                }
                else if (response == "validate")
                {
                    // Validates the given password. Only used for debug purposes
                    // >>> validate
                    string userPasscode = passwordReader(strVals["password_string"]);
                    bool pass = checkPassword(userPasscode);
                    printf(pass.ToString());
                }
                else if (response == "add")
                {
                    // Add command. Adds a new password to the database
                    // >>> add           [Add password with default autogenerated password]
                    // >>> add <size> <-u, -n, -c one or more>
                    //                   [Add password with custom autogenerated password]
                    // >>> add -m        [Add manual password]
                    newProfile(args);
                }
                else if (response == "edit")
                {
                    // Edit command. Edits an existing password in the database
                    // >>> edit <platform> <username>
                    //                   [Edit password with default autogenerated password]
                    // >>> edit <platform> <username> <size> <-u, -n, -c one or more>
                    //                   [Edit password with custom autogenerated password]
                    // >>> edit <platform> <username> -m        [Edit manual password]
                    editProfile(args);
                }
                else if (response == "delete")
                {
                    // Delete command. Deletes an existing password in the database
                    // >>> delete <platform> <username>
                    deleteProfile(args);
                }
                else if (response == "show")
                {
                    // Show command. Shows an existing password in the database
                    // >>> show <platform> <username>
                    showPassword(args);
                }
                else if (response == "copy")
                {
                    // Copy command. Copies an existing password in the database
                    // >>> copy <platform> <username>
                    //copyPassword(args);
                    printf(strVals["copy_not_supported"]);
                }
                else if (response == "platforms")
                {
                    // Platforms command. Shows/search all platforms in database
                    // >>> platform
                    //                   [All the platforms]
                    // >>> platform <keyword>
                    //                   [Search platform with specific keyword]
                    // >>> platform <keyword> <rows>
                    //                   [Search platform with specific keyword and list results in specified row count]
                    showPlatforms(args);
                }
                else if (response == "username")
                {
                    // Usernames command. Shows/search all usernames in database
                    // >>> username
                    //                   [All the usernames in all platforms]
                    // >>> username <keyword>
                    //                   [Search usernames with specific keyword in all platforms]
                    // >>> username <keyword> <platform>
                    //                   [Search platform with specific keyword and platform]
                    // >>> username -a <platform>
                    //                   [All usernames in specified platform]
                    // >>> username <keyword/'-a'> <platform> <rows>
                    //                   [Search platform with specific keyword and platform and list results in specified row count]
                    showUsernames(args);
                }
                else if (response != "")
                {
                    // If unknown command is issued, show error message
                    printf(strVals["unknown_commands"]);
                }

                if (response != "")
                {
                    // Adding an empty line
                    printf("");
                }
            }
        }

        ////////////////////////////////////////////////////////////////
        ////// INITIALIZING FUNCTIONS

        /// <summary>
        /// This function is the function that runs as the application start
        /// </summary>

        static void initialize()
        {
            // Load information
            loadStrings();
            printf(strVals["loading_information"].Replace("<v>", MuragalaLibrary.About.version));

            // Load database
            printf(strVals["initializing_application"]);
            loadDatabase();

            // Load preference
            printf(strVals["initializing_preferences"]);
            loadPreference();
        }

        ////////////////////////////////////////////////////////////////
        ////// ALL USER COMMANDS

        /// <summary>
        /// Show version data
        /// </summary>

        static void showVersion()
        {
            // SHOW VERSION DATA
            // This function shows the version of the application to the user
            // Accept none / Return null
            // NOTE: This section still needs improving!
            printf(MuragalaLibrary.About.version);
        }

        /// <summary>
        /// Show about data
        /// </summary>

        static void showAbout()
        {
            // SHOW ABOUT DATA
            // This function is used to print about information to the user
            // Accepts none / Returns null
            printf(strVals["loading_information"].Replace("<v>", MuragalaLibrary.About.version));
            printf(strVals["about_string"].Replace("<v>", MuragalaLibrary.About.version).Replace("<d>", MuragalaLibrary.About.versionRelease.ToString("dd-MM-yyyy")));
        }

        /// <summary>
        /// Show copyright data
        /// </summary>

        static void showCopyright()
        {
            // SHOW COPYRIGHT DATA
            // This function is used to print copyright information to the user
            // Accepts none / Returns null
            printf(MuragalaLibrary.About.copyright);
        }

        /// <summary>
        /// Shows help
        /// </summary>
        /// <param name="args">Arguements</param>
        /// <returns>Error</returns>

        static bool showHelp(List<string> args)
        {
            // SHOW HELP DATA
            // This function is used to show the help information to the user
            // Accepts none / Returns null

            // We first load the help information and extract the data
            string arguement = "";
            string helpFile = System.IO.File.ReadAllText(myLocation + "/help.json");
            Dictionary<string, string> helpData = JsonSerializer.Deserialize<Dictionary<string, string>>(helpFile);

            // Then we check the arguements
            if (args.Count > 0)
                arguement = args[0];
            else
                printf(helpData["general"]);

            // Finally, help data is printed accordingly
            foreach (string item in helpData.Keys)
            {
                if ((item != "general") && (item.Contains("arguement")))
                    printf("● " + item + " command\n" + helpData[item]);
            }

            return false;
        }

        /// <summary>
        /// Add new profile
        /// </summary>
        /// <param name="args">Arguements</param>
        /// <returns>Error</returns>

        static bool newProfile(List<string> args)
        {
            // NEW PASSWORD FUNCTION
            // Handles the user side tasks to add a new password to the system. This will handle messages,
            // error checks, and inputs.
            // Accepts args as list of Strings / Return success as Boolean
            // NOTE: This does not handle the technical work. Refer the addPassword() for that

            // We first ask user to validate by entering the password
            string userPasscode = passwordReader(strVals["password_string"]);
            bool pass = checkPassword(userPasscode);
            passcode Passcode = new passcode(pass, userPasscode);
            bool manual = args.Contains("-m");

            if (Passcode.progress)
            {
                // If the user entered the incorrect password, we throw an error and return False
                passcode password = new passcode();
                // store the password the user want to store

                if (!manual)
                {
                    // Here we check of the user requested manual password. If not we first check if
                    // user has entered any parameters. If so we first extract the length of the password
                    // from first arguement as this is what the first arguement must be if arguements are
                    // present. The rest are also checked. If no arguements are passed, we generate a
                    // password with default arguements. We also set First value of password tuple to
                    // know later that password is auto generated
                    if (args.Count > 0)
                    {
                        try
                        {
                            Tuple<int, bool, bool, bool> passwordParams
                                = new Tuple<int, bool, bool, bool>(int.Parse(args[0]), args.Contains("-u"), args.Contains("-n"), args.Contains("-c"));
                            // (length, uppercase, numbers, specialChar)
                            password = new passcode(true, library.randomPassword(passwordParams.Item1, passwordParams.Item2, passwordParams.Item3, passwordParams.Item4));

                        }
                        catch
                        {
                            printf(strVals["invalid_new_password_args"]);
                            return false;
                        }
                    }
                    else
                        password = new passcode(true, library.randomPassword());
                }

                // Here we get platform and username of the desired account
                printf(strVals["input_platform"]);
                string platform = Console.ReadLine().Replace(' ', '_').Replace('-', '_');
                printf(strVals["input_username"]);
                string username = Console.ReadLine().Replace(' ', '_').Replace('-', '_');

                // Here we check if either of them are empty or both combined make a duplicate
                // If so, we discard everything and return back
                if ((platform == "") || (username == ""))
                {
                    printf(strVals["invalid_platform_inputs"]);
                    return false;
                }
                else if (library.getUserData(Passcode.password, platform, username).Count > 0)
                {
                    printf(strVals["duplicate_user_platform"]);
                    return false;
                }

                if (manual)
                    // We check again for manual and if so, we send to the manualPassword() function
                    // that handle the manual passwords. This is done later so that for an auto
                    // generation, few args has to be checked first and if an error exist in the
                    // arguements passed, we must show an error and exit BEFORE we ask for user input
                    // On the other hand, if manual, the password has to be handled AFTER user enters
                    // the platform and username data and these are validated
                    password = manualPassword();

                if (Passcode.progress)
                {
                    // Here we check if the new password is ready for assigning. This is put in place
                    // so that we know if auto generation failed or manual password attempt failed.
                    // This makes sure no data is entered to the database in case of an erroneous
                    // password
                    try
                    {
                        // First we show a warning asking whether the user is ok with the new password
                        printf(strVals["show_username_platform"].Replace("<p>", platform).Replace("<u>", username));
                        printf(strVals["proceed_warning_question_with_show"]);
                        string warning = Console.ReadLine();

                        // We loop to make sure incorrect inputs are not accepted. Only 'Y' (Yes), 'N' (No)
                        // and 'S' (Show password) is acceptable. Y and N can only exit from the loop
                        while (!((warning == "Y") || (warning == "N")))
                        {
                            // Entering 'S' will show the user the new password that the application is
                            // ready to store. This is not shown by default for security purposes.
                            if (warning == "S")
                                printf(strVals["show_password"].Replace("<p>", password.password));


                            printf(strVals["show_username_platform"].Replace("<p>", platform).Replace("<u>", username));
                            printf(strVals["proceed_warning_question_with_show"]);
                            warning = Console.ReadLine();
                        }

                        if (warning == "Y")
                        {
                            // Finally we check if 'Y' is given which means that the user accepts
                            // NOTE: Need improvements here. Decide if to show or not show password.
                            // Also implement function to automatically copy password to clipboard!
                            if (library.addPassword(Passcode.password, platform, username, password.password))
                                printf(strVals["password_added_successfully"]);

                        }
                        else
                            // In case the reply was otherwise, we show an aborted message and exit
                            printf(strVals["user_abort"]);

                        return true;
                    }
                    catch (Exception e)
                    {
                        // This is for error handling
                        printf(strVals["fatal_error"].Replace("<l>", "addPassword").Replace("<e>", e.Message));
                        return false;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Delete existing profile
        /// </summary>
        /// <param name="args">Arguements</param>
        /// <returns>Error</returns>

        static bool deleteProfile(List<string> args)
        {
            // DELETE PROFILE FUNCTION
            // Handles the user side tasks to delete an existing password from the system. This will handle messages,
            // error checks, and inputs.
            // Accepts args as list of Strings / Return success as Boolean
            // NOTE: This does not handle the technical work. Refer the deletePassword() for that

            if (args.Count == 2)
            {
                string platform = args[0]; string username = args[1];

                try
                {
                    // First we show a warning asking whether the user is ok with deleting the password
                    printf(strVals["delete_password_warning"]);
                    printf(strVals["show_username_platform"].Replace("<p>", platform).Replace("<u>", username));
                    printf(strVals["proceed_warning_question"]);
                    string warning = Console.ReadLine();

                    // We loop to make sure incorrect inputs are not accepted. Only 'Y' (Yes), 'N' (No)
                    // Y and N can only exit from the loop
                    while (!((warning == "Y") || (warning == "N")))
                    {
                        printf(strVals["delete_password_warning"]);
                        printf(strVals["show_username_platform"].Replace("<p>", platform).Replace("<u>", username));
                        printf(strVals["proceed_warning_question"]);
                        warning = Console.ReadLine();
                    }

                    if (warning == "Y")
                    {
                        // Finally we check if 'Y' is given which means that the user accepts
                        string userPasscode = passwordReader(strVals["password_string"]);
                        bool pass = checkPassword(userPasscode);
                        passcode Passcode = new passcode(pass, userPasscode); // Checking password

                        if (Passcode.progress)
                        {
                            if (library.deletePassword(Passcode.password, platform, username) == MuragalaLibrary.error_list.fail)
                            {
                                printf(strVals["password_deleted_successfully"]);
                                library.dumpDatabase();
                            }
                        }
                    }
                    else
                        // In case the reply was otherwise, we show an aborted message and exit
                        printf(strVals["user_abort"]);
                    return true;
                }
                catch (Exception e) // This is for error handling
                { printf(strVals["fatal_error"].Replace("<l>", "deletePassword").Replace("<e>", e.Message)); }
            }
            else
                printf(strVals["invalid_delete_password_args"]);

            return false;
        }

        /// <summary>
        /// Edit existing password
        /// </summary>
        /// <param name="args">Arguements</param>
        /// <returns>Error</returns>

        static bool editProfile(List<string> args)
        {
            // EDIT PASSWORD FUNCTION
            // Handles the user side tasks to edit an existing password to the system. This will handle messages,
            // error checks, and inputs.
            // Accepts args as list of Strings / Return success as Boolean
            // NOTE: This does not handle the technical work. Refer the editPassword() for that

            // We first ask user to validate by entering the password
            string userPasscode = passwordReader(strVals["password_string"]);
            bool pass = checkPassword(userPasscode);
            passcode Passcode = new passcode(pass, userPasscode);
            bool manual = args.Contains("-m");

            if (args.Count < 2)
            {
                printf(strVals["invalid_edit_password_args"]);
                return false;
            }

            // Here we extract platform and username of the desired account from arguements
            string platform = args[0].Replace(' ', '_').Replace('-', '_');
            string username = args[1].Replace(' ', '_').Replace('-', '_');

            if (Passcode.progress)
            {
                // If the user entered the incorrect password, we throw an error and return False
                passcode password = new passcode(false, ""); // This is NOT the users password. This is the variable to
                // store the password the user want to store

                if (!manual)
                {
                    // Here we check of the user requested manual password. If not we first check if
                    // user has entered any parameters. If so we first extract the length of the password
                    // from first arguement as this is what the first arguement must be if arguements are
                    // present. The rest are also checked. If no arguements are passed, we generate a
                    // password with default arguements. We also set First value of password tuple to
                    // know later that password is auto generated
                    if (args.Count > 2)
                    {
                        try
                        {
                            Tuple<int, bool, bool, bool> passwordParams = new Tuple<int, bool, bool, bool>(int.Parse(args[2]), args.Contains("-u"), args.Contains("-n"), args.Contains("-c"));
                            // (length, uppercase, numbers, specialChar)
                            password = new passcode(true, library.randomPassword(passwordParams.Item1, passwordParams.Item2, passwordParams.Item3, passwordParams.Item4));
                        }
                        catch (Exception)
                        {
                            printf(strVals["invalid_edit_password_args"]);
                            return false;
                        }
                    }
                    else
                        password = new passcode(true, library.randomPassword());
                }

                // Here we check if either of them are empty or both combined make a duplicate
                // If so, we discard everything and return back
                if ((platform == "") || (username == ""))
                {
                    printf(strVals["invalid_platform_inputs"]);
                    return false;
                }
                else if (library.getUserData(Passcode.password, platform, username).Count == 0)
                {
                    printf(strVals["no_user_platform"]);
                    return false;
                }

                if (manual)
                    // We check again for manual and if so, we send to the manualPassword() function
                    // that handle the manual passwords. This is done later so that for an auto
                    // generation, few args has to be checked first and if an error exist in the
                    // arguements passed, we must show an error and exit BEFORE we ask for user input
                    // On the other hand, if manual, the password has to be handled AFTER user enters
                    // the platform and username data and these are validated
                    password = manualPassword();

                if (password.progress)
                {
                    // Here we check if the new password is ready for assigning. This is put in place
                    // so that we know if auto generation failed or manual password attempt failed.
                    // This makes sure no data is entered to the database in case of an erroneous
                    // password
                    try
                    {
                        // First we show a warning asking whether the user is ok with the new password
                        printf(strVals["edit_password_warning"]);
                        printf(strVals["show_username_platform"].Replace("<p>", platform).Replace("<u>", username));
                        printf(strVals["proceed_warning_question_with_show"]);
                        string warning = Console.ReadLine();

                        // We loop to make sure incorrect inputs are not accepted. Only 'Y' (Yes), 'N' (No)
                        // and 'S' (Show password) is acceptable. Y and N can only exit from the loop
                        while (!((warning == "Y") || (warning == "N")))
                        {
                            // Entering 'S' will show the user the new password that the application is
                            // ready to store. This is not shown by default for security purposes.
                            if (warning == "S")
                            {
                                printf(strVals["show_password"].Replace("<p>", password.password));

                                printf(strVals["edit_password_warning"]);
                                printf(strVals["show_username_platform"].Replace("<p>", platform).Replace("<u>", username));
                                printf(strVals["proceed_warning_question_with_show"]);
                                warning = Console.ReadLine();
                            }
                        }

                        if (warning == "Y")
                        {
                            // Finally we check if 'Y' is given which means that the user accepts
                            // NOTE: Need improvements here. Decide if to show or not show password.
                            //       Also implement function to automatically copy password to clipboard!
                            if (library.editPassword(Passcode.password, platform, username, password.password) == MuragalaLibrary.error_list.success)
                                printf(strVals["password_changed_successfully"]);
                        }
                        else
                        {
                            // In case the reply was otherwise, we show an aborted message and exit
                            printf(strVals["user_abort"]);
                        }

                        return true;
                    }
                    catch (Exception e)
                    {
                        // This is for error handling
                        printf(strVals["fatal_error"].Replace("<l>", "editPassword").Replace("<e>", e.Message));

                        return false;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Show password
        /// </summary>
        /// <param name="args">Arguements</param>
        /// <returns>Error</returns>

        static bool showPassword(List<string> args)
        {
            // SHOW PASSWORD FUNCTION
            // Allows user to see the password quickly from username and password
            // Accepts args as list of Strings / Return success as Boolean

            // We first ask user to validate by entering the password
            string userPasscode = passwordReader(strVals["password_string"]);
            bool pass = checkPassword(userPasscode);
            passcode Passcode = new passcode(pass, userPasscode);

            if (args.Count < 2)
            {
                printf(strVals["invalid_view_password_args"]);
                return false;
            }

            // Here we extract platform and username of the desired account from arguements
            string platform = args[0];
            string username = args[1];

            if (Passcode.progress)
            {
                // If the user entered the incorrect password, we throw an error and return False

                List<string> userInformation = library.getUserInformation(Passcode.password, platform, username);

                if (userInformation.Count > 0)
                {
                    // We get the password info from getUserInformation() function for the
                    // provided username and platform combination and we copy the first
                    // returned value if we have more that zero data and show an error if not

                    printf(strVals["show_username_platform"].Replace("<p>", platform).Replace("<u>", username));
                    printf(strVals["show_password_with_info"].Replace("<p>", userInformation[0]).Replace("<d>", userInformation[1]));
                }
                else
                    printf(strVals["user_or_platform_not_exist"]);
            }

            return false;
        }

        /// <summary>
        /// Show all platforms
        /// </summary>
        /// <param name="args">Arguements</param>
        /// <returns>Error</returns>

        static bool showPlatforms(List<string> args)
        {
            // SHOW PLATFORMS FUNCTION
            // This allows users to list all the platforms registered in the
            // database. They can also include a keyword as arguement to
            // search for a matching phrase

            // We first ask user to validate by entering the password
            string userPasscode = passwordReader(strVals["password_string"]);
            bool pass = checkPassword(userPasscode);
            passcode Passcode = new passcode(pass, userPasscode);

            if (Passcode.progress)
            {
                // Number of rows to display   // Keyword user included in arguements
                int rows = 5; string keyword = "";

                if (args.Count > 1)
                {
                    // If the arguement count is more than one, the second arguement must
                    // be an integer that define how many rows to show. If it's wrong we
                    // assume default value
                    try { rows = int.Parse(args[1]); }
                    catch { printf(strVals["invalid_search_args"]); }
                }

                if (args.Count > 0)
                {
                    // If the arguement count is more than 0, then we have a keyword. So
                    // we extract the keyword and show a message. Otherwise we just get
                    // all entries
                    keyword = args[0];
                    printf(strVals["show_keyword_platforms"].Replace("<k>", keyword));
                }
                else
                    printf(strVals["show_all_platforms"]);

                List<string> platforms = library.getPlatformNames(Passcode.password, keyword);
                string output = "";

                // Finally we list all platforms. This is done in rows for easier visibility
                // separated by a |
                if (platforms.Count == 0)
                    printf("     ~~EMPTY~~");
                else
                {
                    for (int i = 0; i < platforms.Count; i++)
                    {
                        output += platforms[i];

                        if ((i % rows) == rows - 1)
                            output += " \n";
                        else
                            output += " | ";
                    }

                    output = output.Substring(0, output.Length - 2);
                }
                printf(output);

                return true;
            }

            return false;
        }

        /// <summary>
        /// Show usernames function
        /// </summary>
        /// <param name="args">Arguement</param>
        /// <returns>Error</returns>

        static bool showUsernames(List<string> args)
        {
            // SHOW USERNAMES FUNCTION
            // This allows users to list all the platforms and users registered in the
            // database. They can also include a keyword as arguement to search for
            // a matching phrase

            // We first ask user to validate by entering the password
            string userPasscode = passwordReader(strVals["password_string"]);
            bool pass = checkPassword(userPasscode);
            passcode Passcode = new passcode(pass, userPasscode);

            if (Passcode.progress)
            {
                // Number of rows to display   // Keyword user included in arguements
                // Keyword user included in arguements for platform
                int rows = 5; string keyword = ""; string platform = "";

                if (args.Count > 2)
                {
                    // If the arguement count is more than two, the thirt arguement must
                    // be an integer that define how many rows to show. If it's wrong we
                    // assume default value
                    try { rows = int.Parse(args[2]); }
                    catch (Exception) { printf(strVals["invalid_search_args"]); }
                }

                if (args.Count > 1)
                {
                    // If the arguement count is more than 1, then we have two keyword. So
                    // we extract the keywords and show a message. Otherwise we just get
                    // all entries
                    keyword = args[0]; platform = args[1];

                    if (keyword == "-a")
                    {
                        keyword = "";
                        printf(strVals["show_all_usernames_in_keyword_platforms"].Replace("<p>", platform));
                    }
                    else
                        printf(strVals["show_keyword_usernames_in_keyword_platforms"].Replace("<p>", platform).Replace("<k>", keyword));
                }
                else if (args.Count == 1)
                {
                    // If the arguement count is exactly 1, then we have a keyword only. So
                    // we extract the keyword and show a message. Otherwise we just get
                    // all entries
                    keyword = args[0];

                    printf(strVals["show_keyword_usernames"].Replace("<k>", keyword));
                }
                else
                    // Otherwise we show all usernames
                    printf(strVals["show_all_usernames"]);

                List<Tuple<string, List<string>>> returnData = library.searchUsernames(Passcode.password, keyword, platform);

                // Finally we list all platforms and their usernames. This is done in rows for easier visibility
                // separated by a |
                if (returnData.Count == 0)
                    printf("     ~~EMPTY~~");
                else
                {
                    foreach (Tuple<string, List<string>> item in returnData)
                    {
                        string output = "";
                        printf(item.Item1 + ":");
                        if (item.Item2.Count == 0)
                            output = "      ~~EMPTY~~";
                        else
                        {
                            for (int i = 0; i < item.Item2.Count; i++)
                            {
                                if ((i % rows) == 0)
                                    output += "     ";

                                output += item.Item2[i];

                                if ((i % rows) == rows - 1)
                                    output += " \n";
                                else
                                    output += " | ";
                            }

                            output = output.Substring(0, output.Length - 2);
                        }
                        printf(output);
                    }
                }
                return true;
            }
            return false;
        }

        ////////////////////////////////////////////////////////////////
        ////// LOADING COMPONENTS

        /// <summary>
        /// Loads the strings needed for outputs and errors
        /// Loads the strings.json file and stores them on the strVals dictionary variable
        /// after deserializing the JSON.
        /// </summary>

        static void loadStrings()
        {
            string json = System.IO.File.ReadAllText(myLocation + "\\strings.json");
            strVals = JsonSerializer.Deserialize<Dictionary<string, string>>(json);
        }

        /// <summary>
        /// Loads the database to the library
        /// </summary>

        static void loadDatabase()
        {
            int error = library.loadDatabase(dataLocation + "\\database.en");

            if (error == MuragalaLibrary.error_list.database_created)
                printf(strVals["no_database_found"]);
            else if (error == MuragalaLibrary.error_list.database_load_failed)
            {
                printf(strVals["fatal_error"].Replace("<l>", "Database loading").Replace("<e>", "Could not load database."));
                Environment.Exit(-1);
            }
        }

        /// <summary>
        /// Loads the preferences to the library
        /// </summary>

        static void loadPreference()
        {
            int error = library.loadPreference(dataLocation + "\\preferences.en");

            if (error == MuragalaLibrary.error_list.preference_load_failed)
            {
                printf(strVals["create_new_password"]);

                passcode myPasscode = new passcode();

                while (!myPasscode.progress)
                {
                    myPasscode = manualPassword();

                }

                error = library.createPreference(dataLocation + "\\preferences.en", myPasscode.password, myPasscode.password);
                if (error == MuragalaLibrary.error_list.success)
                    printf(strVals["create_new_password_success"]);
            }

            if (error == MuragalaLibrary.error_list.fail)
            {
                printf(strVals["fatal_error"].Replace("<l>", "Preference loading").Replace("<e>", "Could not load preference."));
                Environment.Exit(-1);
            }
        }

        ///////////////////////////////////////////////////////////////
        /// PASSWORD RELATED FUNCTIONS

        /// <summary>
        /// Lets the user enter a manual password
        /// Prompts the user to enter a password and sent it back after validation
        /// </summary>
        /// <returns>The password entered as a passcode data type</returns>

        static passcode manualPassword()
        {
            // Request password reader for a password
            string password1 = passwordReader(strVals["input_manual_password"]);

            // If the password we got is empty, we discard it and show error and return
            // password failed state
            if (password1 == "")
            {
                Console.Write(strVals["empty_password"]);
                return new passcode();
            }

            // Request password again to validate
            string password2 = passwordReader(strVals["reenter_manual_password"]);

            // If both don't match we do the same thing
            if (password1 != password2)
            {
                Console.Write(strVals["password_mismatch"]);
                return new passcode();
            }

            // If all is successful, we return success state along with the password
            return new passcode(true, password1);
        }

        /// <summary>
        /// Checks if the password entered by the user is valid.
        /// </summary>
        /// <param name="password">Password the user entered</param>
        /// <returns>passcode class of passed or failed as bool and password as string</returns>

        static bool checkPassword(string password)
        {
            if (!library.checkPassword(password))
            {
                printf(strVals["incorrect_password"]);
                return false;
            }
            else
                return true;
        }

        /// <summary>
        /// This function allows for users to enter passwords in disguise.
        /// </summary>
        /// <param name="message">message to be prompted as string</param>
        /// <returns>The password that the user entered as string</returns>
        
        static string passwordReader(string message)
        {
            // First we print the prompt
            printf(message);

            // Setting up dummy variable and loading ConsoleKeyInfo library
            string password = "";
            ConsoleKeyInfo keyInfo;

            // Here onwards, the data accepting is done
            do
            {
                keyInfo = Console.ReadKey(true);
                // Skip if Backspace or Enter is Pressed
                if (keyInfo.Key != ConsoleKey.Backspace && keyInfo.Key != ConsoleKey.Enter)
                {
                    password += keyInfo.KeyChar;
                    Console.Write("*");
                }
                else
                {
                    if (keyInfo.Key == ConsoleKey.Backspace && password.Length > 0)
                    {
                        // Remove last charcter if Backspace is Pressed
                        password = password.Substring(0, (password.Length - 1));
                        Console.Write("\b \b");
                    }
                }
            }
            // Stops Getting Password Once Enter is Pressed
            while (keyInfo.Key != ConsoleKey.Enter);
            printf("\n");

            // Finally returns the password
            return password;
        }

        ///////////////////////////////////////////////////////////////
        /// PRINT FUNCTIONS

        /// <summary>
        /// This function is used to output strings to the console
        /// </summary>
        /// <param name="stringValue">Accepts the prompt to me output on the console as string</param>
        
        static void printf(string stringValue)
        {
            Console.WriteLine(stringValue);
        }

    }

    /// <summary>
    /// This class is a data type to hold the user password
    /// It informs if the password is an accepted password and the actual password itself
    /// </summary>

    class passcode
    {
        public bool progress = false;
        public string password = "";

        public passcode(bool _progress = false, string _password = "")
        {
            // Initialization
            progress = _progress;
            password = _password;
        }
    }

}
