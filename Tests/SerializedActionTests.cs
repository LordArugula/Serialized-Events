using NUnit.Framework;

using Unity.PerformanceTesting;

using UnityEngine;
using UnityEngine.Events;

namespace Arugula.SerializedEvents.Tests
{
    public class SerializedEventTestsBase
    {
        protected const int WARM_UP_COUNT = 10;
        protected const int ITERATION_COUNT_PER_MEASUREMENT = 100_000;
        protected const int MEASUREMENT_COUNT = 10;

        protected class TestClass : MonoBehaviour
        {
            public const float VALUE = 3.14f;
            public void NoArguments() { }
            public void SingleArgument(GameObject go) { }
            public void FourArguments(GameObject go, float f, int i, string s) { }

            public float GetValue() => VALUE;

            public GameObject GetGameObject(int i)
            {
                return null;
            }

            public GameObject GetGameObject(GameObject go, float val, int i, string str)
            {
                return go;
            }

            public void ThrowInvalidOperationException()
            {
                throw new System.InvalidOperationException();
            }
        }
    }

    public class SerializedActionTests : SerializedEventTestsBase
    {
        private SerializedAction eventFromMethodName;
        private SerializedAction eventFromMethodGroup;
        private UnityEvent unityEvent;

        private SerializedAction<GameObject> eventT1FromMethodName;
        private SerializedAction<GameObject> eventT1FromMethodGroup;
        private UnityEvent<GameObject> unityEventT1;

        private SerializedAction<GameObject, float, int, string> eventT4FromMethodName;
        private SerializedAction<GameObject, float, int, string> eventT4FromMethodGroup;
        private UnityEvent<GameObject, float, int, string> unityEventT4;

        private TestClass testClass;

        [SetUp]
        public void SetUp()
        {
            testClass = new GameObject()
                .AddComponent<TestClass>();

            {
                eventFromMethodName = new SerializedAction();
                eventFromMethodName.AddListener(testClass, nameof(TestClass.NoArguments));

                eventFromMethodGroup = new SerializedAction();
                eventFromMethodGroup.AddListener(testClass.NoArguments);

                unityEvent = new UnityEvent();
                unityEvent.AddListener(testClass.NoArguments);
            }

            {
                eventT1FromMethodName = new SerializedAction<GameObject>();
                eventT1FromMethodName.AddListener(testClass, nameof(TestClass.SingleArgument));

                eventT1FromMethodGroup = new SerializedAction<GameObject>();
                eventT1FromMethodGroup.AddListener(testClass.SingleArgument);

                unityEventT1 = new UnityEvent<GameObject>();
                unityEventT1.AddListener(testClass.SingleArgument);
            }

            {
                eventT4FromMethodName = new SerializedAction<GameObject, float, int, string>();
                eventT4FromMethodName.AddListener(testClass, nameof(testClass.FourArguments));

                eventT4FromMethodGroup = new SerializedAction<GameObject, float, int, string>();
                eventT4FromMethodGroup.AddListener(testClass.FourArguments);

                unityEventT4 = new UnityEvent<GameObject, float, int, string>();
                unityEventT4.AddListener(testClass.FourArguments);
            }
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(testClass);
        }

        [Test, Performance]
        public void MethodCall_NoArguments_Performance()
        {
            Measure
                .Method(() =>
                {
                    testClass.NoArguments();
                })
                .WarmupCount(WARM_UP_COUNT)
                .IterationsPerMeasurement(ITERATION_COUNT_PER_MEASUREMENT)
                .MeasurementCount(MEASUREMENT_COUNT)
                .Run();
        }

        [Test, Performance]
        public void MethodCall_OneArgument_Performance()
        {
            Measure
                .Method(() =>
                {
                    testClass.SingleArgument(testClass.gameObject);
                })
                .WarmupCount(WARM_UP_COUNT)
                .IterationsPerMeasurement(ITERATION_COUNT_PER_MEASUREMENT)
                .MeasurementCount(MEASUREMENT_COUNT)
                .Run();
        }

        [Test, Performance]
        public void MethodCall_FourArguments_Performance()
        {
            Measure
                .Method(() =>
                {
                    testClass.FourArguments(testClass.gameObject, 3.14f, 2, "Test");
                })
                .WarmupCount(WARM_UP_COUNT)
                .IterationsPerMeasurement(ITERATION_COUNT_PER_MEASUREMENT)
                .MeasurementCount(MEASUREMENT_COUNT)
                .Run();
        }

