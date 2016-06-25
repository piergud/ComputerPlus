using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Rage.Forms;

namespace ComputerPlus.API
{
    public sealed class ExternalUI
    {
        public Guid Identifier {
            get;
            internal set;
        }

        public String DisplayName
        {
            get;
            internal set;
        }

        public String Author
        {
            get;
            internal set;
        }

        internal Func<GwenForm> Creator
        {
            get;
            private set;
        }   
        
        internal Action OnOpen
        {
            get;
            private set;
        }         
        
        internal ExternalUI(Guid identifier, String displayName, String author, Func<GwenForm> creator, Action onOpen = null)
        {
            this.Identifier = identifier;
            this.DisplayName = displayName;
            this.Author = author;
            this.Creator = creator;
            this.OnOpen = onOpen;
        }
    }
}
