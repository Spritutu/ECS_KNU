using ECS.Common.Helper;
using INNO6.Core.Manager;
using INNO6.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECS.Application
{
    public class DataChangedEvent
    {
        public static void DataManager_DataChangedEvent(object sender, DataChangedEventHandlerArgs args)
        {
            switch(args.Data.Name)
            {

            }
        }
    }
}
