﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessServices
{
    public interface IUserService
    {
        int Authenticate(string userName, string password);
    }
}
