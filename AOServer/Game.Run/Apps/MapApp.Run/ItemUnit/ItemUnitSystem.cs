namespace AO
{
    using AO;
    using ET;
    using EGamePlay.Combat;
    using EGamePlay;
    using TComp = AO.ItemUnit;
    using ET.Server;
    using Unity.Mathematics;

    public static partial class ItemUnitSystemSystem
    {
        [ObjectSystem]
        public class AwakeSystemHandler : AwakeSystem<TComp>
        {
            protected override void Awake(TComp self)
            {
                //self.SetMapUnitComponents();
            }
        }
    }
}