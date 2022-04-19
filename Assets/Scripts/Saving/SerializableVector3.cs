﻿using System;
using UnityEngine;

namespace RPG.Saving
{
    [Serializable]
    class SerializableVector3
    {
        float x, y, z;

        public SerializableVector3(Vector3 vector)
        {
            x = vector.x;
            y = vector.y;
            z = vector.z;
        }

        public Vector3 GetVector()
        {
            return new Vector3(x, y, z);
        }
    }
}
