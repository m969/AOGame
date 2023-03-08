using ET;
using System.Linq;

namespace AO
{
    public static class MapSceneComponentSystem
    {
        public static void Add(this MapSceneComponent self, Scene scene)
        {
            self.idScenes.Add(scene.Id, scene);
            self.typeScenes.Add(scene.Type, scene);
        }

        public static Scene Get(this MapSceneComponent self, long id)
        {
            self.idScenes.TryGetValue(id, out var gamer);
            return gamer;
        }

        public static Scene GetScene(this MapSceneComponent self, string type)
        {
            return self.typeScenes.GetOne(type);
        }

        public static void Remove(this MapSceneComponent self, long id)
        {
            self.idScenes.Remove(id);
        }

        public static Scene[] GetAll(this MapSceneComponent self)
        {
            return self.idScenes.Values.ToArray();
        }
    }
}