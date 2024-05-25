using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu]
public class RaceSO  :ScriptableObject
{
    public string Name;
    //[EnumFlags]
    public BiomeType[] Biomes;
    public int Sterngth;
    public float Size;
    public float ReproductionSpeed;
    public int Aggressiveness;
    public string From;
}


//public class EnumFlagsAttribute : PropertyAttribute { }

//#if UNITY_EDITOR
//[CustomPropertyDrawer(typeof(EnumFlagsAttribute))]
//public class EnumFlagsAttributeDrawer : PropertyDrawer
//{
//    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
//    {
//        property.intValue = EditorGUI.MaskField(position, label, property.intValue, property.enumDisplayNames);
//    }
//}
//#endif


