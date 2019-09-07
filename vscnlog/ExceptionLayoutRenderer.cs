using System;
using System.ComponentModel;
using System.Text;
using NLog;
using NLog.Config;
using NLog.LayoutRenderers;

namespace VSC.NLog
{
    [LayoutRenderer("ExceptionRenderer")]
    [ThreadAgnostic]
    public class ExceptionLayoutRenderer : LayoutRenderer
    {
        protected ExceptionLayoutRenderer()
        {
        }

        [DefaultValue("")]
        public string Indent { get; set; }

        protected override void Append(StringBuilder builder, LogEventInfo logEvent)
        {
            var ex = logEvent.Exception;
            if(ex != null)
            {
                this.FormatErrorMessage(builder, ex, this.Indent);
            }
        }

        private void FormatErrorMessage(StringBuilder builder, Exception exception, string indent = "")
        {
            string newLine = "\n";
            string stars = new string('*', 80);
            builder.Append(indent + stars + newLine);
            builder.Append(indent + exception.GetType().Name + ": \"" + exception.Message + "\"" + newLine);
            builder.Append(indent + new string('-', 80) + newLine);

            try
            {
                if (exception.InnerException != null)
                {
                    builder.Append(indent + "InnerException:" + newLine);
                    FormatErrorMessage(builder, exception.InnerException, indent + "   ");
                }

                if (exception.StackTrace != null)
                {
                    foreach (string line in exception.StackTrace.Split(new string[] { " at " }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        if (string.IsNullOrEmpty(line.Trim())) continue;
                        string[] parts;
                        parts = line.Trim().Split(new string[] { " in " }, StringSplitOptions.RemoveEmptyEntries);
                        string class_info = parts[0];
                        if (parts.Length == 2)
                        {
                            parts = parts[1].Trim().Split(new string[] { "line" }, StringSplitOptions.RemoveEmptyEntries);
                            string src_file = parts[0];
                            int line_nr = int.Parse(parts[1]);
                            builder.Append(indent + "  " +src_file.TrimEnd(':') + "(" + line_nr + ",1):   " + class_info + newLine);
                        }
                        else
                        {
                            builder.Append(indent + "  " + class_info + newLine);
                        }
                    }
                }

                builder.Append(indent + stars + newLine);
            }
            catch (Exception ex)
            {
                builder.Append(ex.ToString());
            }
        }
    }
}
