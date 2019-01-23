using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Taskalu
{
    class Ngram
    {
        public static String getNgramText(String text, int ngr)
        {
            StringBuilder ngramText = new StringBuilder();
            int length = text.Length;
            if (length < 1)
            {
                return "";
            }
            if (length > ngr)
            {
                int index = length - ngr;
                for (int i = 0; i < index; i++)
                {
                    ngramText.Append(text.Substring(i, ngr));
                    ngramText.Append(" ");
                    if (i == (index - 1))
                    {
                        i++;
                        ngramText.Append(text.Substring(i, ngr));
                        if (i % ngr >= ngr)
                        {
                            i++;
                            ngramText.Append(" ");
                            ngramText.Append(text.Substring(i, ngr));
                        }
                    }
                }
            }
            else
            {
                ngramText.Append(text);
            }
            return ngramText.ToString();
        }

    }
}
