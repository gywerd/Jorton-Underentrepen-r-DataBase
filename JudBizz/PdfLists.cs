using JudRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JudBizz
{
    public class PdfLists
    {
        #region Fields
        private List<Enterprise> enterprises = new List<Enterprise>();
        private List<SubEntrepeneur> subEntrepeneurs = new List<SubEntrepeneur>();
        private List<Shipping> shippingList = new List<Shipping>();

        #endregion

        #region Constructors
        public PdfLists() { }

        public PdfLists(List<Enterprise> enterprises, List<SubEntrepeneur> subEntrepeneurs, List<Shipping> shippingList)
        {
            this.enterprises = enterprises;
            this.subEntrepeneurs = subEntrepeneurs;
            this.shippingList = shippingList;

        }

        #endregion

        #region Properties
        public List<Enterprise> Enterprises { get => enterprises; set => enterprises = value; }
        public List<SubEntrepeneur> SubEntrepeneurs { get => subEntrepeneurs; set => subEntrepeneurs = value; }
        public List<Shipping> ShippingList { get => shippingList; set => shippingList = value; }

        #endregion
    }
}
