using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Arcflix.NativeCallsInterfaces
{
    public interface IFacebookPost
    {
        Task<bool> PostToWall(string urlImage, string message, string name, string link, string description);
    }
}
