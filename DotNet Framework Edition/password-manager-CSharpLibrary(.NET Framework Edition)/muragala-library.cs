using System;
using Fernet; // Fernet encryption
using System.Collections.Generic; // Access Dictionary and List data types
using System.Security.Cryptography; // Cryptographic functions
using System.Text.Json; // JSON Handling

/// <summary>
///                       %% 🔐 MURAGALA PASSWORD MANAGER 🔐 %%
///                                © 2022 Asanka Sovis
///
///                  This is a basic password manager made in C#.
///                                      NOTE:
///                  This is still under development and must not be
///                         used as primary password manager.
///                           *Made with ❤️ in Sri Lanka
///    Blog: https://asanka.hashnode.dev/muragala-password-manager-04
///
///    - Author: Asanka Sovis
///    - Project start: 08/01/2022 6:00am
///    - Public release: 27/05/2022
///    - Version: 1.0.1 Alpha
///    - Current release: 06/07/2022
///    - License: MIT Open License
/// </summary>

namespace password_manager_CSharpLibrary
{
    public class MuragalaLibrary
    {
        /// <summary>
        /// This is the C# library of Muragala
        /// NOTE: All byte arrays are handled as Base64 strings
        /// </summary>

        ///////////////////////////////////////////////////////////////////
        /// GLOBAL VARIABLES

        // Global information needed
        private Dictionary<string, string> preference = new Dictionary<string, string>(); // Preference data
        private Dictionary<string, List<Dictionary<string, List<string>>>> database = new Dictionary<string, List<Dictionary<string, List<string>>>>(); // Password Database

        private string databaseLocation = Environment.CurrentDirectory + "/database.en";

        // Error returns
        public struct return_error
        {
            public int success;
            public int non_existent_username;
            public int non_existent_platform;
            public int same_password_to_edit;
            public int database_created;
            public int fail;
            public int database_load_failed;
            public int preference_load_failed;
            public int passcode_mismatch;

            public return_error(int success, int non_existent_username, int non_existent_platform, int same_password_to_edit, int database_created, int fail, int database_load_failed, int preference_load_failed, int passcode_mismatch)
            {
                this.success = success;
                this.non_existent_username = non_existent_username;
                this.non_existent_platform = non_existent_platform;
                this.same_password_to_edit = same_password_to_edit;
                this.database_created = database_created;
                this.fail = fail;
                this.database_load_failed = database_load_failed;
                this.preference_load_failed = preference_load_failed;
                this.passcode_mismatch = passcode_mismatch;
            }
        }
        public static return_error error_list = new return_error(0, 1, 2, 3, 4, 5, 6, 7, 8);

        // Library Info
        public struct libraryInfo
        {
            public string version;
            public string copyright;
            public DateTime startDate;
            public DateTime publicRelease;
            public DateTime versionRelease;

            public libraryInfo(string version, string copyright, string startDate, string publicRelease, string versionRelease)
            {
                this.version = version;
                this.copyright = copyright;
                this.startDate = DateTime.Parse(startDate);
                this.publicRelease = DateTime.Parse(publicRelease);
                this.versionRelease = DateTime.Parse(versionRelease);
            }
        }
        public static libraryInfo About = new libraryInfo(
            "1.0.1 Alpha",               // Version
            "(C) 2022 Asanka Sovis",     // Copyright
            "08/01/2022",                // Project Start Date
            "27/05/2022",                // Project Release Date
            "06/07/2022"                 // Version Release Date
            );

        //////////////////////////////////////////////////////////////////
        ////// PUBLIC FUNCTIONS

        /// <summary>
        /// This function loads a new database to the class
        /// </summary>
        /// <param name="_database">The database as a dictionary</param>

        public void loadDatabase(Dictionary<string, List<Dictionary<string, List<string>>>> _database)
        {
            database = _database;
        }

