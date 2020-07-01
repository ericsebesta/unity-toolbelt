using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using com.ericsebesta.unitytools;

namespace Tests.Editor
{
    public class ExclusiveChooserTests
    {
        // A Test behaves as an ordinary method
        [Test]
        public void ExclusiveChooer_IsOkay_WithoutChildren()
        {
            var go = new GameObject();
            go.AddComponent<ExclusiveChooser>();
            Assert.True(true);
        }
        [Test]
        public void ExclusiveChooser_With1Child_ChoosesFirstOnAwake()
        {
            var go = new GameObject();
            go.AddComponent<ExclusiveChooser>();
            Assert.True(false); //TODO write this
        }
    }
}