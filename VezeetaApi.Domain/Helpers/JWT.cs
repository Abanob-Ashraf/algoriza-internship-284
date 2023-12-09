namespace VezeetaApi.Domain.Helpers
{
    public class JWT
    {
        public string Key { get; set; }
        public string ValidIssuer { get; set; }
        public string ValidAudiance { get; set; }
        public double DurationInMinutes { get; set; }
    }
}