        /// <summary>
        /// Loads the preferences of the user
        /// </summary>
        /// <param name="_preference">Preference data as dictionary</param>

        public void loadPreference(Dictionary<string, string> _preference)
        {
            preference = _preference;
        }

        /// <summary>
        /// Loads the user database from location
        /// Loads the database.en file file and stores them on a temporary dictionary variable
        /// after deserializing the JSON, to be sent to the library
        /// </summary>

        public int loadDatabase(string location)
        {
            databaseLocation = location;

            // We first check if the file exist, if not we make it after showing an error
            if (!System.IO.File.Exists(databaseLocation))
            {
                System.IO.File.Create(databaseLocation).Close();
                return error_list.database_created;
            }

            // Defining a temporary dictionary variable to store loaded data
            Dictionary<string, List<Dictionary<string, List<string>>>> tempDatabase = new Dictionary<string, List<Dictionary<string, List<string>>>>();

            // Loading data from the file
            string json = System.IO.File.ReadAllText(databaseLocation);

            // If the loaded string is not empty, then we try to extract the data. If we run into an
            // error, we throw an error and exit.
            if (json != "")
            {
                try
                {
                    tempDatabase = JsonSerializer.Deserialize<Dictionary<string, List<Dictionary<string, List<string>>>>>(json);
                }
                catch (Exception)
                { return error_list.fail; }

            }

            // Once done, all data is sent to the library
            loadDatabase(tempDatabase); return error_list.success;
        }

        /// <summary>
        /// Loads the preference data of the user from a file
        /// Loads the preference.en file and stores them on a temporary dictionary variable
        /// after deserializing the JSON, to be sent to the library
        /// </summary>

        public int loadPreference(string location)
        {
            // Temporary dictionary variable to store the information
            Dictionary<string, string> tempPreference = new Dictionary<string, string>();

            try
            {
                // Loading the data from the file
                string json = System.IO.File.ReadAllText(location);
                // If file exist, we attempt to load the information. On error we exit
                tempPreference = JsonSerializer.Deserialize<Dictionary<string, string>>(json);
            }
            catch (Exception)
            { return error_list.preference_load_failed; }

            // Send the loaded data to the library
            loadPreference(tempPreference); return error_list.success;
        }

        /// <summary>
        /// Creates the preference file
        /// </summary>
        /// <param name="location">Location of the file</param>
        /// <param name="passcode">Passcode user entered</param>
        /// <param name="reenterPasscode">Reentered passcode</param>
        /// <returns></returns>

        public int createPreference(string location, string passcode, string reenterPasscode)
        {
            // Temporary dictionary variable to store the information
            Dictionary<string, string> tempPreference = new Dictionary<string, string>();

            if (passcode == reenterPasscode)
            {
                try
                {
                    // We first check if the file exist, if not we make it
                    if (!System.IO.File.Exists(location))
                    {
                        System.IO.File.Create(location).Close();
                    }

                    // If successful, we add an entry to the preference dictionary and then write the data
                    // NOTE: REMEMBER TO ADD FERNET ENCRYPTION
                    string salt = randomSalt(); // Get a random salt
                    tempPreference.Add("salt", salt);
                    tempPreference.Add("hash", getHash(passcode, salt)); // Store new key in the database
                    System.IO.File.WriteAllText(location, JsonSerializer.Serialize<Dictionary<string, string>>(tempPreference));

                    loadPreference(tempPreference);

                }
                catch (Exception) { return error_list.fail; }
            }
            else
            {
                // If failed
                return error_list.passcode_mismatch;
            }

            return error_list.success;
        }

        /// <summary>
        /// Dump data to file
        /// </summary>
        /// <returns>Error</returns>

        public int dumpDatabase()
        {
            // All updates to the database is dumped back to the physical file
            // Accepts none / Return null
            try
            {
                System.IO.StreamWriter databaseFile = new System.IO.StreamWriter(databaseLocation, false);
                databaseFile.Write(JsonSerializer.Serialize(database));
                databaseFile.Flush();
                databaseFile.Dispose();

                return error_list.success;
            }
            catch (Exception) { return error_list.fail; }
        }

