using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JudRepository
{
    public class IndexedRequest : Request
    {
        #region Fields
        int index;

        #endregion

        #region Constructors
        /// <summary>
        /// Empty Constructor
        /// </summary>
        public IndexedRequest() : base(new Request())
        {
            this.index = 0;
        }

        /// <summary>
        /// Constructor to add a new Indexed Request
        /// </summary>
        /// <param name="index">int</param>
        /// <param name="request">Request</param>
        public IndexedRequest(int index, Request request) : base(request)
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
