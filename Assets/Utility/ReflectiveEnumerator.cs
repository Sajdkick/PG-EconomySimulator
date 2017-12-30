using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class ReflectiveEnumerator
{

    static ReflectiveEnumerator() { }

    public static List<System.Type> GetEnumerableOfType<T>(params object[] constructorArgs) where T : class
    {
        List<System.Type> types = new List<System.Type>();
        foreach (System.Type type in
            System.Reflection.Assembly.GetAssembly(typeof(T)).GetTypes()
            .Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(T))))
        {
            types.Add(type);
        }
        return types;
    }
}
