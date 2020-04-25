// Copyright (c) Bruno Brant. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using Xunit;

namespace FuseBox.Tests.ContainerTests
{
    public class Iteration2Tests
    {
        private readonly Container _sut = new Container();

        [Fact]
        public void Resolve_TypeHasSingleLevelCyclicDependency_Throws()
        {
            var ex = Assert.Throws<CyclicDependencyException>(() => _sut.Resolve(typeof(A)));
        }

        [Fact]
        public void Resolve_TypeHasSecondLevelCyclicDependency_Throws()
        {
            var ex = Assert.Throws<CyclicDependencyException>(() => _sut.Resolve(typeof(C)));
        }

        [Fact]
        public void Resolve_SecondConstructorHasNoCycles_Resolves()
        {
            var instance = _sut.Resolve(typeof(F));

            Assert.IsType<F>(instance);
        }

#pragma warning disable IDE0060 // Remove unused parameter

        private class A
        {
            public A(B b)
            {
            }
        }

        private class B
        {
            public B(A a)
            {
            }
        }

        private class C
        {
            public C(D d)
            {
            }
        }

        private class D
        {
            public D(E e)
            {
            }
        }

        private class E
        {
            public E(C c)
            {
            }
        }

        private class F
        {
            public F(A a)
            {
            }

            public F(G g)
            {
            }
        }

        private class G
        {
        }
    }
}
