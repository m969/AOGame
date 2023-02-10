namespace AO
{
    using ET;

    public static partial class ClientReceiveMessages
    {
        public static async partial ETTask M2C_StartSceneChange(M2C_StartSceneChange message)
        {
            await ETTask.CompletedTask;
        }
    }
}