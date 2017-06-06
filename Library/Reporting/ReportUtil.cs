using System.Text;

namespace Library.Reporting
{
    public class ReportUtil
    {
        public enum StringOp
        {
            Pad, Under
        }

        public static string Transform(string x, int count, int spacing, ReportUtil.StringOp op)
        {
            var buffer = new StringBuilder();
            var buffer1 = new StringBuilder();
            string pads;
            switch (op)
            {
                case StringOp.Under:
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
                    break;
                case StringOp.Pad:
                    pads = " ";
                    l = spacing;
                    while (l > 1)
                    {
                        pads = pads + " ";
                        l = l - 1;
                    }
                    buffer.Append(x);
                    buffer.Append(pads);
                    buffer1.Append(buffer.ToString());
                    break;
            }
            return buffer1.ToString();
        }
    }
}