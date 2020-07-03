using UnityEngine;

namespace com.ericsebesta.toolbelt
{
    /// <summary>
    /// A runtime-added helper class for ExclusiveChooser. Do not add this manually, ExclusiveChooser adds this as needed to support its behavior.
    /// </summary>
    [ExecuteAlways]
    public class ExclusiveChooserChild : MonoBehaviour
    {
        /// <summary>
        /// Inform parent ExclusiveChooser (if we have one, and we should) about this so it can respond.
        /// </summary>
        private void OnDisable()
        {
            var chooser = GetComponentInParent<ExclusiveChooser>();
            if (chooser)
            {
                chooser.ChooseChild(null);
            }
        }

        /// <summary>
        /// Inform parent ExclusiveChooser (if we have one, and we should) about this so it can respond.
        /// </summary>
        private void OnEnable()
        {
            var chooser = GetComponentInParent<ExclusiveChooser>();
            if (chooser)
            {
                chooser.ChooseChild(gameObject);
            }
        }

        /// <summary>
        /// If we're reparented and longer parented to an ExclusiveChooser, remove this script
        /// </summary>
        private void OnTransformParentChanged()
        {
            var newParentIsExclusiveChooser = GetComponentInParent<ExclusiveChooser>() != null;
            if (!newParentIsExclusiveChooser)
            {
                Destroy(this);
            }
        }
    }
}