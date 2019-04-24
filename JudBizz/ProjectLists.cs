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
        private List<Enterprise> projectEnterprises = new List<Enterprise>();
        private List<RequestShipping> projectRequestDataList = new List<RequestShipping>();
        private List<SubEntrepeneur> projectSubEntrepeneurs = new List<SubEntrepeneur>();
        private List<IttLetterShipping> projectShippings = new List<IttLetterShipping>();

        #endregion

        #region Constructors
        public ProjectLists() { }

        public ProjectLists(List<Enterprise> projectEnterprises, List<RequestShipping> projectRequestDataList, List<SubEntrepeneur> projectSubEntrepeneurs, List<IttLetterShipping> projectShippingList)
        {
            this.projectEnterprises = projectEnterprises;
            this.projectRequestDataList = projectRequestDataList;
            this.projectSubEntrepeneurs = projectSubEntrepeneurs;
            this.projectShippings = projectShippingList;

        }

        #endregion

        #region Properties
        public List<Enterprise> ProjectEnterprises { get => projectEnterprises; set => projectEnterprises = value; }
        public List<RequestShipping> ProjectRequestDataList { get => projectRequestDataList; set => projectRequestDataList = value; }
        public List<SubEntrepeneur> ProjectSubEntrepeneurs { get => projectSubEntrepeneurs; set => projectSubEntrepeneurs = value; }
        public List<IttLetterShipping> ProjectShippings { get => projectShippings; set => projectShippings = value; }

        #endregion
    }
}
