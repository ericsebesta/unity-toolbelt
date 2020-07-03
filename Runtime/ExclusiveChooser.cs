using UnityEngine;

namespace com.ericsebesta.toolbelt
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
        private GameObject m_chosenChild;
        //Whether we are in the process of choosing an active child or not (necessary to prevent re-entry issues from callbacks).
        private bool m_currentlyChoosing;
        //handle an edge case with deactivating the first child. @see ReActivate 
        private GameObject m_reactivationTarget;

        /// <summary>
        /// Select the given child as the active chosen one, disable all others. If null, we'll choose the first child.
        /// The result of any call to this method will be exactly 1 child being activated, unless there are no children.
        /// </summary>
        /// <param name="go">The gameobject to choose if found as a child. Can be null which means "choose the first child"</param>
        public void ChooseChild(GameObject go)
        {
            if (m_currentlyChoosing)
                return;
            m_currentlyChoosing = true;
            var foundSelection = false;
            for (var i = 0; i < gameObject.transform.childCount; i++)
            {
                var child = gameObject.transform.GetChild(i);
                if (go && child == go.transform)
                {
                    var childGameObject = child.gameObject; 
                    childGameObject.SetActive(true);
                    foundSelection = true;
                    m_chosenChild = childGameObject;
                }
                else
                {
                    child.gameObject.SetActive(false);
                }
            }

            if (!foundSelection && gameObject.transform.childCount > 0)
            {
                var child = gameObject.transform.GetChild(0);
                var childGameObject = child.gameObject;
                if (m_chosenChild == childGameObject)
                {
                    //in this specific situation, we are re-activating the node we *think* was already active.
                    //Most likely, this means that the first child was requested to be deactivated (which logic dictates
                    //should be reactivated here). Doing so inline of the callstack requesting to DISable it will throw
                    //an error, so delay the reactivation to a future frame. This could probably be done later in the same
                    //frame rather than waiting until the next frame.
                    m_reactivationTarget = childGameObject;
                    Invoke(nameof(ReActivate), .05f);
                }
                else
                {
                    childGameObject.SetActive(true);
                }
                m_chosenChild = childGameObject;
            }
            m_currentlyChoosing = false;
        }
        
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
                    m_currentlyChoosing = true;
                    child.gameObject.AddComponent<ExclusiveChooserChild>();
                    m_currentlyChoosing = false;
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
                    m_currentlyChoosing = true;
                    child.gameObject.AddComponent<ExclusiveChooserChild>();
                    m_currentlyChoosing = false;
                }
            }
            //we may have removed the only active child, or we may have added a second active child. Resolve this.
            ChooseChild(m_chosenChild ? m_chosenChild : null);
        }

        /// <summary>
        /// This method exists to handle an edge case of deactivating the first child, which should re-enable it.
        /// This will cause an error if we directly call SetActive(true) within the handling of OnDisable. So we delay
        /// the call and handle it here.
        /// </summary>
        private void ReActivate()
        {
            m_reactivationTarget.SetActive(true);
            m_reactivationTarget = null;
        }
    }
}