using ET;
using Unity.Mathematics;

namespace AO
{
    public class UnitTranslateComponent : Entity, IAwake, IDestroy, IUpdate
    {
        public IMapUnit Unit { get; set; }
        private float3 targetPosition { get; set; }
        public float3 TargetPosition
        {
            get { return targetPosition; }
            set 
            {
                targetPosition = value; 
                TargetPositionNormalize = math.normalize(value - Unit.Position); 
                TranslateFinish = false; 
            }
        }
        public float3 TargetPositionNormalize { get; set; }
        public float Speed { get; set; } = 1f;
        public long PreTime { get; set; }
        public float DeltaTime { get; set; }
        public bool TranslateFinish { get; set; }
        public ETTask TranslateTask { get; set; }
    }
}
