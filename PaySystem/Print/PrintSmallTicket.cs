using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaySystem.Print
{
    class PrintSmallTicket
    {
        string Id = "";
        string CarNum = "";
        double Price = 0;
        string InTime = "";
        public  PrintSmallTicket(string Id,string CarNum,double Price,string InTime)
        {
            this.Id = Id;
            this.CarNum = CarNum;
            this.Price = Price;
            this.InTime = InTime;

            System.Windows.Forms.PrintDialog dlg = new System.Windows.Forms.PrintDialog();
            System.Drawing.Printing.PrintDocument docToPrint = new System.Drawing.Printing.PrintDocument();
            docToPrint.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(docToPrint_PrintPage);  
            dlg.Document = docToPrint;

            docToPrint.Print();//PRINT
     
        }

        private void docToPrint_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {

            Font InvoiceFont = new Font("Arial", 9, FontStyle.Bold);
            SolidBrush GrayBrush = new SolidBrush(Color.Black);
            e.Graphics.DrawString(GetPrintStr(), InvoiceFont, GrayBrush, 0, 0);
          
            e.Graphics.Dispose();
        }


        public string GetPrintStr()
        {
            StringBuilder sb = new StringBuilder();
            Id = Id.Substring(0, 30) + "\n     " + Id.Substring(30,Id.Length); 

            sb.Append("                自助停车收费\n");
            sb.Append("-----------------------------------------------------------------------------------------\n\n");

            sb.Append("订单: "+Id+"\n");
            sb.Append("车牌: "+ CarNum + "\n");
            sb.Append("缴费金额: "+ Price.ToString("0.00") +"元\n");
            sb.Append("进场时间: "+ InTime + "\n");

            sb.Append("打印日期: " + DateTime.Now.ToString()+ "\n");
            sb.Append("               欢迎下次光临\n\n");
         
            sb.Append("---------------------------------------------------------------------------------------------\n");
            return sb.ToString();
        }

    }
}
