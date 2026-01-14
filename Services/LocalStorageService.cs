using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
public class LocalStorageService
{
    private readonly string _folderPath;
    private readonly string _rulesFile;
    private readonly string _tokenFile;

    // Constructor, gets app directory path
    // Creates filepaths for session and rules
    public LocalStorageService(){
        string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        _folderPath = Path.Combine(appData,"WinLimitApp");

        if (!Directory.Exists(_folderPath))
        {
            Directory.CreateDirectory(_folderPath);
        }

        _rulesFile = Path.Combine(_folderPath,"rules.json");
        _tokenFile = Path.Combine(_folderPath,"session.dat");
    }
    // Encrypt the given JWT token
    // Uses the Window's machines currently logged in user's credentials to encrypt it
    public void SaveToken(string jwt)
    {
        byte[] jwtData = Encoding.UTF8.GetBytes(jwt);
        byte[] encryptedJwtData = ProtectedData.Protect(jwtData,null,DataProtectionScope.CurrentUser);

        File.WriteAllBytes(_tokenFile,encryptedJwtData);
    }

    public string? LoadToken(string jwt)
    {
        if (!File.Exists(_tokenFile))
        {
            return null;
        }

        try
        {
            byte[] encryptedData = File.ReadAllBytes(_tokenFile);
            byte[] jwtData = ProtectedData.Unprotect(encryptedData,null,DataProtectionScope.CurrentUser);
            string jwtString = Encoding.UTF8.GetString(jwtData);
            return jwtString;
        }
        catch // Either the current user is not the right one, pass changed, file moved
        {
            return null;
        }
    }
}