        //////////////////////////////////////////////
        ////// CRYPTOGRAPHIC FUNCTIONS

        /// <summary>
        /// Encrypts a given message and returns the fernet encryption of that message
        /// </summary>
        /// <param name="message">Message as string</param>
        /// <param name="password">Password as string</param>
        /// <returns>Returns the fernet of the message as string</returns>

        public string encrypt(string message, string password)
        {
            return SimpleFernet.Encrypt(getKey(password, preference["salt"]).UrlSafe64Decode(), System.Text.Encoding.UTF8.GetBytes(message));
        }

        /// <summary>
        /// Decrypts a fernet string to its original message
        /// </summary>
        /// <param name="message">Fernet string as string</param>
        /// <param name="password">Password as string</param>
        /// <returns>Original message as string</returns>

        public string decrypt(string message, string password)
        {
            var decoded64 = SimpleFernet.Decrypt(getKey(password, preference["salt"]).UrlSafe64Decode(), message, out var timestamp);
            return decoded64.UrlSafe64Encode().FromBase64String();
        }

        /// <summary>
        /// Generates a key from a given password and salt pair
        /// NOTE: Salt must be in Base64 string
        /// </summary>
        /// <param name="password">Password as string</param>
        /// <param name="salt">Salt as base64 string</param>
        /// <returns>Returns the key as string</returns>

        public string getKey(string password, string salt)
        {
            Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(password, System.Text.UTF8Encoding.UTF8.GetBytes(salt), 390000, HashAlgorithmName.SHA256);
            return pbkdf2.GetBytes(32).UrlSafe64Encode();
        }

        /// <summary>
        /// Generates a hash from password using a key as the salt
        /// </summary>
        /// <param name="password">Password of the user</param>
        /// <param name="salt">Salt saved</param>
        /// <returns></returns>

        public string getHash(string password, string salt)
        {
            Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(password, System.Text.UTF8Encoding.UTF8.GetBytes(getKey(password, salt)), 390000, HashAlgorithmName.SHA256);
            return pbkdf2.GetBytes(32).UrlSafe64Encode();
        }

        /// <summary>
        /// Generates a random salt for use
        /// </summary>
        /// <returns>THe random salt as base64 string</returns>

        public string randomSalt()
        {
            RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider();
            byte[] salt = new byte[16];
            provider.GetBytes(salt);

            return salt.UrlSafe64Encode();
        }

        /// <summary>
        /// Decrypt data
        /// </summary>
        /// <param name="data">User data to be decrypted</param>
        /// <param name="password">Password entered by the user</param>
        /// <param name="platform">Platform needed</param>
        /// <param name="username">Username under the platform</param>
        /// <returns>List of userdata</returns>

        public List<string> decryptItem(List<string> data, string password, string platform, string username)
        {
            // This function decrypts the data that is given to it using the password, platform and username
            // NOTE: Data is in tuple form. This is so that encrypted data retrieved from getUserData() function
            // can be quickly decrypted and retrieved with ease
            // Accets data as a tuple of Strings (Lists is also acceptable but is not what is intended),
            // password as String, platform as String, username as String / Returns the same
            // data in the same structure but decrypted and as a tuple

            List<string> decryptedList = new List<string>();

            foreach (string item in data)
            {
                // We perform the decryption for each item in the tuple (or list) and add it to a temporary
                // list
                decryptedList.Add(decrypt(item, password + platform + username));
            }

            return decryptedList;
        }

        /////////////////////////////////////////////////////////////
        ////// APPLICATION SIDE FUNCTIONS

        /// <summary>
        /// Checks the password against the hash of the password saved before
        /// Return true if the hashes match
        /// </summary>
        /// <param name="password"></param>
        /// <returns>Whether the password is correct</returns>