        [Test, Performance]
        public void SerializedAction_FromMethodName_NoArguments_Performance()
        {
            Measure
                .Method(() =>
                {
                    eventFromMethodName.Invoke();
                })
                .WarmupCount(WARM_UP_COUNT)
                .IterationsPerMeasurement(ITERATION_COUNT_PER_MEASUREMENT)
                .MeasurementCount(MEASUREMENT_COUNT)
                .Run();
        }

        [Test, Performance]
        public void SerializedAction_FromMethodGroup_NoArguments_Performance()
        {
            Measure
                .Method(() =>
                {
                    eventFromMethodGroup.Invoke();
                })
                .WarmupCount(WARM_UP_COUNT)
                .IterationsPerMeasurement(ITERATION_COUNT_PER_MEASUREMENT)
                .MeasurementCount(MEASUREMENT_COUNT)
                .Run();
        }

        [Test, Performance]
        public void SerializedAction_FromMethodName_OneArgument_Performance()
        {
            Measure
                .Method(() =>
                {
                    eventT1FromMethodName.Invoke(testClass.gameObject);
                })
                .WarmupCount(WARM_UP_COUNT)
                .IterationsPerMeasurement(ITERATION_COUNT_PER_MEASUREMENT)
                .MeasurementCount(MEASUREMENT_COUNT)
                .Run();
        }

        [Test, Performance]
        public void SerializedAction_FromMethodGroup_OneArgument_Performance()
        {
            Measure
                .Method(() =>
                {
                    eventT1FromMethodGroup.Invoke(testClass.gameObject);
                })
                .WarmupCount(WARM_UP_COUNT)
                .IterationsPerMeasurement(ITERATION_COUNT_PER_MEASUREMENT)
                .MeasurementCount(MEASUREMENT_COUNT)
                .Run();
        }

        [Test, Performance]
        public void SerializedAction_FromMethodName_FourArguments_Performance()
        {
            Measure
                .Method(() =>
                {
                    eventT4FromMethodName.Invoke(testClass.gameObject, 3.14f, 2, "Test");
                })
                .WarmupCount(WARM_UP_COUNT)
                .IterationsPerMeasurement(ITERATION_COUNT_PER_MEASUREMENT)
                .MeasurementCount(MEASUREMENT_COUNT)
                .Run();
        }

        [Test, Performance]
        public void SerializedAction_FromMethodGroup_FourArguments_Performance()
        {
            Measure
                .Method(() =>
                {
                    eventT4FromMethodGroup.Invoke(testClass.gameObject, 3.14f, 2, "Test");
                })
                .WarmupCount(WARM_UP_COUNT)
                .IterationsPerMeasurement(ITERATION_COUNT_PER_MEASUREMENT)
                .MeasurementCount(MEASUREMENT_COUNT)
                .Run();
        }

        [Test, Performance]
        public void UnityEvent_NoArguments_Performance()
        {
            Measure
                .Method(() =>
                {
                    unityEvent.Invoke();
                })
                .WarmupCount(WARM_UP_COUNT)
                .IterationsPerMeasurement(ITERATION_COUNT_PER_MEASUREMENT)
                .MeasurementCount(MEASUREMENT_COUNT)
                .Run();
        }

        [Test, Performance]
        public void UnityEvent_OneArgument_Performance()
        {
            Measure
                .Method(() =>
                {
                    unityEventT1.Invoke(testClass.gameObject);
                })
                .WarmupCount(WARM_UP_COUNT)
                .IterationsPerMeasurement(ITERATION_COUNT_PER_MEASUREMENT)
                .MeasurementCount(MEASUREMENT_COUNT)
                .Run();
        }

        [Test, Performance]
        public void UnityEvent_FourArguments_Performance()
        {
            Measure
                .Method(() =>
                {
                    unityEventT4.Invoke(testClass.gameObject, 3.14f, 2, "Test");
                })
                .WarmupCount(WARM_UP_COUNT)
                .IterationsPerMeasurement(ITERATION_COUNT_PER_MEASUREMENT)
                .MeasurementCount(MEASUREMENT_COUNT)
                .Run();
        }

        [Test]
        public void SerializedAction()
        {
            SerializedAction action = new SerializedAction();

            Assert.DoesNotThrow(() =>
            {
                action.Invoke();
            });

            action.AddListener(testClass, nameof(TestClass.ThrowInvalidOperationException));

            Assert.Throws<System.InvalidOperationException>(() =>
            {
                action.Invoke();
            });

            action.RemoveListener(testClass, nameof(TestClass.ThrowInvalidOperationException));

            Assert.DoesNotThrow(() =>
            {
                action.Invoke();
            });
        }
    }
}
