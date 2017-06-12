using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaySystem.HTTP
{

    public class DataItem
    {
        /// <summary>
        /// 
        /// </summary>
        public int cashCost { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int totalCost { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string tradeNo { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string paymentType { get; set; }
        /// <summary>
        /// 粤B5YY77
        /// </summary>
        public string carNum { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string parkingRecordId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string paymentStatus { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string payFinishTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int onlineCost { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string paymentMethod { get; set; }
    }

    public class jsonClass3
    {
        /// <summary>
        /// 
        /// </summary>
        public int total { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<DataItem> data { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string resultCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string resultMsg { get; set; }
    }

  
}
