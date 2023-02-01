namespace AO
{
    using ET;

    public static partial class ClientReceiveMessages
    {
        [MessageHandler(SceneType.Client)]
        public class M2C_CreateUnitsHandler : AMHandler<M2C_CreateUnits>
        {
            protected override async ETTask Run(M2C_CreateUnits message)
            {
                M2C_CreateUnits(message).Coroutine();
                await ETTask.CompletedTask;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public static partial ETTask M2C_CreateUnits(M2C_CreateUnits message);

        [MessageHandler(SceneType.Client)]
        public class M2C_CreateMyUnitHandler : AMHandler<M2C_CreateMyUnit>
        {
            protected override async ETTask Run(M2C_CreateMyUnit message)
            {
                M2C_CreateMyUnit(message).Coroutine();
                await ETTask.CompletedTask;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public static partial ETTask M2C_CreateMyUnit(M2C_CreateMyUnit message);

        [MessageHandler(SceneType.Client)]
        public class M2C_StartSceneChangeHandler : AMHandler<M2C_StartSceneChange>
        {
            protected override async ETTask Run(M2C_StartSceneChange message)
            {
                M2C_StartSceneChange(message).Coroutine();
                await ETTask.CompletedTask;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public static partial ETTask M2C_StartSceneChange(M2C_StartSceneChange message);

        [MessageHandler(SceneType.Client)]
        public class M2C_RemoveUnitsHandler : AMHandler<M2C_RemoveUnits>
        {
            protected override async ETTask Run(M2C_RemoveUnits message)
            {
                M2C_RemoveUnits(message).Coroutine();
                await ETTask.CompletedTask;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public static partial ETTask M2C_RemoveUnits(M2C_RemoveUnits message);

        [MessageHandler(SceneType.Client)]
        public class M2C_PathfindingResultHandler : AMHandler<M2C_PathfindingResult>
        {
            protected override async ETTask Run(M2C_PathfindingResult message)
            {
                M2C_PathfindingResult(message).Coroutine();
                await ETTask.CompletedTask;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public static partial ETTask M2C_PathfindingResult(M2C_PathfindingResult message);

        [MessageHandler(SceneType.Client)]
        public class M2C_StopHandler : AMHandler<M2C_Stop>
        {
            protected override async ETTask Run(M2C_Stop message)
            {
                M2C_Stop(message).Coroutine();
                await ETTask.CompletedTask;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public static partial ETTask M2C_Stop(M2C_Stop message);


    }
}