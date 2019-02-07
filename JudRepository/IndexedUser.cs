using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JudRepository
{
    public class IndexedUser : User
    {
        #region Field
        int index;

        #endregion

        #region Constructors
        /// <summary>
        /// Empty Constructor
        /// </summary>
        public IndexedUser() : base(new User())
        {
            this.index = 0;
        }

        /// <summary>
        /// Constructor to add a new Indexed EnterpriseForm
        /// </summary>
        /// <param name="index">int</param>
        /// <param name="user">User</param>
        public IndexedUser(int index, User user) : base(user)
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
