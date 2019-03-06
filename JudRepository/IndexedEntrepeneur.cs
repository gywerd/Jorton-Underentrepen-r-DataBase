using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JudRepository
{
    public class IndexedEntrepeneur : Entrepeneur
    {
        #region Fields
        int index;

        #endregion

        #region Constructors
        /// <summary>
        /// Empty Constructor
        /// </summary>
        public IndexedEntrepeneur() : base(new Entrepeneur())
        {
            this.index = 0;
        }

        /// <summary>
        /// Constructor to add a new Indexed Legal Entity
        /// </summary>
        /// <param name="index">int</param>
        /// <param name="entrepeneur">Entrepeneur</param>
        public IndexedEntrepeneur(int index, Entrepeneur entrepeneur) : base(entrepeneur)
        {
            this.index = index;
        }

        #endregion

        #region Methods
        public override string ToString()
        {
            return base.ToString();
        }

        #endregion

        #region Properties
        public int Index
        {
            get => index;
        }

        #endregion
    }
}
