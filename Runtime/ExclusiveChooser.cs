using System;
using UnityEditor;
using UnityEditor.VersionControl;
using UnityEngine;

namespace com.ericsebesta.unitytools
{
    /// <summary>
    /// A behavior that ensures that exactly 1 (or 0 if no children) gameobjects are active. If at any point more than 1 child
    /// is active, the first child will be preferred.
    /// </summary>
    /// This class functions along with the help of ExclusiveChooserChild class which this class automatically adds
    /// to all children of this class. The helper class auto-removes itself if reparented.
    [ExecuteAlways]
    public class ExclusiveChooser : MonoBehaviour
    {
        //The currently chosen child gameobject
        private GameObject _chosenChild;
        //Whether we are in the process of choosing an active child or not (necessary to prevent re-entry issues from callbacks).
        private bool _currentlyChoosing;

        /// <summary>
        /// When waking up, ensure that only the FIRST active child remains active, disable all others.
        /// Also ensure that all children have the helper component.
        /// </summary>
        private void Awake()
        {
            GameObject childToSelect = null;
            //choose the first active child
            for (var i = 0; i < gameObject.transform.childCount; i++)
            {
                var child = gameObject.transform.GetChild(i);
                if (child.gameObject.activeSelf)
                {
                    childToSelect = child.gameObject;
                    break;
                }
            }
            //add child component to all children
            for (var i = 0; i < gameObject.transform.childCount; i++)
            {
                var child = gameObject.transform.GetChild(i);
                if (!child.GetComponent<ExclusiveChooserChild>())
                {
                    //adding components results in OnActivate called and we don't want to process it like a selection
                    _currentlyChoosing = true;
                    child.gameObject.AddComponent<ExclusiveChooserChild>();
                    _currentlyChoosing = false;
                }
            }
            //select the first item (null is fine)
            ChooseChild(childToSelect);
        }

        /// <summary>
        /// When our children change, ensure that newly-added ones get the helper component, and ensure that only the
        /// FIRST active child remains active.
        /// </summary>
        private void OnTransformChildrenChanged()
        {
            for (var i = 0; i < gameObject.transform.childCount; i++)
            {
                var child = gameObject.transform.GetChild(i);
                if (!child.GetComponent<ExclusiveChooserChild>())
                {
                    //adding components results in OnActivate called and we don't want to process it like a selection
                    _currentlyChoosing = true;
                    child.gameObject.AddComponent<ExclusiveChooserChild>();
                    _currentlyChoosing = false;
                }
            }
            //we may have removed the only active child, or we may have added a second active child. Resolve this.
            ChooseChild(_chosenChild ? _chosenChild : null);
        }

        /// <summary>
        /// Select the given child as the active chosen one, disable all others. If null, we'll choose the first child.
        /// </summary>
        /// <param name="go">The gameobject to choose if found as a child. Can be null which means "choose the first child"</param>
        public void ChooseChild(GameObject go)
        {
            if (_currentlyChoosing)
                return;
            _currentlyChoosing = true;
            var foundSelection = false;
            for (var i = 0; i < gameObject.transform.childCount; i++)
            {
                var child = gameObject.transform.GetChild(i);
                if (go && child == go.transform)
                {
                    child.gameObject.SetActive(true);
                    foundSelection = true;
                    _chosenChild = child.gameObject;
                }
                else
                {
                    child.gameObject.SetActive(false);
                }
            }

            if (!foundSelection && gameObject.transform.childCount > 0)
            {
                var child = gameObject.transform.GetChild(0);
                child.gameObject.SetActive(true);
                _chosenChild = child.gameObject;
            }
            _currentlyChoosing = false;
        }
    }
}