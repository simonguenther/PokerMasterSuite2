using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerMasterSuite2
{
    /// <summary>
    /// Extension of BackgroundWorker, only adds an option to store ScanButton it belongs to
    /// </summary>
    class BackgroundSeatingWorker : BackgroundWorker
    {
        public ScanButton scanButton { get; set; }
    }
}
