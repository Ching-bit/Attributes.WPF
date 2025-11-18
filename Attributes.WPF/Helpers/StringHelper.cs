using System.Linq;
using System.Text;

namespace Attributes.WPF
{
    public static class StringHelper
    {
        public static string ToCamel(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return str;
            }

            StringBuilder sb = new StringBuilder(str);
            for (int i = 0; i < sb.Length - 1; i++)
            {
                if ('_' == sb[i])
                {
                    sb[i + 1] = char.ToUpper(sb[i + 1]);
                }
            }

            sb[0] = char.ToUpper(sb[0]);
            sb.Replace("_", "");
            return sb.ToString();
        }

        public static string ToLowerCamel(string str, string prefix = "")
        {
            if (string.IsNullOrEmpty(str))
            {
                return str;
            }

            StringBuilder sb = new StringBuilder(str);
            for (int i = 0; i < sb.Length - 1; i++)
            {
                if ('_' == sb[i])
                {
                    sb[i + 1] = char.ToUpper(sb[i + 1]);
                }
            }

            sb[0] = char.ToLower(sb[0]);
            sb.Replace("_", "");
            return prefix + sb;
        }

        public static string ToGlobalFullName(string typeName)
        {
            string[] basicTypes =
            {
                "bool", "byte", "sbyte", "short", "ushort", "int", "uint", "long", "ulong", "char", "float", "double",
                "decimal", "string", "object"
            };

            if (null == typeName || basicTypes.Any(t => t == typeName))
            {
                return typeName;
            }

            return "global::" + typeName;
        }
    }
}
