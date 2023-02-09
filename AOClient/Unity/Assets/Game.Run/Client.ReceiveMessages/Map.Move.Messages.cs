namespace AO
{
    using ET;

    public static partial class ClientReceiveMessages
    {
        public static async partial ETTask M2C_PathfindingResult(M2C_PathfindingResult message)
        {
            var unit = Avatar.CurrentScene.GetComponent<SceneUnitComponent>().Get(message.Id).MapUnit();
            unit.MoveToAsync(message.Points[0]).Coroutine();
            await ETTask.CompletedTask;
        }

        public static async partial ETTask M2C_Stop(M2C_Stop message)
        {
            await ETTask.CompletedTask;
        }
    }
}