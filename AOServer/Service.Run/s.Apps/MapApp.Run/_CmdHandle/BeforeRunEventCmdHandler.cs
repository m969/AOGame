using AO.Events;
using ET;
using ET.EventType;
using ET.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AO
{
    [Command]
    public class BeforeRunEventCmdHandler : ACommandHandler<BeforeRunEventCmd>
    {
        protected override async ETTask Handle(BeforeRunEventCmd cmd)
        {
            Log.Console($"BeforeRunEventCmdHandler {cmd.EventRun}");
            await ETTask.CompletedTask;
        }
    }
}
