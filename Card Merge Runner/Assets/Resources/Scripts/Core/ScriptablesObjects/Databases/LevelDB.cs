using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
namespace Hyperlab.Core
{
	[System.Serializable]
	public class LevelDB : SingletonScriptableObject<LevelDB>
	{
		[AssetList(Path = "/Resources/Data/Levels")]
		public List<Level> m_List = new List<Level>();
	} 
}
