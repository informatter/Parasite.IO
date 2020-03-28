using System;
using System.Collections.Generic;


namespace ParasiteIO.Core.Data.ReceiveDataFromParasite
{
    public interface IReceiveData
    {
        List<object> ReceiveData(string ID, double tolerance);
    }
}