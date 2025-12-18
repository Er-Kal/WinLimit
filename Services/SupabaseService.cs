using System;
using System.Threading.Tasks;
using dotenv.net;
using Tmds.DBus.Protocol;
using WinLimit.Models;

namespace WinLimit.Services;

public class SupabaseService
{
    private string SupabaseURL;
    private string SupabaseKey;
    public Supabase.Client? client;
    public SupabaseService()
    {
        DotEnv.Load();
        var envVars = DotEnv.Read();
        this.SupabaseKey = envVars["SupabaseKey"];
        this.SupabaseURL = envVars["SupabaseURL"];
        createClient();
    }
    async public void createClient()
    {
        var options = new Supabase.SupabaseOptions
        {
            AutoConnectRealtime=true,
        };
        this.client = new Supabase.Client(this.SupabaseURL,this.SupabaseKey,options);
        await this.client.InitializeAsync();
    }

    public async Task<string> getSupabaseURL()
    {
        var results = await this.client!.From<User>().Get();
        return results.Models[0].email;
    }
}