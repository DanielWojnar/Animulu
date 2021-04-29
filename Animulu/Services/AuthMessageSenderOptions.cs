using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Animulu.Services
{
    public class AuthMessageSenderOptions
    {
        //set them as user-secret using secret-manager tool
        //dotnet user-secrets set SendGridUser <user>
        //dotnet user-secrets set SendGridKey <key>
        public string SendGridUser { get; set; }
        public string SendGridKey { get; set; }
    }
}
