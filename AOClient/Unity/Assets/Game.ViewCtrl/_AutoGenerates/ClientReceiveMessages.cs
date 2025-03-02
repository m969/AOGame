namespace AO
{
    using ET;

    public static partial class ClientReceiveMessages
    {
        [MessageHandler(SceneType.Client)]
        public class M2C_OnEnterMapHandler : AMHandler<M2C_OnEnterMap>
        {
            protected override async ETTask Run(M2C_OnEnterMap message)
            {
                M2C_OnEnterMap(message).Coroutine();
                await ETTask.CompletedTask;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public static partial ETTask M2C_OnEnterMap(M2C_OnEnterMap message);

        [MessageHandler(SceneType.Client)]
        public class M2C_OnLeaveMapHandler : AMHandler<M2C_OnLeaveMap>
        {
            protected override async ETTask Run(M2C_OnLeaveMap message)
            {
                M2C_OnLeaveMap(message).Coroutine();
                await ETTask.CompletedTask;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public static partial ETTask M2C_OnLeaveMap(M2C_OnLeaveMap message);

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
        public class M2C_ComponentPropertyNotifyHandler : AMHandler<M2C_ComponentPropertyNotify>
        {
            protected override async ETTask Run(M2C_ComponentPropertyNotify message)
            {
                M2C_ComponentPropertyNotify(message).Coroutine();
                await ETTask.CompletedTask;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public static partial ETTask M2C_ComponentPropertyNotify(M2C_ComponentPropertyNotify message);

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

        [MessageHandler(SceneType.Client)]
        public class M2C_SpellStartHandler : AMHandler<M2C_SpellStart>
        {
            protected override async ETTask Run(M2C_SpellStart message)
            {
                M2C_SpellStart(message).Coroutine();
                await ETTask.CompletedTask;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public static partial ETTask M2C_SpellStart(M2C_SpellStart message);

        [MessageHandler(SceneType.Client)]
        public class M2C_SpellStepHandler : AMHandler<M2C_SpellStep>
        {
            protected override async ETTask Run(M2C_SpellStep message)
            {
                M2C_SpellStep(message).Coroutine();
                await ETTask.CompletedTask;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public static partial ETTask M2C_SpellStep(M2C_SpellStep message);

        [MessageHandler(SceneType.Client)]
        public class M2C_SpellEndHandler : AMHandler<M2C_SpellEnd>
        {
            protected override async ETTask Run(M2C_SpellEnd message)
            {
                M2C_SpellEnd(message).Coroutine();
                await ETTask.CompletedTask;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public static partial ETTask M2C_SpellEnd(M2C_SpellEnd message);


    }
}