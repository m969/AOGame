namespace AO
{
    using ET;

    public static partial class ClientReceiveMessages
    {
        public static partial async ETTask M2C_SpellCastStart(M2C_SpellCastStart message)
        {
            await ETTask.CompletedTask;
        }

        public static partial async ETTask M2C_SpellCastEnd(M2C_SpellCastEnd message)
        {
            await ETTask.CompletedTask;
        }
    }
}