using JudRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JudBizz
{
    public class ProjectLists
    {
        #region Fields
        private List<Bullet> bullets = new List<Bullet>();
        private List<Enterprise> enterprises = new List<Enterprise>();
        private List<IttLetterShipping> ittLetterShippings = new List<IttLetterShipping>();
        private List<Paragraf> paragrafs = new List<Paragraf>();
        private List<RequestShipping> requestShippings = new List<RequestShipping>();
        private List<SubEntrepeneur> subEntrepeneurs = new List<SubEntrepeneur>();

        #endregion

        #region Constructors
        public ProjectLists() { }

        public ProjectLists(List<Enterprise> projectEnterprises, List<RequestShipping> projectRequestDataList, List<SubEntrepeneur> projectSubEntrepeneurs, List<IttLetterShipping> projectShippingList)
        {
            this.enterprises = projectEnterprises;
            this.requestShippings = projectRequestDataList;
            this.subEntrepeneurs = projectSubEntrepeneurs;
            this.ittLetterShippings = projectShippingList;

        }

        #endregion

        #region Properties
        public List<Bullet> Bullets { get => bullets; set => bullets = value; }
        public List<Enterprise> Enterprises { get => enterprises; set => enterprises = value; }
        public List<IttLetterShipping> IttLetterShippings { get => ittLetterShippings; set => ittLetterShippings = value; }
        public List<Paragraf> Paragrafs { get => paragrafs; set => paragrafs = value; }
        public List<RequestShipping> RequestShippings { get => requestShippings; set => requestShippings = value; }
        public List<SubEntrepeneur> SubEntrepeneurs { get => subEntrepeneurs; set => subEntrepeneurs = value; }

        #endregion
    }
}