        public bool checkPassword(string password)
        {
            return (getHash(password, preference["salt"]) == preference["hash"]);
        }

        /// <summary>
        /// Get the encrypted platform names from database
        /// This function scans the database and retrievs the platforms from it
        /// </summary>
        /// <param name="password">Password entered by the user</param>
        /// <param name="platform">Platform needed</param>
        /// <returns></returns>

        public List<string> getEncPlatformNames(string password, string platform)
        {
            // NOTE: These data are encrypted and thus the returned data is ENCRYPTED
            // NOTE: This only returns the list of encrypted platform names as String
            // NOTE: In theory, the platform name must appear only once in the database but still we do
            // a looping check for added safety and retrieve every appearance from a loop
            // NOTE: This function can be used to validate if a platform already exists in the database

            List<string> platformData = new List<string>();

            foreach (string item in database.Keys)
            {
                // We first take out all the keys in the database[Platforms] and for each one, we decrypt
                // it using the password and compare it with the platform name sent for reference.
                // NOTE: The reason why we have to decrypt each item instead of encrypting the provided
                // platform once and comparing it with the existing encrypted items, is because with
                // Fernet encrypting, the resulting encrypting is different for the same item at different
                // times
                if (decrypt(item, password) == platform)
                {
                    platformData.Add(item);
                }
            }

            return platformData;
        }

        /// <summary>
        /// Get the data associated with a platform from database
        /// </summary>
        /// <param name="password">Password entered by the user</param>
        /// <param name="platform">Platform user needs</param>
        /// <returns></returns>

        public List<Dictionary<string, List<string>>> getPlatform(string password, string platform)
        {
            // This function scans the database and retrievs the platforms from it
            // Unlike the getEncPlatformNames() function, this returns a list of dictionaries that
            // Contain the data available under this platform. This include the usernames as keys
            // and the data of relevant user name as values of each key. Refer to the database structure
            // for more details. Of course this data is also encrypted
            // NOTE: In theory, the platform name must appear only once in the database but still we do
            // a looping check for added safety and retrieve every appearance from a loop
            // NOTE: This function can be used to validate if a platform already exists in the database
            // but still the getEncPlatformNames() is recommended for efficiency
            // Accepts password as String, platform as String / Returns the retrieved data as list

            List<Dictionary<string, List<string>>> platformData = new List<Dictionary<string, List<string>>>();

            foreach (string item in database.Keys)
            {
                // We first take out all the keys in the database[Platforms] and for each one, we decrypt
                // it using the password and compare it with the platform name sent for reference.
                // NOTE: The reason why we have to decrypt each item instead of encrypting the provided
                // platform once and comparing it with the existing encrypted items, is because with
                // Fernet encrypting, the resulting encrypting is different for the same item at different
                // times
                if (decrypt(item, password) == platform)
                {
                    platformData.AddRange(database[item]);
                }
            }

            return platformData;
        }

        /// <summary>
        /// Extract the user data from platform
        /// </summary>
        /// <param name="password"></param>
        /// <param name="platform"></param>
        /// <param name="username"></param>
        /// <returns></returns>

        public List<string> getUserData(string password, string platform, string username)
        {
            // This function extracts the data (password, time, etc.) from the database for a given username
            // and platform. It uses the getPlatform() function to extract the data for a particular platform
            // and then use this data to extract the information for that particular username.
            // NOTE: The extracted data is still encrypted and in order to use it, it has to be decrypted
            // Accepts password as String, platform as String, username as String / Returns encrypted data
            // as list of tuples of user information
            //       [(encPassword for 1, encTime for 1), (encPassword for 2, encTime for 2), ...]

            List<Dictionary<string, List<string>>> platformData = getPlatform(password, platform);
            List<string> userData = new List<string>();

            foreach (Dictionary<string, List<string>> dict in platformData)
            {
                // For each username dictionary returned, we iterate through them and for each username
                // we decrypt it and compare it with the provided username. If they match, the values
                // (tuples with information) is added to a list which is returned back
                // NOTE: Since JSON Stores tuples as lists we also convert the list to tuple. This
                // is not a must but is efficient as information stored is in a fixed structure
                foreach (string item in dict.Keys)
                {
                    if (decrypt(item, password + platform) == username)
                    {
                        userData.AddRange(dict[item]);
                    }
                }
            }

            return userData;
        }

