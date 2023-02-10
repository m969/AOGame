namespace AO
{
    using ET;

    public static partial class ClientReceiveMessages
    {
        public static partial async ETTask M2C_SpellStart(M2C_SpellStart message)
        {
            Log.Debug("M2C_SpellStart");
            var unit = Scene.CurrentScene.GetComponent<SceneUnitComponent>().Get(message.UnitId);
            await ETTask.CompletedTask;
        }

        public static partial async ETTask M2C_SpellEnd(M2C_SpellEnd message)
        {
            Log.Debug("M2C_SpellEnd");
            await ETTask.CompletedTask;
        }
    }
}