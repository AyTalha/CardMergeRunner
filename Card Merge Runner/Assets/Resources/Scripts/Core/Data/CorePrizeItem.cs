using UnityEngine;
using System.Collections;
using Hyperlab.Core.UI;
namespace Hyperlab.Core
{
	[System.Serializable]
	public class CorePrizeItem
	{
		public int m_PrizeUnlockLevel;
		public int m_Piece;
		protected virtual void Unlock() { }
	}

}