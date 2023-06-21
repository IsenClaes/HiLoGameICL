using System.Configuration;

namespace HiLoGameClient.Helpers
{
    /// <Summary>
    /// The Client config helper
    /// </Summary>
    public static class ClientConfigHelper
    {
        public static string ServerHost => ConfigurationManager.AppSettings["serverHost"] ?? string.Empty;

        public static int ServerPort => int.Parse(ConfigurationManager.AppSettings["serverPort"] ?? string.Empty);
    }
}