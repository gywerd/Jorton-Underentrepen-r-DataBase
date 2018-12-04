using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JudRepository
{
    public class IndexedContact : Contact
    {
        int index;

        /// <summary>
        /// Empty Constructor
        /// </summary>
        public IndexedContact() : base(new Contact())
        {
            this.index = 0;
        }

        /// <summary>
        /// Constructor, to create an Indexable Request
        /// </summary>
        /// <param name="index">int</param>
        /// <param name="contact">Contact</param>
        public IndexedContact(int index, Contact contact) : base(contact)
        {
            this.index = index;
        }

        public override string ToString()
        {
            return base.ToString();
        }

        public int Index { get => index; }
    }
}
