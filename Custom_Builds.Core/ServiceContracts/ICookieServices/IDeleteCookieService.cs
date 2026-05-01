using Custom_Builds.Core.Models;
using System;

namespace Custom_Builds.Core.ServiceContracts.ICookieServices
{
    public interface IDeleteCookieService
    {
        Result Delete(string key);
    }
}
