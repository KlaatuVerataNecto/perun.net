﻿namespace peruncore.Config
{
    public static class ConfigVariables
    {
        public static string AuthSchemeName { get { return "Cookies"; } }
        public static string GoogleClientId { get { return "clientid"; } }
        public static string GoogleClientSecret { get { return "secret"; } }

        public static string AuthSchemeGoogle { get { return "Google"; } }
        public static string AuthSchemeFacebook { get { return "Facebook"; } }

        public static string LogFile { get { return "log/peruncore-{Date}.log"; } }
    }
}