        /// <summary>
        /// Get user information
        /// </summary>
        /// <param name="password">Password entered by the user</param>
        /// <param name="platform">Platform selected by the user</param>
        /// <param name="username">Username selected by the user</param>
        /// <returns>List of strings of data</returns>

        public List<string> getUserInformation(string password, string platform, string username)
        {
            // This function decrypts the data from getUserData() function using the decryptItem()
            // function. This function is useful to directly get the decrypted information of the user
            // without any steps, as this handles the in between step of decrypting each item in
            // the data
            // Accepts password as String, platform as String, username as String / Returns decrypted
            // user information as list of tuples of user information
            //       [(password for 1, time for 1), (password for 2, time for 2), ...]

            List<string> userData = getUserData(password, platform, username);
            List<string> decUserData = new List<string>();

            return decryptItem(userData, password, platform, username);
        }

        /// <summary>
        /// Extract usernames from platform
        /// </summary>
        /// <param name="password">Password entered by the user</param>
        /// <param name="platform">Platform needed by the user</param>
        /// <param name="username">Username user wants to search</param>
        /// <returns>List of platforms that contain the username</returns>

        public List<string> getUsernamesInPlatform(string password, string platform, string username)
        {
            // EXTRACT USERNAMES FROM PLATFORM
            // This function extracts the usernames from the database for a given username
            // and platform. It uses the getPlatform() function to extract the data for a particular platform
            // and then use this data to extract the name for that particular username.
            // Accepts password as String, platform as String, username as String / Returns usernames
            // as list of Strings

            List<Dictionary<string, List<string>>> platformData = getPlatform(password, platform);
            List<string> userData = new List<string>();

            foreach (Dictionary<string, List<string>> dict in platformData)
            {
                // For each username dictionary returned, we iterate through them and for each username
                // we decrypt it and compare it with the provided username. If they match, the decrypted
                // username is added to a list which is returned back

                foreach (string item in dict.Keys)
                {
                    string extracts = decrypt(item, password + platform);

                    if (extracts.Contains(username))
                        userData.Add(extracts);
                }
            }

            return userData;
        }

        /// <summary>
        /// Adds new information to the database
        /// </summary>
        /// <param name="passcode">The password user entered for authentication</param>
        /// <param name="platform">Platform added</param>
        /// <param name="username">Username added</param>
        /// <param name="password">Password added</param>
        /// <returns>Returns success or fail</returns>

