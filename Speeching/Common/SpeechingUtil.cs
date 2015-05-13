using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Speeching.Common
{
    public class SpeechingUtil
    {
        public static int ParseInt(object obj)
        {
            int i;
            try{
                i = Convert.ToInt32(obj);
            }
            catch
            {
                i = -1;
            }
            return i;
        }
    }
}