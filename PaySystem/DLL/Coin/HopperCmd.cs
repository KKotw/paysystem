﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaySystem.DLL.Coin
{
    class HopperCmd
    {
        //收币检测是否在线
        public static byte[] GetCoinOnLine_Send = new byte[] { 0x02,0x00,0x01,0xfe,0xff};
        public static byte[] GetCoinOnLine_Get = new byte[] { 0x02, 0x00, 0x01, 0xfe, 0xff, 0x01,0x00,0x02,0x00,0xFD };
        //收币自检
        public static byte[] GetCoinSelfCheck_Send = new byte[] { 0x02, 0x00, 0x01, 0xe8, 0x15 };
        public static byte[] GetCoinSelfCheck_Get = new byte[] { 0x02, 0x00, 0x01, 0xe8, 0x15 , 0x01,0x01,0x02,0x00,0x00,0xFC };
        //收币复位
        public static byte[] GetCoinReset_Send = new byte[] { 0x02, 0x00, 0x01, 0x01, 0xfc };
        public static byte[] GetCoinReset_Get = new byte[] { 0x02, 0x00, 0x01, 0x01, 0xfc, 0x01, 0x00, 0x02, 0x00, 0xFD };
        //收币打开一元通道
        public static byte[] GetCoinOpenChannelOne_Send = new byte[] { 0x02, 0x02, 0x01, 0xe7, 0x01,0x00,0x13 };
        public static byte[] GetCoinOpenChannelOne_Get = new byte[] { 0x02, 0x02, 0x01, 0xe7, 0x01, 0x00, 0x13, 0x01, 0x00, 0x02, 0x00, 0xFD };
        //收币关闭所有通道
        public static byte[] GetCoinCloseChannelALL_Send = new byte[] { 0x02, 0x02, 0x01, 0xe7, 0x00, 0x00, 0x14 };
        public static byte[] GetCoinCloseChannelALL_Get = new byte[] { 0x02, 0x02, 0x01, 0xe7, 0x00, 0x00, 0x14, 0x01, 0x00, 0x02, 0x00, 0xFD };
        //接收硬币，循环poll（200ms）
        public static byte[] GetCoinReciveCoin_Send = new byte[] { 0x02,0x00,0x01,0xE5,0x18 };



        //收币检测是否在线
        public static byte[] PayoutCoinOnLine_Send = new byte[] { 0x03, 0x00, 0x01, 0xfe, 0xfe };
        public static byte[] PayoutCoinOnLine_Get = new byte[] { 0x03, 0x00, 0x01, 0xfe, 0xfe, 0x01, 0x00, 0x03, 0x00, 0xFC };

        //检查HOPPER里还有多少硬币，只能模糊判断
        public static byte[] PayoutCoinCheck_Send = new byte[] { 0x03,0x00,0x01,0xD9,0x23 };
        //打开出币功能
        public static byte[] PayoutCoinEnable_Send = new byte[] { 0x03,0x01,0x01,0xA4,0xA5,0xB2 };
        public static byte[] PayoutCoinEnable_Get = new byte[] { 0x03, 0x01, 0x01, 0xA4, 0xA5, 0xB2  ,0x01 ,0x00,0x03,0x00,0xFC };
        //复位
        public static byte[] PayoutCoinReset_Send = new byte[] { 0x03,0x00,0x01,0x01,0xFB };
        public static byte[] PayoutCoinReset_Get = new byte[] { 0x03, 0x00, 0x01, 0x01, 0xFB, 0x01, 0x00, 0x03, 0x00, 0xFC };

        //鸡肋功能 但是出币前必须发
        public static byte[] PayoutCoinRequestCipher_Send = new byte[] { 0x03,0x00,0x01,0xA0,0x5C };

        //返回出币状态
        public static byte[] RequestPayoutCoinStatus_Send = new byte[] { 0x03, 0x00, 0x01, 0xA6, 0x56 };


        public static bool CheckEquals(byte[] byte1,byte[] byte2) // common
        {
            IStructuralEquatable temp = byte1;
            return (temp.Equals(byte2, StructuralComparisons.StructuralEqualityComparer));               
        }
    }
}
