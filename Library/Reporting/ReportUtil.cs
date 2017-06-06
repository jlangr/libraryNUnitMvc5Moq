using System;
using System.Text;

namespace Library.Reporting
{
    public class ReportUtil
    {
        public enum StringOp
        {
            Pad, Under, Camel
        }

        public static string Transform(string x, int count, int spacing, ReportUtil.StringOp op)
        {
            var buffer = new StringBuilder();
            var buffer1 = new StringBuilder();
            string pads;
            if (op == StringOp.Pad)
            {
                pads = " ";
                var l = spacing;
                while (l > 1)
                {
                    pads = pads + " ";
                    l = l - 1;
                }
                buffer.Append(x);
                buffer.Append(pads);
                buffer1.Append(buffer.ToString());
            }
            if (op == StringOp.Under)
            {
                var i = 0;
                for (; i < count; i++)
                    buffer.Append('-');
                var ptext = buffer.ToString();
                pads = "";
                buffer1.Append(ptext);
                var l = count + spacing - ptext.Length;
                for (int j = 0; j < l; j++)
                {
                    pads += " ";
                }
                buffer1.Append(pads);
            }
            if (op == StringOp.Camel)
            {
                var m = 0;
                bool lower = false;
                for (; m < x.Length; m++)
                {
                    if (Char.IsWhiteSpace(x[m]))
                    {
                        lower = false;
                        buffer1.Append(x[m]);
                    }
                    else if (!lower)
                    {
                        buffer1.Append(Char.ToUpper(x[m]));
                        lower = true;
                    }
                    else
                        buffer1.Append(x[m]);
                }
            }
            return buffer1.ToString();
        }
    }
}