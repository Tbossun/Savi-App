﻿using System;
using System.Collections.Generic;
using System.Text;

namespace SavingsApp.Data.Repositories.IRepositories
{
    public interface IUnitOfWork
    {
        IKycRepository KycRepository { get; }

        void Save();
    }
}
