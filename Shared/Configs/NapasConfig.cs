namespace Shared.Configs
{
    public class NapasConfiguration
    {
        public string BaseUrl { get; set; }
        public string TokenEndpoint { get; set; }
        public string InvestigationEndpoint { get; set; }
        public string NotificationEndpoint { get; set; }
        public string ReconciliationEndpoint { get; set; }
        public string ClientKey { get; set; }
        public string ClientSecret { get; set; }
        public string PrivateKeyPemPath { get; set; }
        public string PublicKeyPemPath { get; set; }
        public string ClientCertPath { get; set; }
        public string ClientCertPassword { get; set; }
        public string NapasPayloadCertPath { get; set; }
        public string ZenPayPayClientKey { get; set; }
    }
}