        public bool addPassword(string passcode, string platform, string username, string password)
        {
            // This function can be used to add an information set to the database. It will handle
            // the encryption of data, duplicates and writing to the database itself
            // Accepts passcode as String, platform as String, username as String, password as String /
            // Return success as boolean
            // NOTE: In this function, the password is the password that the user needs to store
            // not the password used to access the database. That is the passcode here

            DateTime time = DateTime.Now; // We also store the date and time for validating purposes

            // Below, all data is encrypted according to the method discussed. Refer to documentation
            // for more information
            string encPlatform = encrypt(platform, passcode);
            string encUsername = encrypt(username, passcode + platform);
            string encPassword = encrypt(password, passcode + platform + username);
            string encTime = encrypt(time.ToString("yyyy-MM-dd HH:mm:ss"), passcode + platform + username);

            // A profile is created for that particular username as a dictionary that has the username
            // as the key and information in the tuple form
            Dictionary<string, List<string>> profile = new Dictionary<string, List<string>>();
            List<string> info = new List<string>(); info.Add(encPassword); info.Add(encTime);
            profile.Add(encUsername, info);

            // Here we check if the platform is already included in the database. We store all the
            // instances of this platform appearing in the database as a list using the getEncPlatformNames()
            // function.
            // NOTE: In theory, there should be only one instance but we still check for multiple just in case
            // NOTE: The reason why we do is so that we can store the new profile data under the same platform
            // instead of creating new instance of this platform
            // NOTE: A platform can have multiple user profiles and multiple user profiles with the same username
            // can be created under different platforms. However, same platform MUST NOT hold multiple
            // profiles under the same username. Checks to mitigate this is taken here
            List<string> platformInstances = getEncPlatformNames(passcode, platform);

            if (getUserData(passcode, platform, username).Count == 0)
            {
                // We first check if a user profile exist under the same username within this platform. This is
                // done by using the count of lists returned by the getUserData() function. If so, we throw an
                // error. Otherwise we check if the platform is a new platform. This is done by using the count
                // of platformInstances list. If so we use the encrypted platform name, or else we use the
                // first instance of platform name we get and discard the encrypted platform name we made before
                // Then we append the profile to the list
                if (platformInstances.Count == 0)
                {
                    List<Dictionary<string, List<string>>> append = new List<Dictionary<string, List<string>>>();
                    append.Add(profile);
                    database[encPlatform] = append;
                }
                else
                {
                    // print(platformInstances[0]);
                    database[platformInstances[0]].Add(profile);
                }

                // Once updating the loaded database, we dump it back to the physical database to store
                dumpDatabase();

                return true;
            }

            return false;
        }

        /// <summary>
        /// Deletes an existing profile
        /// </summary>
        /// <param name="passcode">Passcode user entered for authentication</param>
        /// <param name="platform">Platform user entered</param>
        /// <param name="username">Username user entered</param>
        /// <returns>Error</returns>

        public int deletePassword(string passcode, string platform, string username)
        {
            // This function will delete an existing profile from the database. This will go through the
            // database and delete all instances of the profile within the given platform.
            // Accepts passcode as String, platform as String, username as String / Return success as boolean

            // Here we check if the platform is already included in the database. We store all the
            // instances of this platform appearing in the database as a list using the getEncPlatformNames()
            // function.
            // NOTE: In theory, there should be only one instance but we still check for multiple just in case
            List<string> platformInstances = getEncPlatformNames(passcode, platform);

            if (getUserData(passcode, platform, username).Count != 0)
            {
                // We first check if a user profile exist under the same username within this platform. This is
                // done by using the count of lists returned by the getUserData() function. If not, we throw an
                // error. Otherwise we check if the platform exist. This is done by using the count of
                // platformInstances list. If so we use the encrypted platform name to get all profiles under
                // it. Then for each list item, we iterate and check if the decrypted first key item (Because
                // the profile dictionaries have only a single key value pair) match with the username. If so
                // we delete that instance from the database
                if (platformInstances.Count == 0)
                    // Profile not exist
                    return error_list.non_existent_username;
                else
                {
                    // Found the profile in the right platform under right username!
                    // print(platformInstances[0])
                    foreach (string platformItem in platformInstances)
                    {
                        List<Dictionary<string, List<string>>> usernameInstances = database[platformItem];

                        foreach (Dictionary<string, List<string>> usernameInstance in usernameInstances)
                        {
                            List<string> keys = new List<string>();
                            keys.AddRange(usernameInstance.Keys);

                            if (decrypt(keys[0], passcode + platform) == username)
                            {
                                database[platformItem].Remove(usernameInstance);
                                dumpDatabase();

                                return error_list.success;
                            }
                        }
                    }
                }

                // Once updating the loaded database, we dump it back to the physical database to store
                dumpDatabase();
            }
            else
            {
                // Platform not exist
                return error_list.non_existent_platform;
            }

            return error_list.fail;
        }

