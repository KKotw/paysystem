using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaySystem.LOG
{
     
    class LogSerial
    {
        //new Log.LogClass().WriteLogFile("serial:" + Log.LogSerial.logSerialnum + "  " + "写入的信息");
        public static string logSerialnum;

        public void exchangeLogSerialNum()
        {
            Random ran = new Random();
            int RandKey = ran.Next(100, 999);
            logSerialnum = RandKey.ToString();

        }

       
    }
}
