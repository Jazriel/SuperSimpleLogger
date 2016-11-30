using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logging
{
    public class Log
    {
        public int id;
        public String userName;
        public DateTime dateTime;
        public String area;
        public String type;

        public Log(int _id, String _userName, DateTime _dateTime, String _area, String _type)
        {
            id = _id;
            userName = _userName;
            dateTime = _dateTime;
            area = _area;
            type = _type; 
        }

        public Log(Object _id, Object _userName, Object _dateTime, Object _area, Object _type)
        {
            id = Convert.ToInt32(_id);
            userName = Convert.ToString(_userName).TrimEnd();
            dateTime = Convert.ToDateTime(_dateTime);
            area = Convert.ToString(_area).TrimEnd();
            type = Convert.ToString(_type).TrimEnd();
        }

        override
        public String ToString()
        {
            return userName + " " + dateTime.ToString() + " " + area + " " + type;
        }
    }
}
