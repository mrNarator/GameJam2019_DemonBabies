using DB.EventSystem;
using DB.Extensions;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace DB
{
	public class LevelInitializer : MonoBehaviour
	{
		[SerializeField]
		private ObjInitializationInfo playerSpawn;

		[SerializeField]
		private List<ObjInitializationInfo> enemySpawns;

		private void Start()
		{
			SpawnObjects();
		}

		private void SpawnObjects()
		{
			var spawnF = SpawnLogicMap[playerSpawn.SpawnLogic];
			spawnF(playerSpawn, null);

			foreach(var info in enemySpawns)
			{
				spawnF = SpawnLogicMap[info.SpawnLogic];
				spawnF(info, null);
			}

			Instantiate(Settings.Get.GeneralPrefabs.UIStatusPrefab);

			GlobalEvents.GetEvent<LevelSetupEvent>().Publish();
		}

		private Dictionary<SpawnInfoLogic, Action<ObjInitializationInfo, Action<GameObject>>> SpawnLogicMap =
			new Dictionary<SpawnInfoLogic, Action<ObjInitializationInfo, Action<GameObject>>>()
			{
				{SpawnInfoLogic.InAll, SpawnAll },
				{SpawnInfoLogic.Sequentially, SpawnSequentialy },
				{SpawnInfoLogic.RandomlyLayers, SpawnRandomControlled },
				{SpawnInfoLogic.RAndomlyTrue, SpawnTrueRandom },
			};

		private static void SpawnAll(ObjInitializationInfo info, Action<GameObject> postSpawn = null)
		{
			foreach(var tr in info.SpawnLocations)
			{
				var go = Instantiate(info.PrefabToSpawn, tr.position, tr.rotation);
				go.transform.SetParent(info.Parent, true);
				if(postSpawn != null)
				{
					postSpawn(go);
				}
			}
		}

		private static void SpawnSequentialy(ObjInitializationInfo info, Action<GameObject> postSpawn = null)
		{
			var locCnt = info.SpawnLocations.Count;
			for(int i = 0; i < info.SpawnsCount; i++)
			{
				var loc = info.SpawnLocations[i % locCnt];
				var go = Instantiate(info.PrefabToSpawn, loc.position, loc.rotation);
				go.transform.SetParent(info.Parent, true);
				if(postSpawn != null)
				{
					postSpawn(go);
				}
			}
		}

		private static void SpawnRandomControlled(ObjInitializationInfo info, Action<GameObject> postSpawn = null)
		{
			var spawnedIndices = new List<bool>(info.SpawnLocations.Count);
			var listAvailable = new List<Transform>(info.SpawnLocations);
			for (int i = 0; i < info.SpawnsCount; i++)
			{
				if(listAvailable.Count == 0)
				{
					listAvailable = new List<Transform>(info.SpawnLocations);
				}

				var loc = listAvailable.GetRandomItemFromList();
				listAvailable.Remove(loc);
				var go = Instantiate(info.PrefabToSpawn, loc.position, loc.rotation);
				go.transform.SetParent(info.Parent, true);
				if(postSpawn != null)
				{
					postSpawn(go);
				}
			}
		}

		private static void SpawnTrueRandom(ObjInitializationInfo info, Action<GameObject> postSpawn = null)
		{
			for (int i = 0; i < info.SpawnsCount; i++)
			{
				var loc = info.SpawnLocations.GetRandomItemFromList();
				var go = Instantiate(info.PrefabToSpawn, loc.position, loc.rotation);
				go.transform.SetParent(info.Parent, true);
				if(postSpawn != null)
				{
					postSpawn(go);
				}
			}
		}
	}

	[Serializable]
	public class ObjInitializationInfo
	{
		public List<Transform> SpawnLocations;
		public GameObject PrefabToSpawn;
		public Transform Parent;
		public SpawnInfoLogic SpawnLogic;
		public int SpawnsCount;

#if UNITY_EDITOR
		[ContextMenu("FillLocationsFromParentChildren")]
		internal void EditorFillChildrenFromParentField()
		{
			Action<List<Transform>, Transform> recursive = null;
			recursive = (list, obj) =>
			{
				foreach (Transform tr in obj)
				{
					list.Add(tr);
					if (tr.childCount > 0) recursive(list, obj);
				}
			};

			recursive(SpawnLocations, Parent);
		}
#endif // UNITY_EDITOR
	}

	public enum SpawnInfoLogic
	{
		InAll = 0,
		Sequentially = 1,
		RandomlyLayers = 2,
		RAndomlyTrue = 3,
	}
}
