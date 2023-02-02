using ET;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AO
{
    public static class AvatarSystem
    {
        public static Scene GetScene(this Avatar avatar)
        {
            return avatar.GetParent<Scene>();
        }
    }
}