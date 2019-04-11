using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JudRepository
{
    public class UpdateData
    {
        #region Fields
        private bool active = false;
        private LegalEntity entity = new LegalEntity();
        #endregion

        #region Properties
        public bool Active
        {
            get { return active; }
            set
            {
                try
                {
                    active = value;
                }
                catch (Exception)
                {
                    active = false;
                }
            }
        }

        public LegalEntity Entity
        {
            get { return entity; }
            set
            {
                try
                {
                    entity = value;
                }
                catch (Exception)
                {
                    entity = new LegalEntity();
                }
            }
        }

        #endregion

    }
}
