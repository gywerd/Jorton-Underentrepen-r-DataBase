using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JudRepository
{
    public class IndexedRequestStatus : RequestStatus
    {
        #region Fields
        int index;

        #endregion

        #region Constructors
        /// <summary>
        /// Empty Constructor
        /// </summary>
        public IndexedRequestStatus() : base(new RequestStatus())
        {
            this.index = 0;
        }

        /// <summary>
        /// Constructor to add a new Indexed Project Status
        /// </summary>
        /// <param name="index">int</param>
        /// <param name="status">ProjectStatus</param>
        public IndexedRequestStatus(int index, RequestStatus status) : base(status)
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
        public int Index { get => index; }

        #endregion

    }
}
