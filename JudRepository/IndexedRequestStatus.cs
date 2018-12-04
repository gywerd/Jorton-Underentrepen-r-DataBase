using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JudRepository
{
    public class IndexedRequestStatus : RequestStatus
    {
        int index;

        /// <summary>
        /// Empty Constructor
        /// </summary>
        public IndexedRequestStatus() : base(new RequestStatus())
        {
            this.index = 0;
        }

        /// <summary>
        /// Constructor, to create an Indexable Project Status
        /// </summary>
        /// <param name="index">int</param>
        /// <param name="status">ProjectStatus</param>
        public IndexedRequestStatus(int index, RequestStatus status) : base(status)
        {
            this.index = index;
        }

        public override string ToString()
        {
            return base.ToString();
        }

        public int Index
        {
            get => index;
        }
    }
}
