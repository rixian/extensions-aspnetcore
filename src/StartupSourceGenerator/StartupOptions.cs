namespace StartupSourceGenerator
{
    internal class StartupOptions
    {
        public bool EnableWebApi { get; set; }

        public bool EnableRazorPages { get; set; }

        public bool EnableDataProtection { get; set; }

        public bool EnableRedis { get; set; }

        public bool EnableOAuth2 { get; set; }

        public bool EnableOpenIdConnect { get; set; }
    }
}
