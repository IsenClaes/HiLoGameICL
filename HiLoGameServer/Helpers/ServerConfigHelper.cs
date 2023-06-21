using System.Configuration;

namespace HiLoGameServer.Helpers
{
    /// <Summary>
    /// The Server config helper
    /// </Summary>
    public static class ServerConfigHelper
    {
        public static int LowerBound => int.Parse(ConfigurationManager.AppSettings["lowerBound"] ?? string.Empty);

        public static int UpperBound => int.Parse(ConfigurationManager.AppSettings["upperBound"] ?? string.Empty);

        public static int ServerPort => int.Parse(ConfigurationManager.AppSettings["serverPort"] ?? string.Empty);
    }
}