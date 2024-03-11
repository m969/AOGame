namespace AO
{
    using ET;
    using Unity.Mathematics;
    using UnityEngine;
    using System;

    [Event(SceneType.Process)]
    public class PropertyChanged_UpdateInfo : AEvent<EventType.PropertyChangedEvent>
    {
        protected override async ETTask Run(Entity source, EventType.PropertyChangedEvent args)
        {
            //if (source is UnitLevelComponent UnitLevelComponent) UnitLevelComponentSystem.Level_Changed(UnitLevelComponent);
            var type = args.Instance.GetType();
            var typeName = type.FullName;
            var systemTypeName = typeName + "System";
            var systemType = Type.GetType(systemTypeName);
            var changedMethodName = args.PropertyName + "_Changed";
            Log.Debug($"PropertyChanged_UpdateInfo {systemTypeName} {changedMethodName} systemType={systemType != null}");
            if (systemType != null)
            {
                var changedMethod = systemType.GetMethod(changedMethodName, System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);
                Log.Debug($"PropertyChanged_UpdateInfo {systemTypeName} {changedMethodName} changedMethod={changedMethod != null}");
                if (changedMethod != null)
                {
                    changedMethod.Invoke(null, new object[] { args.Instance });
                }
            }

            AOGame.ClientApp.JsCallback?.Invoke(type.Name + "_" + changedMethodName, args.Instance);

            await ETTask.CompletedTask;
        }
    }
}