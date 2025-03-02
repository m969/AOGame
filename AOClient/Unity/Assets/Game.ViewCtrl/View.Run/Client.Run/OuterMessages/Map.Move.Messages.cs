namespace AO
{
    using ET;

    public static partial class ClientReceiveMessages
    {
        public static async partial ETTask M2C_PathfindingResult(M2C_PathfindingResult message)
        {
            var unit = Scene.CurrentScene.GetComponent<SceneUnitComponent>().Get(message.Id).MapUnit();
            unit.MovePathAsync(message.Points.ToArray()).Coroutine();
            await ETTask.CompletedTask;
        }

        public static async partial ETTask M2C_Stop(M2C_Stop message)
        {
            await ETTask.CompletedTask;
        }
    }
}