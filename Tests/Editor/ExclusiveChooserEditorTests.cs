using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace com.ericsebesta.unitytools.Tests.Editor
{
    [TestFixture]
    public class ExclusiveChooserEditorTests
    {   
        /// <summary>
        /// A trivial GameObject with ExclusiveChooser component, reset for each test
        /// </summary>
        private GameObject _exclusiveChooser;
        
        /// <summary>
        /// A trivial GameObject, reset for each test
        /// </summary>
        private GameObject _child1;
        
        /// <summary>
        /// A trivial GameObject, reset for each test
        /// </summary>
        private GameObject _child2;

        [SetUp]
        public void Setup()
        {
            _exclusiveChooser = new GameObject();
            _exclusiveChooser.AddComponent<ExclusiveChooser>();
            _child1 = new GameObject();
            _child2 = new GameObject();
        }

        [TearDown]
        public void Teardown()
        {
            _exclusiveChooser = null;
            _child1 = null;
            _child2 = null;
        }
    }
}