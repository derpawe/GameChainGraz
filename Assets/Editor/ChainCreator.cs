using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
public class ChainCreator : Editor 
{
	public static int chainsize = 20;
	[MenuItem ("ChainCreator/CreateChain")]
	static void PrefabRoutine()
	{
		if (! Selection.activeObject) {
			return;
		}
		List<GameObject> chain_list = new List<GameObject> ();
		GameObject chain = new GameObject();
		GameObject protochainlink = Selection.activeGameObject;
		Vector3 scale = protochainlink.transform.localScale;
		Vector3 size = protochainlink.renderer.bounds.size;
		Vector3 offset = new Vector3 (size.x, 0, 0);
		Vector3 localoffset = new Vector3 (size.x/scale.x, 0, 0);
		GameObject prev_chainlink = null;
		for(int i=0;i<chainsize;i++){
			GameObject chainlink = Instantiate(protochainlink) as GameObject;
			chainlink.transform.parent = chain.transform;
			if(chainlink.GetComponent<BoxCollider2D>() == null)
				chainlink.AddComponent<BoxCollider2D>();
			if(chainlink.GetComponent<Rigidbody2D>() == null)
				chainlink.AddComponent<Rigidbody2D>();
			chain_list.Add(chainlink);
			if(prev_chainlink != null){ 
				chainlink.transform.position = prev_chainlink.transform.position + offset;
				prev_chainlink.AddComponent<HingeJoint2D>();
				HingeJoint2D h = prev_chainlink.GetComponent<HingeJoint2D>();
				h.connectedBody = chainlink.rigidbody2D;
				h.collideConnected = false;
				h.anchor = localoffset/2;
				h.connectedAnchor = -localoffset/2;
//				prev_chainlink.AddComponent<DistanceJoint2D>();
//				DistanceJoint2D d = prev_chainlink.GetComponent<DistanceJoint2D>();
//				d.connectedBody = chainlink.rigidbody2D;
//				d.distance = 0;
			}
			prev_chainlink = chainlink;
		}
	}
}
