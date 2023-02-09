namespace ET
{
    public static class SceneHelper
    {
        public static int DomainZone(this Entity entity)
        {
            return ((IDomain) entity.Domain)?.DomainIndex ?? 0;
        }

        //public static AO.IApp DomainScene(this Entity entity)
        //{
        //    return (AO.IApp) entity.Domain;
        //}
    }
}