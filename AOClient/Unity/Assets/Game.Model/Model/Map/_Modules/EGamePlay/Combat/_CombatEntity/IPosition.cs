using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using ET;
using Unity.Mathematics;

namespace EGamePlay
{
    public interface IPosition
    {
        float3 Position { get; set; }
    }
}