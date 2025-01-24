using System;
using ScriptableObjects.Skins;
using UnityEngine;

namespace Interface.Store
{
    [Serializable]
    public class StoreMeshHolder
    {
        public MeshFilter filter;
        public MeshRenderer renderer;

        public void Apply(Skin skin)
        {
            filter.mesh = skin.mesh;
            renderer.material = skin.material;
        }
    }
}
