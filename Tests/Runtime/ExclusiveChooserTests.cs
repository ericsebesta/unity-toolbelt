using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace com.ericsebesta.toolbelt.Tests
{
    [TestFixture]
    public class ExclusiveChooserTests
    {
        /// <summary>
        /// A trivial GameObject with ExclusiveChooser component, reset for each test
        /// </summary>
        private GameObject m_exclusiveChooser;
        
        /// <summary>
        /// A trivial GameObject, reset for each test
        /// </summary>
        private GameObject m_child1;
        
        /// <summary>
        /// A trivial GameObject, reset for each test
        /// </summary>
        private GameObject m_child2;

        [SetUp]
        public void Setup()
        {
            m_exclusiveChooser = new GameObject();
            m_exclusiveChooser.AddComponent<ExclusiveChooser>();
            m_child1 = new GameObject();
            m_child2 = new GameObject();
        }

        [TearDown]
        public void Teardown()
        {
            Object.Destroy(m_child2);
            Object.Destroy(m_child1);
            Object.Destroy(m_exclusiveChooser);
            m_exclusiveChooser = null;
            m_child1 = null;
            m_child2 = null;
        }
        
        [UnityTest]
        public IEnumerator ExclusiveChooser_With1Child_Child1ActiveAfterCreation()
        {
            //not logic we "own", but confirm that a newly created object is active, since we depend on it so heavily here
            var obj = new GameObject();
            Assert.False(obj.activeSelf, "a GameObject should be active when created"); //purposefully break a test to validate cloud build test support
            yield return null;
        }
        
        [UnityTest]
        public IEnumerator ExclusiveChooser_With1Child_Child1ActiveAfterAdding()
        {
            m_child1.transform.parent = m_exclusiveChooser.transform;
            Assert.True(m_child1.activeSelf, "a singular child of a ExclusiveChooser should be active when added");
            yield return null;
        }
        
        [UnityTest]
        public IEnumerator ExclusiveChooser_With1Child_Child1HasChildComponentAdded()
        {
            m_child1.transform.parent = m_exclusiveChooser.transform;
            Assert.NotNull(m_child1.GetComponent<ExclusiveChooserChild>(), "a singular child of a ExclusiveChooser should be get the ExclusiveChooserChild added when parented to it");
            yield return null;
        }
        
        [UnityTest]
        public IEnumerator ExclusiveChooser_AddingChild2_Child1ActiveAndChild2Inactive()
        {
            m_child1.transform.parent = m_exclusiveChooser.transform;
            m_child2.transform.parent = m_exclusiveChooser.transform;
            
            Assert.True(m_child1.activeSelf, "a first child of a ExclusiveChooser should be active when a second child is added");
            Assert.False(m_child2.activeSelf, "a second child of a ExclusiveChooser should be inactive when added");
            yield return null;
        }
        
        [UnityTest]
        public IEnumerator ExclusiveChooser_ActivatingChild2_Child1Inactive()
        {
            m_child1.transform.parent = m_exclusiveChooser.transform;
            m_child2.transform.parent = m_exclusiveChooser.transform;

            m_child2.SetActive(true);

            Assert.False(m_child1.activeSelf, "a first child of a ExclusiveChooser should be inactive when a second child is activated");
            Assert.True(m_child2.activeSelf, "a second child of a ExclusiveChooser should be active when activated");
            yield return null;
        }
        
        [UnityTest]
        public IEnumerator ExclusiveChooser_ReparentingChild1_Child1Active()
        {
            m_child1.transform.parent = m_exclusiveChooser.transform;
            m_child2.transform.parent = m_exclusiveChooser.transform;

            Assert.False(m_child2.activeSelf, "a second child of a ExclusiveChooser should be inactive when a the first is active");
            // ReSharper disable once Unity.InefficientPropertyAccess
            m_child1.transform.parent = null;
            Assert.True(m_child2.activeSelf, "a second child of a ExclusiveChooser should be active when a second child is reparented away from the ExclusiveChooser");
            yield return null;
        }
        
        [UnityTest]
        public IEnumerator ExclusiveChooser_ReparentingChild1LeavingNoChildren_IsOkay()
        {
            m_child1.transform.parent = m_exclusiveChooser.transform;
            //should not cause any exceptions
            yield return null;
        }

        /// <summary>
        /// Deactivate the only child node. This causes an invoke with slight delay to reactivate it.
        /// </summary>
        /// <returns></returns>
        [UnityTest]
        public IEnumerator ExclusiveChooser_DeactivateOnlyChild_ReactivatesChild()
        {
            m_child1.transform.parent = m_exclusiveChooser.transform;
            m_child1.SetActive(false);
           yield return new WaitForSeconds(1f);
           //should have reactivated itself
           Assert.IsTrue(m_child1.activeSelf);
           yield return null;
        }
        
        /// <summary>
        /// Test that when we add the component, it resolves the necessary state of existing children
        /// </summary>
        /// <returns></returns>
        [UnityTest]
        public IEnumerator ExclusiveChooser_AddComponentWith2ActiveChildren_SetsOnlyFirstActiveOnAwake()
        {
            var exclusiveChooser = new GameObject();
            var child1 = new GameObject();
            var child2 = new GameObject();
            child1.transform.parent = exclusiveChooser.transform;
            child2.transform.parent = exclusiveChooser.transform;
            exclusiveChooser.AddComponent<ExclusiveChooser>();
            yield return null;
            //only first child should be active
            Assert.IsTrue(child1.activeSelf);
            Assert.IsFalse(child2.activeSelf);
            yield return null;
        }
        
        /// <summary>
        /// Test that when we add the component, it resolves the necessary state of existing children
        /// </summary>
        /// <returns></returns>
        [UnityTest]
        public IEnumerator ExclusiveChooser_AddComponentWithChild1InactiveAndChild2Active_LeavesThemOnAwake()
        {
            var exclusiveChooser = new GameObject();
            var child1 = new GameObject();
            var child2 = new GameObject();
            child1.transform.parent = exclusiveChooser.transform;
            child1.SetActive(false);
            child2.transform.parent = exclusiveChooser.transform;
            exclusiveChooser.AddComponent<ExclusiveChooser>();
            yield return null;
            //only first child should be active
            Assert.IsFalse(child1.activeSelf);
            Assert.IsTrue(child2.activeSelf);
            yield return null;
        }
        
        /// <summary>
        /// Test that when we add the component, it adds child components to existing children
        /// </summary>
        /// <returns></returns>
        [UnityTest]
        public IEnumerator ExclusiveChooser_AddingComponentToParent_AddsChildComponentsOnAwake()
        {
            var exclusiveChooser = new GameObject();
            var child1 = new GameObject();
            var child2 = new GameObject();
            child1.transform.parent = exclusiveChooser.transform;
            child2.transform.parent = exclusiveChooser.transform;
            //shouldn't have the component before attachment of parent component
            Assert.IsNull(m_child1.GetComponent<ExclusiveChooserChild>());
            Assert.IsNull(m_child2.GetComponent<ExclusiveChooserChild>());
            exclusiveChooser.AddComponent<ExclusiveChooser>();
            //adding the chooser to the parent should add the child component to all children
            Assert.IsNotNull(child1.GetComponent<ExclusiveChooserChild>());
            Assert.IsNotNull(child2.GetComponent<ExclusiveChooserChild>());
            yield return null;
        }
    }
}