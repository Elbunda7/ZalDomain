﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZalDomain.ActiveRecords
{
    public interface IActualityItem:ISimpleItem
    {
        string Type { get; }

        string GetShortText(int length);
    }
}
