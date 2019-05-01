using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UkZipApi
{
    public class RootObject
    {
        #region Fields
        private int _status = 0;
        private bool _result = false;

        #endregion

        #region Properties
        public int status { get => _status; set => _status = value; }
        public bool result { get => _result; set => _result = value; }

        #endregion
    }
}
