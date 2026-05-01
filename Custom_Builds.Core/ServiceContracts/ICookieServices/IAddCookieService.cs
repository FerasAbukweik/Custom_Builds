using Custom_Builds.Core.Models;
using System;

namespace Custom_Builds.Core.ServiceContracts.CookieServices
{
    public interface IAddCookieService
    {
        Result Add(string key, string value, double lifeTime);
    }
}
