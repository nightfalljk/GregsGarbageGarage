using System;
using System.Collections.Generic;
using Trash.Properties;
using UnityEngine;

namespace Trash
{
    public class Trash : MonoBehaviour
    {
        public TrashProperty[] Properties;
        public TrashType Type;

        public Dictionary<Type, TrashProperty> PropertiesDictionary;

        [NonSerialized]
        public Material OriginalMaterial;
        
        private void Awake()
        {
            OriginalMaterial = GetComponentInChildren<MeshRenderer>().material;
            PropertiesDictionary = new Dictionary<Type, TrashProperty>();

            foreach (TrashProperty trashProperty in Properties)
            {
                PropertiesDictionary.Add(trashProperty.GetType(), trashProperty);
            }
        }
    }
}