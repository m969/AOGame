namespace AO
{
    using ET;

    public static partial class ClientReceiveMessages
    {
        public static partial async ETTask M2C_SpellStart(M2C_SpellStart message)
        {
            await ETTask.CompletedTask;
        }

        public static partial async ETTask M2C_SpellEnd(M2C_SpellEnd message)
        {
            await ETTask.CompletedTask;
        }
    }
}