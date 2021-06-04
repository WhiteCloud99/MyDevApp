using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Cache;
using System.Text;
using System.Threading.Tasks;

namespace MyTelegram
{
  public class TelegramBot
  {
    private static readonly string _baseUrl = "https://api.telegram.org/bot";
    private static readonly string _token = "1640789263:AAHsWxWv5nKxBSvKIoAMOeDsg1aWeWGLkKQ";
    public static string _chatId = string.Empty;

    public static bool SendMessage()
    {
      return SendMessage();
    }

    public static bool SendMessage(String chatId, string text, out string errorMessage)
    {
      string url = string.Format("{0}{1}/sendMessage", _baseUrl,_token);

      HttpWebRequest req = WebRequest.Create(new Uri(url)) as HttpWebRequest;
      req.CachePolicy = new HttpRequestCachePolicy(HttpRequestCacheLevel.NoCacheNoStore);
      req.Timeout = 30 * 1000;
      req.Method = "POST";
      req.ContentType = "application/json";

      string json = String.Format("{{\"chat_id\":\"{0}\", \"text\":\"{1}\"}}", chatId, EncodeJsonChars(text));

      byte[] data = UTF8Encoding.UTF8.GetBytes(json);
      req.ContentLength = data.Length;

      using (Stream stream = req.GetRequestStream())
      {
        stream.Write(data, 0, data.Length);
        stream.Flush();
      }

      HttpWebResponse httpResponse = null;

      try
      {
        httpResponse = req.GetResponse() as HttpWebResponse;
        if(httpResponse.StatusCode == HttpStatusCode.OK)
        {
          string responseData = null;
          using (Stream responseStream = httpResponse.GetResponseStream())
          {
            using (StreamReader reader = new StreamReader(responseStream, UTF8Encoding.UTF8))
            {
              responseData = reader.ReadToEnd();
            }

          }

          if(0 < responseData.IndexOf("\"ok\":true"))
          {
            errorMessage = string.Empty;
            return true;
          }
          else
          {
            errorMessage = String.Format("json 파싱오류: ({0})", responseData);
            return false;
          }
        }
        else
        {
          errorMessage = String.Format("Http Status: {0}", httpResponse.StatusCode);
          return false;
        }
      }
      catch(Exception ex)
      {
        errorMessage = ex.Message;
        return false;
      }
      finally
      {
        if (httpResponse != null)
          httpResponse.Close();
      }


    }

    //
    private static string EncodeJsonChars(string text)
    {
      return text.Replace("\b", "\\\b")
        .Replace("\f", "\\\f")
        .Replace("\n", "\\\n")
        .Replace("\r", "\\\r")
        .Replace("\t", "\\\t")
        .Replace("\"", "\\\"")
        .Replace("\\", "\\\\");
    }


  }
}
