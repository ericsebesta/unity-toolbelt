using System.Collections;
using System.Dynamic;
using System.Threading.Tasks;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.TestTools;

namespace com.ericsebesta.unitytools.Tests
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
        /*
        public static IEnumerator Execute (Task task)
        {
            while (!task.IsCompleted) { yield return null; }
            if (task.IsFaulted) { throw task.Exception; }
        }
        
        [Test]
        [Ignore("Need to better understand how to write multi-frame tests, this won't work until i do")]
        public async void ExclusiveChooser_AfterRemoving1Child_Child1HasChildComponentRemoved()
        {
            var go = new GameObject();
            go.AddComponent<ExclusiveChooser>();
            var child1 = new GameObject();
            child1.transform.parent = go.transform;
            child1.transform.parent = null;

            await DelayAFrame();
            Debug.Log("hello1");
//            Execute(DelayAFrame());
            Debug.Log("hello4");
            Assert.IsNull(child1.GetComponent<ExclusiveChooserChild>(), "a child of a ExclusiveChooser should no longer have the ExclusiveChooserChild component after unparenting");
            
//            var component = go.GetComponent<ExclusiveChooser>();
//            IEnumerator coroutine = ExclusiveChooser_AfterRemoving1Child_Child1HasChildComponentRemoved_Part2(child1);
//            component.StartCoroutine(coroutine);
        }

        private async Task DelayAFrame()
        {
            Debug.Log("hello2");
            await Task.Delay(5000);
            Debug.Log("hello3");
        }
        
        private IEnumerator ExclusiveChooser_AfterRemoving1Child_Child1HasChildComponentRemoved_Part2(GameObject child1)
        {
            Debug.Log("hello1");
            yield return new WaitForSeconds(1.0f);
            Debug.Log("hello2");
            Assert.IsNull(child1.GetComponent<ExclusiveChooserChild>(), "a child of a ExclusiveChooser should no longer have the ExclusiveChooserChild component after unparenting");
            Assert.True(false);
            Debug.Log("hello3");
        }*/
        
        [Test]
        public void ExclusiveChooser_With1Child_Child1ActiveAfterDeactivating()
        {
            //even if we try to deactivate the singular child, ExclusiveChooser should keep it active
            _child1.transform.parent = _exclusiveChooser.transform;
            _child1.SetActive(false);

            UnityEngine.TestTools.LogAssert.Expect(LogType.Error, "GameObject is already being activated or deactivated.");
            Assert.True(_child1.activeSelf, "a singular child of a ExclusiveChooser should be active when added");
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