namespace API.Helpers;

public  static class Konstants
{
    public const string CorsPolicy = "CorsPolicy";
    public const string TokenKey = "TokenKey";
    public const string DefaultConnection = "DefaultConnection";

    public static readonly Environment Env = new Environment();
    
    public enum ClaimTypes
    {
        DeviceId,
    }
    
    public struct Environment
    {
        public readonly string Name;
        public readonly Type EnvType;

        public Environment()
        {
            this.Name = "ASPNETCORE_ENVIRONMENT";
            this.EnvType = new Type();
            
        }
    
        public Environment(string name)
        {
            this.Name = name;
            this.EnvType = new Type();
        }

        public enum Type
        {
            Development,
            Production
        } 
    }
}

