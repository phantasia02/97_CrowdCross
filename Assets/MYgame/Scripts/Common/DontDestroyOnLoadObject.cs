using UnityEngine;
using System.Collections;
//using GameAnalyticsSDK;

namespace UnityUtility {
	public class DontDestroyOnLoadObject : MonoBehaviour {
		void Awake () {
           // GameAnalytics.Initialize();
            DontDestroyOnLoad( this );
		}
	}
}
