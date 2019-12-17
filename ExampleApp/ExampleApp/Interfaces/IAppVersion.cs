using System;
using System.Collections.Generic;
using System.Text;

namespace ExampleApp.Interfaces
{
    public interface IAppVersion
    {
        string GetVersion();
        int GetBuild();
    }
}