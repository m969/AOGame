namespace AO
{
    using ET;
    using Unity.VisualScripting.Antlr3.Runtime.Tree;

    public static partial class ClientReceiveMessages
    {
        public static async partial ETTask M2C_CreateUnits(M2C_CreateUnits message)
        {
            Scene currentScene = Avatar.CurrentScene;
            var unitComponent = currentScene.GetComponent<SceneUnitComponent>();

            foreach (UnitInfo unitInfo in message.Units)
            {
                if (unitComponent.Get(unitInfo.UnitId) != null)
                {
                    continue;
                }
			    AOGame.Publish(new AO.EventType.CreateUnit() { Unit = unitInfo, IsMainAvatar = false });
            }
            await ETTask.CompletedTask;
        }

        public static async partial ETTask M2C_CreateMyUnit(M2C_CreateMyUnit message)
        {
			AOGame.Publish(new AO.EventType.CreateUnit() { Unit = message.Unit, IsMainAvatar = true });
            await ETTask.CompletedTask;
        }

        public static async partial ETTask M2C_RemoveUnits(M2C_RemoveUnits message)
        {
            //UnitComponent unitComponent = session.DomainScene().CurrentScene()?.GetComponent<UnitComponent>();
            //if (unitComponent == null)
            //{
            //	return;
            //}
            //foreach (long unitId in message.Units)
            //{
            //	unitComponent.Remove(unitId);
            //}

            await ETTask.CompletedTask;
        }

        public static async partial ETTask M2C_ComponentPropertyNotify(M2C_ComponentPropertyNotify message)
        {
            var unit = Avatar.Main.GetScene().GetComponent<SceneUnitComponent>().Get(message.UnitId);
            foreach (var kv in unit.Components)
            {
                if (kv.Key.Name == message.ComponentName)
                {
                    var property = kv.Key.GetProperty(message.PropertyName);
                    var value = ProtobufHelper.Deserialize(property.PropertyType, message.PropertyBytes, 0, message.PropertyBytes.Length);
                    property.SetValue(kv.Value, value);
                    Log.Debug($"{unit.GetType().Name} {property.Name} {value}");
                    break;
                }
            }
            await ETTask.CompletedTask;
        }
    }
}