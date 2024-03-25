using EGamePlay.Combat;
using ET;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;

namespace AO.Events
{
    public struct BeforeRunEventCmd : ICommand
    {
        public IEventRun EventRun;
    }

    public struct AfterRunEventCmd : ICommand
    {
        public IEventRun EventRun;
    }
}
