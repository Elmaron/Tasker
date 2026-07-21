using System;
using System.Collections.Generic;
using System.Text;
using Tasker.Classes.Data.Conversion.Tables;
using Tasker.Classes.Data.Retrieval;

namespace Tasker.Classes.Templates
{
    public abstract class ObservableObjectTemplate : ObservableTableTemplate
    {
        private InternalData _data;
        public InternalData Data => _data;

        public ObservableObjectTemplate(int pId, InternalData pData) : base(pId)
        {
            _data = pData;
        }
    }
}
