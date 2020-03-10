using System.Globalization;
using IoC.Attributes;

namespace IoC.Tests
{
    class ClassWithNoExplicitConstructors
    {}

    class ClassWithOneParamConstructor
    {
        private ClassWithNoExplicitConstructors A { get; }
        
        public ClassWithOneParamConstructor(ClassWithNoExplicitConstructors a)
        {
            A = a;
        }
    }

    class ClassWithExplicitParameterlessConstructor
    {
        private bool? Invoked { get; set; }

        public ClassWithExplicitParameterlessConstructor()
        {
            Invoked = true;
        }
    }

    interface IMaterial
    {
    }

    class Plastic : IMaterial
    {
        
    }

    class Toy : Plastic
    {
        public Plastic Material { get; }
        
        public Toy(Plastic material)
        {
            Material = material;
        }
    }

    class Pet
    { }

    class Animal
    {
        [Init]
        public Animal parent { get; set; }
    }

    interface INoRelization
    {
    } 
}