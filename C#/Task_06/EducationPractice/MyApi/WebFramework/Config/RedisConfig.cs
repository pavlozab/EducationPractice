namespace Config
{
    public class RedisConfig
    {
        public string Server { get; set; }
        public int Port { get; set; }
        public string AllowAdmin { get; set; }
        public string ConnectionString => $"{Server}:{Port},allowAdmin={AllowAdmin}";
    }
}