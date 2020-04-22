using System;
using Xunit;

namespace FuseBox.Tests
{
    public class ContainerTests
    {
        private readonly Container _sut = new Container();

        [Fact]
        public void Resolve_TypeHasParameterlessConstructor_GetsAnInstance()
        {
            var instance = _sut.Resolve(typeof(A));

            Assert.IsType<A>(instance);
        }

        [Fact]
        public void Resolve_TypeIsNull_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => _sut.Resolve(null));
        }

        [Fact]
        public void Resolve_TypeHasPrivateConstructorOnly_Throws()
        {
            Assert.Throws<UnresolvableTypeException>(() => _sut.Resolve(typeof(OnlyPrivateConstructor)));
        }

        [Fact]
        public void Resolve_TypeHasDependency_GetsAnInstanceOfType()
        {
            var instance = _sut.Resolve(typeof(B));

            Assert.IsType<B>(instance);
        }

        [Fact]
        public void Resolve_TypeHasMultipleConstructors_UsedTheOneWithMostParameters()
        {
            var instance = _sut.Resolve(typeof(C));

            Assert.IsType<C>(instance);
            Assert.True(((C)instance).ConstructorInvoked);
        }

        [Theory]
        [InlineData(typeof(MultipleConstructorsOneValid))]
        [InlineData(typeof(MultipleConstructorsOneValid2))]
        public void Resolve_TypeHasOneValidAndOneInvalidConstructor_ResolvesTheValidOne(Type type)
        {
            var instance = _sut.Resolve(type);

            Assert.IsType(type, instance);
        }

        [Fact]
        public void Resolve_TypeDependsOnPrimitive_Throws()
        {
            Assert.ThrowsAny<UnresolvableTypeException>(() => _sut.Resolve(typeof(OnlyPrimitives)));
        }

        [Fact]
        public void Resolve_TypeHasMultipleLevelsOfDependency_ResolvesAnInstanceOfType()
        {
            var instance = _sut.Resolve(typeof(D));

            Assert.IsType<D>(instance);
        }
        private class A
        {
        }

        private class B
        {
            public B(A a)
            {
            }
        }

        private class C
        {
            public C()
            {
            }

            public C(A a, B b)
            {
                ConstructorInvoked = true;
            }

            public bool ConstructorInvoked { get; }
        }

        private class D
        {
            public D(C c)
            {
            }
        }

        private class OnlyPrivateConstructor
        {
            private OnlyPrivateConstructor()
            {
            }
        }

        private class OnlyPrimitives
        {
            public OnlyPrimitives(int a)
            {
            }
        }

        private class MultipleConstructorsOneValid
        {
            public MultipleConstructorsOneValid(int a, string b, bool c)
            {
            }

            public MultipleConstructorsOneValid()
            {
            }
        }

        private class MultipleConstructorsOneValid2
        {
            public MultipleConstructorsOneValid2(int a, string b, bool c)
            {
            }

            public MultipleConstructorsOneValid2(A a, B b, C c)
            {
            }
        }
    }
}
