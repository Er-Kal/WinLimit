using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using dotenv.net;
using Supabase.Gotrue;
using WinLimit.Models;

namespace WinLimit.Services;

public class SupabaseService
{
    // Attributes
    private string supabaseURL;
    private string supabaseKey;
    public Supabase.Client? Client;

    // Constructor
    public SupabaseService()
    {
        // Load Environment Variables
        DotEnv.Load();
        var envVars = DotEnv.Read();
        // Set attributes
        this.supabaseKey = envVars["SupabaseKey"];
        this.supabaseURL = envVars["SupabaseURL"];
    }
    async public Task InitializeAsync()
    {
        // Make sure this doesnt keep running
        if (Client != null) return;
        // Client options
        var options = new Supabase.SupabaseOptions
        {
            AutoConnectRealtime = true,
        };
        // Initiate the Supabase Client
        Client = new Supabase.Client(this.supabaseURL, this.supabaseKey, options);
        await Client.InitializeAsync();
    }

    public async Task<string> getFirstUserEmail()
    {
        if (Client == null)
            await InitializeAsync();

        // Select * From Users
        var results = await this.Client!.From<Users>().Get();

        // Check if anything was returned
        if (results.Models.Count > 0)
            return results.Models[0].email ?? "No Users";
        return "No Users";
    }
    // Sign up function
    public async Task<Session> SignUp(string email, string password)
    {
        try
        {
            if (Client == null)
                await InitializeAsync();

            Session session = await Client.Auth.SignUp(email, password);

            if (session.User != null)
                return session;

            throw new Exception("User failed to create");
        }
        catch (Exception ex)
        {
            throw new Exception("User failed to create");
        }
    }
}