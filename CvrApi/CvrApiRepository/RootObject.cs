using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CvrApiRepository
{
    public class RootObject
    {
        #region Fields
        private int _vat;
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
        private int _companycode;
        private string _companydesc;
        private string _creditstartdate;
        private bool _creditbankrupt;
        private string _creditstatus;
        private object _owners;
        private List<Productionunit> _productionunits;
        private int _t;
        private int _version;
        #endregion

        #region
        public RootObject() { }

        #endregion

        #region Properties
        public int vat { get => _vat; set => _vat = value; }
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
        public int companycode { get => _companycode; set => _companycode = value; }
        public string companydesc { get => _companydesc; set => _companydesc = value; }
        public string creditstartdate { get => _creditstartdate; set => _creditstartdate = value; }
        public bool creditbankrupt { get => _creditbankrupt; set => _creditbankrupt = value; }
        public string creditstatus { get => _creditstatus; set => _creditstatus = value; }
        public object owners { get => _owners; set => _owners = value; }
        public List<Productionunit> productionunits { get => _productionunits; set => _productionunits = value; }
        public int t { get => _t; set => _t = value; }
        public int version { get => _version; set => _version = value; }
        #endregion

    }
}
