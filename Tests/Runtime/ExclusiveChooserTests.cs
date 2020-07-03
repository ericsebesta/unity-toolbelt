using System.Collections;
using System.Dynamic;
using System.Threading.Tasks;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.TestTools;

namespace com.ericsebesta.toolbelt.Tests
{
    [TestFixture]
    public class ExclusiveChooserTests
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
        
        [Test]
        public void ExclusiveChooser_With1Child_Child1ActiveAfterCreation()
        {
            //not logic we "own", but confirm that a newly created object is active, since we depend on it so heavily here
            var obj = new GameObject();
            Assert.True(obj.activeSelf, "a GameObject should be active when created");
        }
        
        [Test]
        public void ExclusiveChooser_With1Child_Child1ActiveAfterAdding()
        {
            _child1.transform.parent = _exclusiveChooser.transform;
            Assert.True(_child1.activeSelf, "a singular child of a ExclusiveChooser should be active when added");
        }
        
        [Test]
        public void ExclusiveChooser_With1Child_Child1HasChildComponentAdded()
        {
            _child1.transform.parent = _exclusiveChooser.transform;
            Assert.NotNull(_child1.GetComponent<ExclusiveChooserChild>(), "a singular child of a ExclusiveChooser should be get the ExclusiveChooserChild added when parented to it");
        }
        
        [Test]
        public void ExclusiveChooser_AddingChild2_Child1ActiveAndChild2Inactive()
        {
            _child1.transform.parent = _exclusiveChooser.transform;
            _child2.transform.parent = _exclusiveChooser.transform;
            
            Assert.True(_child1.activeSelf, "a first child of a ExclusiveChooser should be active when a second child is added");
            Assert.False(_child2.activeSelf, "a second child of a ExclusiveChooser should be inactive when added");
        }
        
        [Test]
        public void ExclusiveChooser_ActivatingChild2_Child1Inactive()
        {
            _child1.transform.parent = _exclusiveChooser.transform;
            _child2.transform.parent = _exclusiveChooser.transform;

            _child2.SetActive(true);

            Assert.False(_child1.activeSelf, "a first child of a ExclusiveChooser should be inactive when a second child is activated");
            Assert.True(_child2.activeSelf, "a second child of a ExclusiveChooser should be active when activated");
        }
        
        [Test]
        public void ExclusiveChooser_ReparentingChild1_Child1Active()
        {
            _child1.transform.parent = _exclusiveChooser.transform;
            _child2.transform.parent = _exclusiveChooser.transform;

            Assert.False(_child2.activeSelf, "a second child of a ExclusiveChooser should be inactive when a the first is active");
            _child1.transform.parent = null;
            Assert.True(_child2.activeSelf, "a second child of a ExclusiveChooser should be active when a second child is reparented away from the ExclusiveChooser");
        }
        
        [Test]
        public void ExclusiveChooser_ReparentingChild1LeavingNoChildren_IsOkay()
        {
            _child1.transform.parent = _exclusiveChooser.transform;
            //should not cause any exceptions
        }
    }
}