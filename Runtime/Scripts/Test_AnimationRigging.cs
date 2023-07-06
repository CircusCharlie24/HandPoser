using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CodeLibrary24.HandPoser
{
    public class Test_AnimationRigging : MonoBehaviour
    {
        [SerializeField] private List<GameObject> followers;
        [SerializeField] private List<GameObject> targets;

        private void Update()
        {
            FollowTargets();
        }

        [ContextMenu("Follow")]
        private void FollowTargets()
        {
            for (int i = 0; i < followers.Count; i++)
            {
                followers[i].transform.position = targets[i].transform.position;
                followers[i].transform.rotation = targets[i].transform.rotation;
            }
        }
    }
}
