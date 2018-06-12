﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZalApiGateway.Models
{
    public interface IModel
    {
        int Id { get; set; }

        IModel Copy();
    }
}
