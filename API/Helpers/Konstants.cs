namespace API.Helpers;

public  static class Konstants
{
    public const string CorsPolicy = "CorsPolicy";

    public static readonly Environment Env = new Environment();
    
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
        }

        public enum Type
        {
            Development,
            Production
        } 
    }
}

