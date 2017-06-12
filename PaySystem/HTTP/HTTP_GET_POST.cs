using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;


/***
 *  eg:
 *  同步
 *  http.HTTP_GET_POST geturlcode = new http.HTTP_GET_POST();
 *  str=geturlcode.HttpPostMath("http://localhost:8099/Default.aspx", "name2=57&id=123&po=sjs");
 *  str=geturlcode.HttpGetMath("http://localhost:8099/Default.aspx", "name=18");
 * 
 *  textBox.Text += http.HTTP_GET_POST.HttpGetMath("http://localhost:8099/Default.aspx","name=45");
 *  
 * 
 * 异步调用
 *  http.HttpGetMathHandler handler = new http.HttpGetMathHandler(http.HTTP_GET_POST.HttpGetMath);
 *  IAsyncResult result = handler.BeginInvoke("http://localhost:8099/Default.aspx", "name=45", null, null);
 *  //干其他事情....
 *  textBox.Text += (handler.EndInvoke(result));
 * */
namespace PaySystem.HTTP
{
    public delegate string HttpPostMathHandler(string url, string paramsValue); 
    public delegate string HttpGetMathHandler(string url, string paramsValue);

    class HTTP_GET_POST
    {
        

        public static string HttpPostMath(string url, string paramsValue)
        {
            byte[] byteArray = Encoding.UTF8.GetBytes(paramsValue);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = byteArray.Length;
            using (Stream newStream = request.GetRequestStream())
            {
                newStream.Write(byteArray, 0, byteArray.Length);
            }
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            string result = "";
            using (StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
            {
                result = sr.ReadToEnd();
            }
            return result;

        }

        public static string HttpGetMath(string url, string paramsValue)
        {
            string result = string.Empty;
            Uri uri = new Uri(url);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri + "?" + paramsValue);
            request.Method = "Get";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();
            return retString;
        }

    }
}
