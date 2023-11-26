using System.Text;

namespace DA_Assets.Shared
{
    public class DAFormatter
    {
        private const int indentLenght = 4;

        private static string Repeat(int n) => new string(' ', n * indentLenght);

        public static JFResult Format(string str)
        {
            JFResult jsonFormatResult = new JFResult();

            bool hasOpenBrase = false;
            bool hasCloseBrase = false;

            int indent = 0;
            bool quoted = false;

            StringBuilder sb = new StringBuilder();

            for (var i = 0; i < str.Length; i++)
            {
                var ch = str[i];
                switch (ch)
                {
                    case '{':
                    case '[':
                        if (ch == '{')
                            hasOpenBrase = true;

                        sb.Append(ch);
                        if (quoted == false)
                        {
                            sb.AppendLine();
                            sb.Append(Repeat(++indent));
                        }
                        break;
                    case '}':
                    case ']':
                        if (ch == '}')
                            hasCloseBrase = true;

                        if (quoted == false)
                        {
                            sb.AppendLine();
                            sb.Append(Repeat(--indent));
                        }
                        sb.Append(ch);
                        break;
                    case '"':
                        sb.Append(ch);
                        bool escaped = false;
                        var index = i;
                        while (index > 0 && str[--index] == '\\')
                            escaped = !escaped;
                        if (escaped == false)
                            quoted = !quoted;
                        break;
                    case ',':
                        sb.Append(ch);
                        if (quoted == false)
                        {
                            sb.AppendLine();
                            sb.Append(Repeat(indent));
                        }
                        break;
                    case ':':
                        sb.Append(ch);
                        if (quoted == false)
                            sb.Append(" ");
                        break;
                    default:
                        sb.Append(ch);
                        break;
                }
            }

            jsonFormatResult.Json = sb.ToString();
            jsonFormatResult.IsValid = hasOpenBrase && hasCloseBrase;

            return jsonFormatResult;
        }
    }

    public struct JFResult
    {
        public bool IsValid { get; set; }
        public string Json { get; set; }
    }
}
