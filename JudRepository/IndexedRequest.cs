using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JudRepository
{
    public class IndexedRequest : Request
    {
        int index;

        /// <summary>
        /// Empty Constructor
        /// </summary>
        public IndexedRequest() : base(new Request())
        {
            this.index = 0;
        }

        /// <summary>
        /// Constructor, to create an Indexable Request
        /// </summary>
        /// <param name="index">int</param>
        /// <param name="request">Request</param>
        public IndexedRequest(int index, Request request) : base(request)
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
