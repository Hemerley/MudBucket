using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MudBucket
{
    public class ApplicationSettings
    {
        public required string IPAddress { get; set; }
        public int Port { get; set; }
        public int BufferSize { get; set; }
        public int ConnectionTimeout { get; set; }
    }
}
