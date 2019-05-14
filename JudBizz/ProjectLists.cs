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
        private List<Paragraf> paragrafs = new List<Paragraf>();
        private List<Shipping> shippings = new List<Shipping>();
        private List<SubEntrepeneur> subEntrepeneurs = new List<SubEntrepeneur>();

        #endregion

        #region Constructors
        /// <summary>
        /// Empty constructor
        /// </summary>
        public ProjectLists() { }

        /// <summary>
        /// Constructor used to create new ProjectLists from existing lists
        /// </summary>
        /// <param name="bullets">List<Bullet></param>
        /// <param name="enterprises">List<Bullet></param>
        /// <param name="paragrafs">List<Paragraf></param>
        /// <param name="shippings">List<Shipping></param>
        /// <param name="subEntrepeneurs">List<SubEntrepeneur></param>
        public ProjectLists(List<Bullet> bullets, List<Enterprise> enterprises, List<Paragraf> paragrafs, List<Shipping> shippings, List<SubEntrepeneur> subEntrepeneurs)
        {
            this.bullets = bullets;
            this.enterprises = enterprises;
            this.paragrafs = paragrafs;
            this.shippings = shippings;
            this.subEntrepeneurs = subEntrepeneurs;

        }

        #endregion

        #region Properties
        public List<Bullet> Bullets { get => bullets; set => bullets = value; }
        public List<Enterprise> Enterprises { get => enterprises; set => enterprises = value; }
        public List<Paragraf> Paragrafs { get => paragrafs; set => paragrafs = value; }
        public List<Shipping> Shippings { get => shippings; set => shippings = value; }
        public List<SubEntrepeneur> SubEntrepeneurs { get => subEntrepeneurs; set => subEntrepeneurs = value; }

        #endregion
    }
}
