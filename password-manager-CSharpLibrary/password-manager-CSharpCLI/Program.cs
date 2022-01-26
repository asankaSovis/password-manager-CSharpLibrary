using System;
using System.Text.Json; // JSON Handling
using System.Text.Json.Serialization;
using System.Collections.Generic; // Access Dictionary and List data types
using password_manager_CSharpLibrary; // Muragala Library

/// <summary>
///                            %% 🔐 PASSWORD MANAGER 🔐 %%
///                                © 2022 Asanka Sovis
///
///                  This is a basic password manager made in python.
///                                      NOTE:
///                  This is still under development and must not be
///                         used as primary password manager.
///                           *Made with ❤️ in Sri Lanka
///
///    - Author: Asanka Sovis
///    - Project start: 08/01/2022 6:00am
///    - Public release: 11/01/2022
///    - Version: 1.0.0 Alpha
///    - Current release: 12/01/2022
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
        /// GLOBAL VARIABLES

        // Global information needed
        public static Dictionary<string, string> managerInfo = new Dictionary<string, string>(); // Application information
        public static Dictionary<string, string> strVals = new Dictionary<string, string>(); // Output strings

        // Location of core files
        public static string myLocation = AppDomain.CurrentDomain.BaseDirectory;

        /////////////////////////////////////////////////////////////////
        /// ENTRY POINT OF THE APPLICATION

        /// <summary>
        /// Entry point of the application
        /// </summary>

        static void Main(string[] args)
        {
            initialize();
            printf(library.decrypt("gAAAAABh4amlNRF6igdJGMsUSubwXm-BbLA8wvEuCrAzsYAu_URNhCXPe3IeXBothcihOKJpouA8u5G9HmsV4afnce3gUoukQhGHMqU3uy2T1Cn8PUdPvqE=", "Asanka123Artist"));
            printf("Breakpoint");

        }

        ////////////////////////////////////////////////////////////////
        /// INITIALIZING FUNCTIONS

        /// <summary>
        /// This function is the function that runs as the application start
        /// </summary>

        static void initialize()
        {
            loadStrings();
            printf(strVals["loading_information"]);
            printf(strVals["initializing_application"]);
            
            loadDatabase();
            printf(strVals["initializing_preferences"]);
            loadPreference();
        }

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
        /// Loads the user database
        /// Loads the database.en file file and stores them on a temporary dictionary variable
        /// after deserializing the JSON, to be sent to the library
        /// </summary>
        static void loadDatabase()
        {
            // We first check if the file exist, if not we make it after showing an error
            if (!System.IO.File.Exists(myLocation + "database.en"))
            {
                printf(strVals["no_database_found"]);
                System.IO.File.Create(myLocation + "database.en");
            }

            // Defining a temporary dictionary variable to store loaded data
            Dictionary<string, List<Dictionary<string, List<string>>>> database = new Dictionary<string, List<Dictionary<string, List<string>>>>();

            // Loading data from the file
            string json = System.IO.File.ReadAllText(myLocation + "\\database.en");

            // If the loaded string is not empty, then we try to extract the data. If we run into an
            // error, we throw an error and exit.
            if (json != "")
            {
                try
                {
                    database = JsonSerializer.Deserialize<Dictionary<string, List<Dictionary<string, List<string>>>>>(json);
                }
                catch (Exception err)
                { printf(strVals["fatal_error"].Replace("<l>", "Database loading").Replace("<e>", err.Message)); Environment.Exit(0); }
                
            }
            
            // Once done, all data is sent to the library
            library.loadDatabase(database);
        }

        /// <summary>
        /// Loads the preference data of the user
        /// Loads the preference.en file and stores them on a temporary dictionary variable
        /// after deserializing the JSON, to be sent to the library
        /// </summary>
        static void loadPreference()
        {
            // We first check if the file exist, if not we make it
            if (!System.IO.File.Exists(myLocation + "preferences.en"))
            {
                //printf(strVals["no_database_found"]);
                System.IO.File.Create(myLocation + "preferences.en").Close();
            }

            // Temporary dictionary variable to store the information
            Dictionary<string, string> preference = new Dictionary<string, string>();

            // Loading the data from the file
            string json = System.IO.File.ReadAllText(myLocation + "\\preferences.en");

            // If the loaded data is empty, we request the user to make a new password and
            // if not, we load the preference data from the file.
            if (json == "")
            {
                // Setting up a new password
                printf(strVals["create_new_password"]);
                passcode password = manualPassword();

                if (password.progress)
                {
                    // If successful, we add an entry to the preference dictionary and then write the data
                    // NOTE: REMEMBER TO ADD FERNET ENCRYPTION
                    string salt = library.randomSalt(); // Get a random salt
                    preference.Add("salt", salt);
                    preference.Add("key", library.getKey(password.password, salt)); // Store new key in the database
                    System.IO.File.WriteAllText(myLocation + "\\preferences.en", JsonSerializer.Serialize<Dictionary<string, string>>(preference));

                    printf(strVals["create_new_password_success"]);
                } else
                {
                    // If failed, we exit the application
                    Environment.Exit(0);
                }

            } else
            {
                // If file exist, we attempt to load the information. On error we exit
                try
                {
                    preference = JsonSerializer.Deserialize<Dictionary<string, string>>(json);
                }
                catch (Exception err)
                { printf(strVals["fatal_error"].Replace("<l>", "Preferences loading").Replace("<e>", err.Message)); Environment.Exit(0); }

            }

            // Send the loaded data to the library
            library.loadPreference(preference);
        }

        ////////////////////////////////////////////////////////////////
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