        /// <summary>
        /// Edit an existing profile
        /// </summary>
        /// <param name="passcode">Passcode entered by the user for authentication</param>
        /// <param name="platform">Platform user entered</param>
        /// <param name="username">Username user entered</param>
        /// <param name="password">Password user entered</param>
        /// <returns>Error</returns>

        public int editPassword(string passcode, string platform, string username, string password)
        {
            // This function will edit an existing profile from the database. This will go through the
            // database and edit all instances of the profile within the given platform.
            // Accepts passcode as String, platform as String, username as String, password as String / Return
            // success as boolean

            // Here we check if the platform is already included in the database. We store all the
            // instances of this platform appearing in the database as a list using the getEncPlatformNames()
            // function.
            // NOTE: In theory, there should be only one instance but we still check for multiple just in case
            List<string> platformInstances = getEncPlatformNames(passcode, platform);

            if (getUserData(passcode, platform, username).Count != 0)
            {
                // We first check if a user profile exist under the same username within this platform. This is
                // done by using the count of lists returned by the getUserData() function. If not, we throw an
                // error. Otherwise we check if the platform exist. This is done by using the count of
                // platformInstances list. If so we use the encrypted platform name to get all profiles under
                // it. Then for each list item, we iterate and check if the decrypted first key item (Because
                // the profile dictionaries have only a single key value pair) match with the username. If so
                // we edit that instance in the database itself to include the new password along with the current
                // time
                if (platformInstances.Count == 0)
                    // Profile not exist
                    return error_list.non_existent_username;

                else
                {
                    // Found the profile in the right platform under right username!
                    // print(platformInstances[0])
                    foreach (string platformItem in platformInstances)
                    {
                        List<Dictionary<string, List<string>>> usernameInstances = database[platformItem];

                        for (int i = 0; i < usernameInstances.Count; i++)
                        {
                            List<string> keys = new List<string>();
                            keys.AddRange(usernameInstances[i].Keys);
                            string usernameInstance = keys[0];

                            if (decrypt(usernameInstance, passcode + platform) == username)
                            {
                                if (decrypt(database[platformItem][i][usernameInstance][0], passcode + platform + username) != password)
                                {
                                    DateTime time = DateTime.Now;

                                    string encPassword = encrypt(password, passcode + platform + username);
                                    string encTime = encrypt(time.ToString("yyyy-MM-dd HH:mm:ss"), passcode + platform + username);

                                    List<string> data = new List<string>();
                                    data.Add(encPassword);
                                    data.Add(encTime);
                                    database[platformItem][i][usernameInstance] = data;

                                    // Once updating the loaded database, we dump it back to the physical database to store
                                    dumpDatabase();

                                    return error_list.success;
                                }
                                else
                                {
                                    return error_list.same_password_to_edit;
                                }
                            }
                        }
                    }
                }
                dumpDatabase();
            }
            else
            {
                // Platform not exist
                return error_list.non_existent_platform;
            }

            return error_list.fail;
        }

        /// <summary>
        /// Get the decrypted platform names from the database
        /// </summary>
        /// <param name="password">Password entered by the user</param>
        /// <param name="keyword">Keyword user entered for search</param>
        /// <returns></returns>

        public List<string> getPlatformNames(string password, string keyword = "")
        {
            // GET THE **DECRYPTED** PLATFORM NAMES FROM DATABASE
            // This function scans the database and retrievs the platforms from it
            // NOTE: These data are decrypted and thus the returned data is NOT ENCRYPTED
            // NOTE: This only returns the list of decrypted platform names as String
            // NOTE: In theory, the platform name must appear only once in the database but still we do
            // a looping check for added safety and retrieve every appearance from a loop
            // NOTE: This function can be used to validate if a platform already exists in the database
            // Accepts password as String, keyword as String / Returns the retrieved data as list

            List<string> platformData = new List<string>();

            foreach (string item in database.Keys)
            {
                // We first take out all the keys in the database[Platforms] and for each one, we decrypt
                // it using the password and check if it exist in the list. If not we add it
                string platform = decrypt(item, password);

                if (!(platformData.Contains(platform)) && platform.Contains(keyword))
                {
                    platformData.Add(platform);
                }
            }

            return platformData;
        }

