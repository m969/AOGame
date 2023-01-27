using ET;
using System.Linq;

namespace AO
{
    public static class SceneUnitComponentSystem
    {
        public static void Add(this SceneUnitComponent self, Entity unit)
        {
            self.idUnits.Add(unit.Id, unit);
            if (unit is Avatar avatar) self.idAvatars.Add(unit.Id, avatar);
        }

        public static Entity Get(this SceneUnitComponent self, long id)
        {
            self.idUnits.TryGetValue(id, out var gamer);
            return gamer;
        }

        public static void Remove(this SceneUnitComponent self, long id)
        {
            self.idUnits.Remove(id);
            self.idAvatars.Remove(id);
        }

        public static Entity[] GetAll(this SceneUnitComponent self)
        {
            return self.idUnits.Values.ToArray();
        }
    }
}