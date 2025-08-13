using DVLD_Business_Layer;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD_System.Global_Classes
{
    internal static class clsGlobal
    {
        public static clsUser CurrentUser;


        // This method to save user login data in a text file.
        public static bool RememberUsernameAndPassword(string Username, string Password)
        {
            try
            {
                // This will get the current project directory folder.
                string currentDirectory = System.IO.Directory.GetCurrentDirectory();


                // Define the path to the text file where you want to save the data
                string filePath = currentDirectory + "\\data.txt";


                // Incase the username is empty, delete the file
                if (Username == "" && File.Exists(filePath))
                {
                    File.Delete(filePath);
                    return false;
                }

                // concatonate username and passwrod withe seperator.
                string dataToSave = Username + "#//#" + Password;


                // Create a StreamWriter to write to the file
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    // Write the data to the file
                    writer.WriteLine(dataToSave);
                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
                // Log the error to the event viewer
                clsLogger.Log(ex, System.Diagnostics.EventLogEntryType.Error);
                return false;
            }
        }
        public static bool GetStoredCredential(ref string Username, ref string Password)
        {
            // This will get the stored username and password and will return true if found and false if not found.
            try
            {
                // Gets the current project's directory
                string currentDirectory = System.IO.Directory.GetCurrentDirectory();

                // Path for the file that contains the credential.
                string filePath = currentDirectory + "\\data.txt";

                // Check if the file exists before attempting to read it
                if (File.Exists(filePath))
                {
                    // Create a StreamReader to read from the file
                    using (StreamReader reader = new StreamReader(filePath))
                    {
                        // Read data line by line until the end of the file
                        string line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            Console.WriteLine(line); // Output each line of data to the console
                            string[] result = line.Split(new string[] { "#//#" }, StringSplitOptions.None);

                            Username = result[0];
                            Password = result[1];
                        }
                        return true;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while reading the data credential from data file \n: {ex.Message}");
                clsLogger.Log(ex);
                return false;
            }
        }


        // This method to save user login data in the registry.
        public static bool RememberUsernameAndPasswordInRegistry(string Username, string Password)
        {
            string subkey = @"SOFTWARE\DVLD";

            try
            {
                using (RegistryKey key = Registry.CurrentUser.CreateSubKey(subkey))
                {
                    if (key == null)
                    {
                        return false;
                    }

                    key.SetValue("DVLD_Login_UserName", Username, RegistryValueKind.String);
                    key.SetValue("DVLD_Login_Password", Password, RegistryValueKind.String);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while reading the data credential in REGISTRY! \n: {ex.Message}");
                clsLogger.Log(ex);
                return false;
            }

            return true;
        }
        public static bool GetStoredCredentialFromRegistry(ref string Username, ref string Password)
        {
            string subkey = @"SOFTWARE\DVLD";

            try
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(subkey))
                {
                    if (key == null)
                    {
                        return false;
                    }

                    Username = key.GetValue("DVLD_Login_UserName") as string;
                    Password = key.GetValue("DVLD_Login_Password") as string;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while reading the data credential in REGISTRY! \n: {ex.Message}");
                clsLogger.Log(ex);
                return false;
            }

            return true;
        }

    }
}
