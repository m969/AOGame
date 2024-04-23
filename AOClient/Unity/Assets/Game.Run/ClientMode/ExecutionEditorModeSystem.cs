using Codice.Client.BaseCommands;
using ET;
using Puerts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using TComp = AO.ExecutionEditorModeComponent;

namespace AO
{
    public static class ExecutionEditorModeSystem
    {
        [ObjectSystem]
        public class ExecutionEditorModeAwakeSystem : AwakeSystem<TComp>
        {
            protected override async void Awake(TComp self)
            {
                self.Parent.GetComponent<MapModeComponent>().RemoveCurrentScene();
                var mapScene = self.Parent.GetComponent<MapModeComponent>().CreateMapScene("ExecutionLinkScene");
                Scene.CurrentScene = mapScene;

                var avatarInfo = new UnitInfo();
                avatarInfo.UnitType = ((int)UnitType.Actor);
                avatarInfo.ActorType = ((int)ActorType.Player);
                avatarInfo.UnitId = IdGenerater.Instance.GenerateUnitId(0);
                EventSystem.Instance.Publish(self, new ET.EventType.CreateUnit() { Unit = avatarInfo, IsMainAvatar = true });

                var enemyInfo = new UnitInfo();
                enemyInfo.UnitType = ((int)UnitType.Actor);
                avatarInfo.ActorType = ((int)ActorType.NonPlayer);
                enemyInfo.UnitId = IdGenerater.Instance.GenerateUnitId(0);
                enemyInfo.Position = new Unity.Mathematics.float3 { x = 5, y = 0, z = 0 };
                EventSystem.Instance.Publish(self, new ET.EventType.CreateUnit() { Unit = enemyInfo });

                await TimeUtils.WaitAsync(500);

                Actor.Main.AddComponent<AttributeHPComponent>();
                Actor.Main.GetComponent<AttributeHPComponent>().AvailableValue = 100;
                Actor.Main.GetComponent<AttributeHPComponent>().AttributeValue = 100;
                self.BossUnit.AddComponent<AttributeHPComponent>();
                self.BossUnit.GetComponent<AttributeHPComponent>().AvailableValue = 100;
                self.BossUnit.GetComponent<AttributeHPComponent>().AttributeValue = 100;
            }
        }
    }
}