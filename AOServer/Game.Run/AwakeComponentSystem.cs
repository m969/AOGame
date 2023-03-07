namespace AO
{
    using AO;
    using ET;
    using EGamePlay.Combat;
    using EGamePlay;
    using TComp = AO.AwakeComponent;
    using ET.Server;
    using Unity.Mathematics;
    using SharpCompress.Common;

    public static partial class AwakeComponentSystem
    {
        [ObjectSystem]
        public class AwakeHandler : AwakeSystem<TComp>
        {
            protected override void Awake(TComp self)
            {

            }
        }
    }
}