using NUnit.Framework;
using UnityEngine;

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
            m_exclusiveChooser = null;
            m_child1 = null;
            m_child2 = null;
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
            m_child1.transform.parent = m_exclusiveChooser.transform;
            Assert.True(m_child1.activeSelf, "a singular child of a ExclusiveChooser should be active when added");
        }
        
        [Test]
        public void ExclusiveChooser_With1Child_Child1HasChildComponentAdded()
        {
            m_child1.transform.parent = m_exclusiveChooser.transform;
            Assert.NotNull(m_child1.GetComponent<ExclusiveChooserChild>(), "a singular child of a ExclusiveChooser should be get the ExclusiveChooserChild added when parented to it");
        }
        
        [Test]
        public void ExclusiveChooser_AddingChild2_Child1ActiveAndChild2Inactive()
        {
            m_child1.transform.parent = m_exclusiveChooser.transform;
            m_child2.transform.parent = m_exclusiveChooser.transform;
            
            Assert.True(m_child1.activeSelf, "a first child of a ExclusiveChooser should be active when a second child is added");
            Assert.False(m_child2.activeSelf, "a second child of a ExclusiveChooser should be inactive when added");
        }
        
        [Test]
        public void ExclusiveChooser_ActivatingChild2_Child1Inactive()
        {
            m_child1.transform.parent = m_exclusiveChooser.transform;
            m_child2.transform.parent = m_exclusiveChooser.transform;

            m_child2.SetActive(true);

            Assert.False(m_child1.activeSelf, "a first child of a ExclusiveChooser should be inactive when a second child is activated");
            Assert.True(m_child2.activeSelf, "a second child of a ExclusiveChooser should be active when activated");
        }
        
        [Test]
        public void ExclusiveChooser_ReparentingChild1_Child1Active()
        {
            m_child1.transform.parent = m_exclusiveChooser.transform;
            m_child2.transform.parent = m_exclusiveChooser.transform;

            Assert.False(m_child2.activeSelf, "a second child of a ExclusiveChooser should be inactive when a the first is active");
            // ReSharper disable once Unity.InefficientPropertyAccess
            m_child1.transform.parent = null;
            Assert.True(m_child2.activeSelf, "a second child of a ExclusiveChooser should be active when a second child is reparented away from the ExclusiveChooser");
        }
        
        [Test]
        public void ExclusiveChooser_ReparentingChild1LeavingNoChildren_IsOkay()
        {
            m_child1.transform.parent = m_exclusiveChooser.transform;
            //should not cause any exceptions
        }
    }
}