using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThietBiYeuThuong.Data.Utilities
{
    public static class GetNextId
    {
        public static string NextID(string lastID, string prefixID)
        {
            try
            {
                if (lastID == "")
                {
                    return prefixID + "0001";
                }
                int nextID = int.Parse(lastID.Remove(0, prefixID.Length)) + 1;
                int lengthNumerID = lastID.Length - prefixID.Length;
                string zeroNumber = "";
                for (int i = 1; i <= lengthNumerID; i++)
                {
                    if (nextID < Math.Pow(10, i))
                    {
                        for (int j = 1; j <= lengthNumerID - i; i++)
                        {
                            zeroNumber += "0";
                        }
                        return prefixID + zeroNumber + nextID.ToString();
                    }
                }
                return prefixID + nextID;
            }
            catch (Exception ex)
            {
                //return "";
                throw ex;
            }
        }

        public static string NextID_BenhNhan(string lastID, string prefixID)
        {
            try
            {
                if (lastID == "")
                {
                    return prefixID + "000001";
                }
                int nextID = int.Parse(lastID.Remove(0, prefixID.Length)) + 1;
                int lengthNumerID = lastID.Length - prefixID.Length;
                string zeroNumber = "";
                for (int i = 1; i <= lengthNumerID; i++)
                {
                    if (nextID < Math.Pow(10, i))
                    {
                        for (int j = 1; j <= lengthNumerID - i; i++)
                        {
                            zeroNumber += "0";
                        }
                        return prefixID + zeroNumber + nextID.ToString();
                    }
                }
                return prefixID + nextID;
            }
            catch (Exception ex)
            {
                //return "";
                throw ex;
            }
        }

        public static string NextKHId(string lastID, string prefixID, string length)
        {
            if (lastID == "")
            {
                return prefixID + length;
            }
            int nextID = int.Parse(lastID.Remove(0, prefixID.Length)) + 1;
            int lengthNumerID = lastID.Length - prefixID.Length;
            string zeroNumber = "";
            for (int i = 1; i <= lengthNumerID; i++)
            {
                if (nextID < Math.Pow(10, i))
                {
                    for (int j = 1; j <= lengthNumerID - i; i++)
                    {
                        zeroNumber += "0";
                    }
                    return prefixID + zeroNumber + nextID.ToString();
                }
            }
            return prefixID + nextID;
        }
    }
}