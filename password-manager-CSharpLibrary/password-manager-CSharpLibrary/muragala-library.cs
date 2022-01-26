using System;
using Fernet; // Fernet encryption
using System.Collections.Generic; // Access Dictionary and List data types
using System.Security.Cryptography; // Cryptographic functions

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

        //////////////////////////////////////////////////////////////////
        /// PUBLIC FUNCTIONS

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
    }
}
