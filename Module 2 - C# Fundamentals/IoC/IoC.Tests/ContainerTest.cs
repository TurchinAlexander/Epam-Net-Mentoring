using NUnit;
using NUnit.Framework;

namespace IoC.Tests
{
    [TestFixture]
    public class ContainerTest
    {
        private Container Container;

        [SetUp]
        public void BeforeEach()
        {
            Container = new Container();
        }

        [TearDown]
        public void AfterEach()
        {
            Container = null;
        }

        [Test]
        public void GetInstance_CreateInstanceWithNoParams_InstanceCreated()
        {
            var subject = (ClassWithNoExplicitConstructors) Container.GetInstance(typeof(ClassWithNoExplicitConstructors));
            
            Assert.IsInstanceOf<ClassWithNoExplicitConstructors>(subject);
        }

        [Test]
        public void GetInstance_CreateInstanceWithParams_InstanceCreated()
        {
            var subject = (ClassWithOneParamConstructor) Container.GetInstance(typeof(ClassWithOneParamConstructor));
            
            Assert.IsInstanceOf<ClassWithOneParamConstructor>(subject);
        }

        [Test]
        public void GetInstance_CreateInstanceWithExplicitParameterlessConstructor_InstanceCreated()
        {
            var subject = (ClassWithExplicitParameterlessConstructor) Container.GetInstance(typeof(ClassWithExplicitParameterlessConstructor));
            
            Assert.IsInstanceOf<ClassWithExplicitParameterlessConstructor>(subject);
        }

        [Test]
        public void GetInstance_CreateInstanceUsingGenericMethod_InstanceCreated()
        {
            var subject = Container.GetInstance<ClassWithNoExplicitConstructors>();
            
            Assert.IsInstanceOf<ClassWithNoExplicitConstructors>(subject);
        }

        [Test]
        public void Register_CreateInstanceFromInterface()
        {
            Container.Register<IMaterial, Plastic>();
            
            var subject = Container.GetInstance<IMaterial>();
            
            Assert.IsInstanceOf<Plastic>(subject);
            Assert.IsInstanceOf<IMaterial>(subject);
        }

        [Test]
        public void Register_InitializeObjectWthDependencies_ObjectCreated()
        {
            Container.Register<IMaterial, Toy>();

            var subject = Container.GetInstance<IMaterial>();
            
            Assert.IsInstanceOf<Toy>(subject);
        }

        [Test]
        public void Register_RegisterAsSingleton_ReturnSameInstance()
        {
            Container.RegisterSingleton<Pet>();
            
            var subject1 = Container.GetInstance<Pet>();
            var subject2 = Container.GetInstance<Pet>();

            Assert.AreSame(subject1, subject2);
        }
    }
}