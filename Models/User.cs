using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace WinLimit.Models;
// User Data Model

[Table("users")]
public class Users : BaseModel
{
    [PrimaryKey("userid")]
    public string ?uuid {get; set;}

    [Column("username")]
    public string ?username {get; set;}

    [Column("email")]
    public string ?email {get; set;}
    
    [Column("password")]
    public string ?password {get; set;}
}