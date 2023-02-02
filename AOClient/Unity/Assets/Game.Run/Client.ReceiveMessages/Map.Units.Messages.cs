namespace AO
{
    using ET;

    public static partial class ClientReceiveMessages
    {
        public static async partial ETTask M2C_CreateUnits(M2C_CreateUnits message)
        {
            //Scene currentScene = session.DomainScene().CurrentScene();
            //UnitComponent unitComponent = currentScene.GetComponent<UnitComponent>();

            //foreach (UnitInfo unitInfo in message.Units)
            //{
            //	if (unitComponent.Get(unitInfo.UnitId) != null)
            //	{
            //		continue;
            //	}
            //	Unit unit = UnitFactory.Create(currentScene, unitInfo);
            //}
            await ETTask.CompletedTask;
        }

        public static async partial ETTask M2C_CreateMyUnit(M2C_CreateMyUnit message)
        {
			AOGame.Publish(new AO.EventType.CreateMyUnit() { Unit = message.Unit });
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
            var unit = Avatar.Main.GetScene().Get<SceneUnitComponent>().Get(message.UnitId);
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