using ET;
using System.Linq;
using TComp = ET.Scene;

namespace AO
{
    public static class SceneSystem
    {
        public class SceneAwakeSystem : AwakeSystem<TComp, string>
        {
            protected override void Awake(TComp self, string type)
            {
                self.Type = type;
            }
        }
    }
}