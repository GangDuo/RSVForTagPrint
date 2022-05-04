using System.Linq;
using System.Reflection;

namespace RSVForTagPrint.Helpers
{
    class Preferences
    {
        public static readonly string HostName = "FmwwHostName";
        public static readonly string AccessKeyId = "FmwwAccessKeyId";
        public static readonly string UserName = "FmwwUserName";
        public static readonly string VirgoApiUri = "VirgoApiUri";

        public static readonly string CNGroupPassword = "GroupPassword";
        public static readonly string CNUserPassword = "UserPassword";

        public static object GetVariable(string variable)
        {
            var type = typeof(Properties.Settings);
            var instance = type.GetProperty("Default").GetMethod.Invoke(null, null);

            var properties = type.GetProperties(
                BindingFlags.DeclaredOnly |
                BindingFlags.Public |
                BindingFlags.Instance)
                .Select(p => p.Name);
            if (properties.Contains(variable))
            {
                return type.GetProperty(variable).GetValue(instance);
            }

            return Password.Read(variable);
        }
    }
}