        /// <summary>
        /// Search usernames in database
        /// </summary>
        /// <param name="password">Passcode entered by the user</param>
        /// <param name="keyword">Keyword to search</param>
        /// <param name="platform">Platform the user entered</param>
        /// <returns>List of tuples with (platform, list of usernames)</returns>

        public List<Tuple<string, List<string>>> searchUsernames(string password, string keyword = "", string platform = "")
        {
            // This function can search the database for a particular username or all and
            // return them back as a list of tuples with Platform and its corresponding
            // usernames.
            // Accept password as String, keyword as String(Default ''), platform as String(Default '') /
            // Return platforms as a list of tuples of the form
            // [(Platform 01, [Username 01, Username 02, ...]), (Platform 02, [Username 01, Username 02, ...]), ...]
            List<string> platforms = getPlatformNames(password, platform);
            List<Tuple<string, List<string>>> returnData = new List<Tuple<string, List<string>>>(); // List to be returned

            // We iterate through each platform and use getUsernamesInPlatform() to get the matching
            // usernames. If data is returned, we append them to the return variable in correct
            // form
            foreach (string item in platforms)
            {
                List<string> matches = getUsernamesInPlatform(password, item, keyword);

                if (item.Contains(platform))
                {
                    Tuple<string, List<string>> match = new Tuple<string, List<string>>(item, matches);
                    returnData.Add(match);
                }
            }

            return returnData;
        }

        ////////////////////////////////////////////////////////
        ////// PASSWORD RELATED FUNCTIONS

        /// <summary>
        /// Generate a password automatically
        /// </summary>
        /// <param name="length">Length of the required password</param>
        /// <param name="uppercase">Include uppercase?</param>
        /// <param name="numbers">Include numbers?</param>
        /// <param name="specialChar">Include specialChars?</param>
        /// <returns>Random password</returns>

        public string randomPassword(int length = 12, bool uppercase = true, bool numbers = true, bool specialChar = true)
        {
            // This function generates a password automatically at random for security
            // It uses a list of characters from eacj type of characters and uses the random module
            // to randomly assign to a string
            // Accepts length as int(Default 12), uppercase as Boolean(Default True), numbers as
            // Boolean(Default True), specialChar as Boolean(Default True) / Return password as String
            // These variables can be changed to customize the password style
            List<char> charList = new List<char>();

            // Adding lowercase characters
            for (int i = 97; i <= 122; i++)
            { charList.Add((char)i); }

            // Adding uppercase characters
            if (uppercase)
                for (int i = 65; i <= 90; i++)
                { charList.Add((char)i); }

            // Adding numbers
            if (numbers)
                for (int i = 48; i <= 57; i++)
                { charList.Add((char)i); }

            // Adding special characters
            if (specialChar)
            {
                for (int i = 33; i <= 47; i++)
                { charList.Add((char)i); }

                for (int i = 58; i <= 64; i++)
                { charList.Add((char)i); }

                for (int i = 91; i <= 96; i++)
                { charList.Add((char)i); }

                for (int i = 123; i <= 126; i++)
                { charList.Add((char)i); }
            }

            string charPallette = string.Join("", charList.ToArray());

            string randomPW = "";

            Random rnd = new Random();

            for (int i = 0; i < length; i++)
            {
                randomPW += charPallette[rnd.Next(0, charPallette.Length - 1)];
            }

            return randomPW;
        }

    }
}
