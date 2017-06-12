using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaySystem.HTTP
{
    public class RowsItem
    {
        /// <summary>
        /// 
        /// </summary>
        public int limit { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int offset { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 浙SYY496
        /// </summary>
        public string carNum { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public object carNumTemp { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public object cardNo { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public object cardStatus { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public object waitCardNo { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public object carType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public object userId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public object userPhone { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public object buildingId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public object buildingName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public object parkId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public object parkName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public object inTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string inTimeStr { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public object outTimeStr { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public object outTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public object status { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public object constStatus { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public object userType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public object address { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public object monthCardEndTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public object charge { get; set; }
        /// <summary>
        /// /ftpdownload/2017-04-13/192.168.1.10_in/20170413095812_浙SYY496.jpg
        /// </summary>
        public string enterSnapPath { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string exitSnapPath { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<object> paymentRecordList { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<object> parkIdList { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public object couponValue { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public object couponTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public object couponId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public object fee { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public object payedFee { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public object deptFee { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string myStatus { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string type { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public object costDetail { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public object timeType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public object beginTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public object endTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public object securityLock { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public object autoOpen { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public object enterOpen { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public object enterTip { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public object latestPaymentId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public object outCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public object storageCardBalance { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public object desc { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public object balanceLimit { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public object parkSpaceFull { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public object storageCard { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public object cardInfo { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public object coupon { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public object enterImg { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public object exitImg { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public object outType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public object freeOutReason { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public object sysUserId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public object realOutTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public object validMonthCard { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public object needPay { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public object monthCard { get; set; }
    }

    public class JsonClass
    {
        /// <summary>
        /// 
        /// </summary>
        public int total { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<RowsItem> rows { get; set; }
    }
    


}
