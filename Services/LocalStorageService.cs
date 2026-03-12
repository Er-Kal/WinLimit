using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using WinLimit.Models;
using WinLimit.Services;
public class LocalStorageService
{
    private readonly string _folderPath;
    private readonly string _schedulesFile;
    private readonly string _blockedAppsFile;
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

        _schedulesFile = Path.Combine(_folderPath, "schedules.json");
        _blockedAppsFile = Path.Combine(_folderPath, "blocked_apps.json");
        _tokenFile = Path.Combine(_folderPath,"session.dat");
    }
    // Encrypt the given JWT token
    // Uses the Window's machines currently logged in user's credentials to encrypt it
    public void SaveToken(string jwt)
    {
        try
        {    
            byte[] jwtData = Encoding.UTF8.GetBytes(jwt);
            byte[] encryptedJwtData = ProtectedData.Protect(jwtData,null,DataProtectionScope.CurrentUser);

            File.WriteAllBytes(_tokenFile,encryptedJwtData);
        }
        catch
        {
            Console.WriteLine("Theres an error writing the save token");
        }
    }
    public string? LoadToken()
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
    public void DeleteToken()
    {
        try
        {
            if (File.Exists(_tokenFile))
            {
                File.Delete(_tokenFile);
            }
        }
        catch
        {
            Console.WriteLine("Theres an error deleting the token");
        }
    }
    public void SaveSchedules(string jsonString)
    {
        try
        {    
            
            File.WriteAllText(_schedulesFile, jsonString);
        }
        catch
        {
            Console.WriteLine("Theres error");
        }
    }

    public Dictionary<string, List<ScheduleRule>>? LoadSchedules()
    {
        try
        {
            string data = File.ReadAllText(_schedulesFile);
            return JsonSerializer.Deserialize<Dictionary<string,List<ScheduleRule>>>(data);
        }
        catch (FileNotFoundException) // No file
        {
            return null;
        }
    }

    public void SaveBlockedApps(string jsonString)
    {
        try
        {
            File.WriteAllText(_blockedAppsFile, jsonString);
        }
        catch
        {
            Console.WriteLine("Theres error");
        }
    }

    public List<BlockItem>? LoadBlockedApps()
    {
        try
        {
            string data = File.ReadAllText(_blockedAppsFile);
            return JsonSerializer.Deserialize<List<BlockItem>>(data);
        }
        catch (FileNotFoundException) // No file
        {
            return null;
        }
    }
    
}