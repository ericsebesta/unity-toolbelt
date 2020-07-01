using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using com.ericsebesta.unitytools;

namespace com.ericsebesta.unitytools.editor
{
    /// <summary>
    /// To support ExclusiveChooser, ensure the exclusive choice whenever selecting the object in the editor.
    /// Note, this allows having > 1 object active by manually activating/deactivating children (at edit time ONLY),
    /// for easier development.
    /// </summary>
    [CustomEditor(typeof(ExclusiveChooser))]
    public class ExclusiveChooserEditor : Editor
    {
        /// <summary>
        /// Whenever this element is clicked on, enforce the exclusive selection.
        /// </summary>
        private void Awake()
        {
            var script = (ExclusiveChooser) target;
            GameObject childToSelect = null;
            //choose the first active child
            for (var i = 0; i < script.gameObject.transform.childCount; i++)
            {
                var child = script.gameObject.transform.GetChild(i);
                if (child.gameObject.activeSelf)
                {
                    childToSelect = child.gameObject;
                    break;
                }
            }
            script.ChooseChild(childToSelect);
        }
    }
}
