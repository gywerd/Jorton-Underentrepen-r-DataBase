using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CvrApiRepository
{
    public class Productionunit
    {
        #region Fields
        private int _pno;
        private bool _main;
        private string _name;
        private string _address;
        private int _zipcode;
        private string _city;
        private string _cityname;
        private bool _protected;
        private int? _phone;
        private string _email;
        private int? _fax;
        private string _startdate;
        private string _enddate;
        private string _employees;
        private string _addressco;
        private int _industrycode;
        private string _industrydesc;
        #endregion

        #region Constructors
        public Productionunit() { }

        #endregion

        #region Properties
        public int pno { get => _pno; set => _pno = value; }
        public bool main { get => _main; set => _main = value; }
        public string name { get => _name; set => _name = value; }
        public string address { get => _address; set => _address = value; }
        public int zipcode { get => _zipcode; set => _zipcode = value; }
        public string city { get => _city; set => _city = value; }
        public string cityname { get => _cityname; set => _cityname = value; }
        public bool @protected { get => _protected; set => _protected = value; }
        public int? phone { get => _phone; set => _phone = value; }
        public string email { get => _email; set => _email = value; }
        public int? fax { get => _fax; set => _fax = value; }
        public string startdate { get => _startdate; set => _startdate = value; }
        public string enddate { get => _enddate; set => _enddate = value; }
        public string employees { get => _employees; set => _employees = value; }
        public string addressco { get => _addressco; set => _addressco = value; }
        public int industrycode { get => _industrycode; set => _industrycode = value; }
        public string industrydesc { get => _industrydesc; set => _industrydesc = value; }
        #endregion

    }

}
