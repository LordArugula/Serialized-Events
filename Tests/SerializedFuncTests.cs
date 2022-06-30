using NUnit.Framework;

using Unity.PerformanceTesting;

using UnityEngine;

namespace Arugula.SerializedEvents.Tests
{
    public class SerializedFuncTests : SerializedEventTestsBase
    {
        private SerializedFunc<float> eventFromMethodName;
        private SerializedFunc<float> eventFromMethodGroup;

        private SerializedFunc<int, GameObject> eventT1FromMethodName;
        private SerializedFunc<int, GameObject> eventT1FromMethodGroup;

        private SerializedFunc<GameObject, float, int, string, GameObject> eventT4FromMethodName;
        private SerializedFunc<GameObject, float, int, string, GameObject> eventT4FromMethodGroup;

        private TestClass testClass;

        [SetUp]
        public void SetUp()
        {
            testClass = new GameObject()
                .AddComponent<TestClass>();

            {
                eventFromMethodName = new SerializedFunc<float>();
                eventFromMethodName.AddListener(testClass, nameof(TestClass.GetValue));

                eventFromMethodGroup = new SerializedFunc<float>();
                eventFromMethodGroup.AddListener(testClass.GetValue);
            }

            {
                eventT1FromMethodName = new SerializedFunc<int, GameObject>();
                eventT1FromMethodName.AddListener(testClass, nameof(TestClass.GetGameObject));

                eventT1FromMethodGroup = new SerializedFunc<int, GameObject>();
                eventT1FromMethodGroup.AddListener(testClass.GetGameObject);
            }

            {
                eventT4FromMethodName = new SerializedFunc<GameObject, float, int, string, GameObject>();
                eventT4FromMethodName.AddListener(testClass, nameof(TestClass.GetGameObject));

                eventT4FromMethodGroup = new SerializedFunc<GameObject, float, int, string, GameObject>();
                eventT4FromMethodGroup.AddListener(testClass.GetGameObject);
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
                    testClass.GetValue();
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
                    testClass.GetGameObject(10);
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
                    testClass.GetGameObject(testClass.gameObject, 3.14f, 2, "Test");
                })
                .WarmupCount(WARM_UP_COUNT)
                .IterationsPerMeasurement(ITERATION_COUNT_PER_MEASUREMENT)
                .MeasurementCount(MEASUREMENT_COUNT)
                .Run();
        }

        [Test, Performance]
        public void SerializedFunc_FromMethodName_NoArguments_Performance()
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
        public void SerializedFunc_FromMethodGroup_NoArguments_Performance()
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
        public void SerializedFunc_FromMethodName_OneArgument_Performance()
        {
            Measure
                .Method(() =>
                {
                    eventT1FromMethodName.Invoke(10);
                })
                .WarmupCount(WARM_UP_COUNT)
                .IterationsPerMeasurement(ITERATION_COUNT_PER_MEASUREMENT)
                .MeasurementCount(MEASUREMENT_COUNT)
                .Run();
        }

        [Test, Performance]
        public void SerializedFunc_FromMethodGroup_OneArgument_Performance()
        {
            Measure
                .Method(() =>
                {
                    eventT1FromMethodGroup.Invoke(10);
                })
                .WarmupCount(WARM_UP_COUNT)
                .IterationsPerMeasurement(ITERATION_COUNT_PER_MEASUREMENT)
                .MeasurementCount(MEASUREMENT_COUNT)
                .Run();
        }

        [Test, Performance]
        public void SerializedFunc_FromMethodName_FourArguments_Performance()
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
        public void SerializedFunc_FromMethodGroup_FourArguments_Performance()
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

        [Test]
        public void SerializedFunc()
        {
            SerializedFunc<float> func = new SerializedFunc<float>();

            Assert.DoesNotThrow(() =>
            {
                func.Invoke();
            });

            func.AddListener(testClass, nameof(TestClass.GetValue));

            Assert.DoesNotThrow(() =>
            {
                func.Invoke();
            });
            Assert.AreEqual(TestClass.VALUE, func.Invoke());

            func.RemoveListener(testClass, nameof(TestClass.GetValue));

            Assert.DoesNotThrow(() =>
            {
                func.Invoke();
            });
        }
    }
}
