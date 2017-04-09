using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZS1Planv2.Model.SQL
{
    public class SQLService
    {
        private static SQLService _Instance;
        public static SQLService Instance { get => _Instance; private set => _Instance = value; }

        public SQLService()
        {
            if(Instance == null)
                _Instance = this;
        }

        public bool DatabaseExist()
        {
            return true;
        }

        public bool LoadDatabase()
        {
            return false;
        }
    }
}
