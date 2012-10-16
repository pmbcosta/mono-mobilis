using System.Text.RegularExpressions;
using System;
using System.Net;
using System.IO;
using System.Text;

namespace Mobilis.Lib.Util
{
    public class HttpUtils
    {
        static string[] months = { "janeiro", "fevereiro", "mar�o", "abril", "maio", "junho", "julho", "agosto", "setembro", "outubro", "novembro", "dezembro" };

        public static string Strip(string text)
        {
            return Regex.Replace(text, @"<(.|\n)*?>", string.Empty).Replace("//s", " ").Trim();
        }

        public static string postDateToServerFormat(string date)
        {
            return date.Substring(0, 19).Replace("T", string.Empty).Replace("-", string.Empty).Replace(":", string.Empty);
        }

        public static string discussionDateToShowFormat(string date)
        {
            return date.Substring(0, 10).Replace("-", "/");
        }

        public static string postDateToShowFormat(string date)
        {
            string header = "";
            int year = Convert.ToInt16(date.Substring(0, 4));
            int month = Convert.ToInt16(date.Substring(5, 2));
            int day = Convert.ToInt16(date.Substring(8, 2));
            int hour = Convert.ToInt16(date.Substring(11, 2));
            int minute = Convert.ToInt16(date.Substring(14, 2));
            if (DateTime.Today.Year == year)
            {
                if (DateTime.Today.Month == month)
                {
                    if (DateTime.Today.Day == day)
                    {
                        if (DateTime.Today.Hour == hour)
                        {
                            header = "H� "
                                + (DateTime.Today.Minute - minute)
                                + " minutos";
                        }
                        else
                        {
                            header = "�s " + hour + "horas";
                        }
                    }
                    else
                    {
                        if (DateTime.Today.Day - 1 == day)
                        {
                            header = "Ontem";
                        }
                        else
                        {
                            header = "Dia " + day + " �s " + hour
                                + " horas";
                        }
                    }
                }
                else
                {
                    header = "Dia "
                        + day
                        + " de "
                        + months[month - 1];
                }
            }
            else
            {
                header = "Dia "
                            + day
                            + " de "
                            + months[month - 1] + " de " + year;
            }
            return header;
        }

        public static byte[] toByteArray(string s)
        {
            return System.Text.Encoding.UTF8.GetBytes(s);
        }

        public static string WebResponseToString(WebResponse response)
        {
            Stream responseStream = response.GetResponseStream();
            StreamReader responseReader = new System.IO.StreamReader(responseStream, Encoding.UTF8);
            string result = responseReader.ReadToEnd();
            return result;
        }

        public static void SaveFileToStorage(WebResponse response,int fileId) 
        {
            byte[] result = null;
            byte[] buffer = new byte[4097];

            Stream responseStream = response.GetResponseStream();
            MemoryStream memoryStream = new MemoryStream();

            int count = 0;

            do
            {
                count = responseStream.Read(buffer, 0, buffer.Length);
                memoryStream.Write(buffer, 0, count);

                if (count == 0)
                {
                    break;
                }
            }
            while (true);

            result = memoryStream.ToArray();

            FileStream fs = new FileStream(Constants.RECORGING_PATH + fileId + ".wav", FileMode.OpenOrCreate, FileAccess.ReadWrite);

            fs.Write(result, 0, result.Length);

            fs.Close();
            memoryStream.Close();
            responseStream.Close();
        }
    }
}