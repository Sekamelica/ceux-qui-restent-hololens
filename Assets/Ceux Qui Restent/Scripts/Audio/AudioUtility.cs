using System;
using AK;
using AK.Wwise;
using System.Collections.Generic;
using UnityEditor;

namespace CeuxQuiRestent.Audio
{
    public class AudioUtility
    {
        public static string GetUniqueID()
        {
            var random = new System.Random();
            DateTime epochStart = new System.DateTime(1970, 1, 1, 8, 0, 0, System.DateTimeKind.Utc);
            double timestamp = (System.DateTime.UtcNow - epochStart).TotalSeconds;
            string uniqueID = String.Format("{0:X}", Convert.ToInt32(timestamp)) + "-" + String.Format("{0:X}", random.Next(1000000000));
            return uniqueID;
        }
    }
}
