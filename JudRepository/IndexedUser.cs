using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JudRepository
{
    public class IndexedUser : User
    {
        int index;

        /// <summary>
        /// Empty Constructor
        /// </summary>
        public IndexedUser() : base(new User())
        {
            this.index = 0;
        }

        /// <summary>
        /// Constructor, to create an Indexable EnterpriseForm
        /// </summary>
        /// <param name="index">int</param>
        /// <param name="user">User</param>
        public IndexedUser(int index, User user) : base(user)
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
