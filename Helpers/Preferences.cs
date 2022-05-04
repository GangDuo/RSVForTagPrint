using System.Linq;
using System.Reflection;

namespace RSVForTagPrint.Helpers
{
    class Preferences
    {
        public static string CNGroupPassword = "GroupPassword";
        public static string CNUserPassword = "UserPassword";

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
