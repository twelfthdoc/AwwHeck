using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public static class ServiceLocator
{
	private readonly static Dictionary<Type, ManagerBase> Managers = new();
	private readonly static Dictionary<Type, SingletonBase> Singletons = new();

	public static T GetManager<T>() where T : ManagerBase
	{
		var key = typeof(T);

		if (Managers.ContainsKey(key))
		{
			return (T) Managers[key];
		}

		var manager = Object.FindFirstObjectByType<T>();

		if (manager != null)
		{
			Managers.Add(key, manager);
		}

		return manager;
	}

	public static T GetSingleton<T>() where T : SingletonBase, new()
	{
		var key = typeof(T);

		if (Singletons.ContainsKey(key))
		{
			return (T) Singletons[key];
		}

		var singleton = new T();
		Singletons.Add(key, singleton);
		return singleton;
	}

	public static void DestroyManager<T>() where T : ManagerBase
	{
		var key = typeof(T);

		if (Managers.ContainsKey(key))
		{
			var instance = Object.FindFirstObjectByType<T>();
			if (instance == null)
			{
				Debug.LogWarning($"Tried to destroy manager of Type {key}, but it does not exist!");
			}
			else
			{
				Object.Destroy(instance.gameObject);
			}

			Managers.Remove(key);
		}
	}

	public static void DestroySingleton<T>() where T : SingletonBase => Singletons.Remove(typeof(T));
}
