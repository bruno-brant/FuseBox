// Copyright (c) Bruno Brant. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace FuseBox.Tests.ContainerTests
{
    public class Iteration3Tests
    {
        private readonly Container _sut = new Container();

        [Fact]
        public void Register_RequestedIsNull_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentNullException>(() => _sut.Register(null, typeof(A)));
        }

        [Fact]
        public void Register_ResolvedIsNull_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentNullException>(() => _sut.Register(typeof(A), null));
        }

        [Fact]
        public void Register_ResolvesIsntAssignableToRequired_ThrowsMappingException()
        {
            var ex = Assert.Throws<ImproperMappingException>(() => _sut.Register(typeof(A), typeof(B)));

            Assert.Equal(typeof(A), ex.Requested);
            Assert.Equal(typeof(B), ex.Resolves);
        }

        [Fact]
        public void Resolve_RequestedIsInterfaceAndIsMapped_ReturnsInstanceOfMapping()
        {
            _sut.Register(typeof(IC), typeof(C));

            var instance = _sut.Resolve(typeof(IC));

            Assert.IsType<C>(instance);
        }

        [Fact]
        public void Resolve_RequestedIsAbstractAndIsMapped_ReturnsInstanceOfMapping()
        {
            _sut.Register(typeof(D), typeof(E));

            var instance = _sut.Resolve(typeof(D));

            Assert.IsType<E>(instance);
        }

        [Fact]
        public void Resolve_ResolvesInheritsRequested_ReturnsInstanceOfMapping()
        {
            _sut.Register(typeof(F), typeof(G));

            var instance = _sut.Resolve(typeof(F));

            Assert.IsType<G>(instance);
        }

        [Fact]
        public void Register_MappingAlreadyMade_Throws()
        {
            _sut.Register(typeof(F), typeof(G));

            Assert.Throws<DuplicatedMappingException>(() => _sut.Register(typeof(F), typeof(G)));
        }

        [Theory]
        [InlineData(typeof(IH), typeof(I))]
        [InlineData(typeof(IH), typeof(IJ))]
        public void Register_ResolvesIsntInstatiable_Throws(Type request, Type resolves)
        {
            Assert.Throws<UninstantiableTypeException>(() => _sut.Register(request, resolves));
        }

        [Fact]
        public void Resolve_TypeIsAbstract_ThrowsUnmappedTypeException()
        {
            Assert.Throws<UnmappedTypeException>(() => _sut.Resolve(typeof(IC)));
        }

        [Fact]
        public void Resolve_TypeHasAMappedDependency_ReturnsCorrectInstance()
        {
            _sut.Register(typeof(IL), typeof(L));

            var actual = _sut.Resolve(typeof(K));

            Assert.IsType<K>(actual);
        }

        [Fact]
        public void Resolve_ConstructorHasUnmappedType_Throws()
        {
            var ex = Assert.Throws<UnmappedTypeException>(() => _sut.Resolve(typeof(M)));

            Assert.Equal(typeof(IL), ex.Requested);
        }

        [Fact]
        public void Resolve_ConstructorSecondArgumentUnmappedType_ExceptionInformsCorrectType()
        {
            _sut.Register(typeof(IL), typeof(L));

            var ex = Assert.Throws<UnmappedTypeException>(() => _sut.Resolve(typeof(M)));

            Assert.Equal(typeof(IK), ex.Requested);
        }

        [Fact]
        public void Resolve_WhenThereAreComplexMappings_ReturnsExpectedType()
        {
            _sut.Register(typeof(IN), typeof(N));
            _sut.Register(typeof(IL), typeof(L));
            _sut.Register(typeof(IK), typeof(K));
            _sut.Register(typeof(F), typeof(G));

            var actual = _sut.Resolve(typeof(IN));

            Assert.IsType<N>(actual);
            Assert.IsType<G>(((N)actual).F);
        }

        [Theory]
        [InlineData(typeof(IL), typeof(L))]
        [InlineData(typeof(IP), typeof(P))]
        public void Resolve_MultipleConstructorsSecondHasMappings_ResolveWithSecond(Type request, Type resolve)
        {
            _sut.Register(typeof(IK), typeof(K));
            _sut.Register(request, resolve);

            var actual = _sut.Resolve(typeof(O));

            Assert.IsType<O>(actual);
            Assert.IsType(resolve, ((O)actual).WhatWasResolved);
        }

        [Fact]
        public void Register_MappingWasUnregistered_Allows()
        {
            _sut.Register(typeof(IC), typeof(C));

            Assert.Throws<DuplicatedMappingException>(() => _sut.Register(typeof(IC), typeof(C)));

            _sut.Unregister(typeof(IC));

            _sut.Register(typeof(IC), typeof(C));
        }

        [Fact]
        public void Unregister_RequestedIsNull_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => _sut.Unregister(null));
        }

        [Fact]
        public void Unregister_NoMappingsForType_Throws()
        {
            Assert.Throws<TypeNotRegisteredException>(() => _sut.Unregister(typeof(A)));
        }

        [Fact]
        public void HasMappings_TypeDoesntHaveMappings_ReturnsFalse()
        {
            Assert.False(_sut.IsRegistered(typeof(K)));
        }

        [Fact]
        public void HasMappingTypeHasMappings_ReturnsTrue()
        {
            _sut.Register(typeof(IK), typeof(K));

            Assert.True(_sut.IsRegistered(typeof(IK)));
        }

        [Fact]
        public void TryRegister_TypeDoesntHaveMapping_ReturnsTrue()
        {
            Assert.True(_sut.TryRegister(typeof(IK), typeof(K)));
        }

        [Fact]
        public void TryRegister_TypeHasMapping_ReturnsFalse()
        {
            Assert.True(_sut.TryRegister(typeof(IK), typeof(K)));
            Assert.False(_sut.TryRegister(typeof(IK), typeof(K)));
        }

#pragma warning disable SA1201 // Elements should appear in the correct order
#pragma warning disable IDE0060 // Remove unused parameter

        private class A
        {
        }

        private class B
        {
        }

        private interface IC
        {
        }

        private class C : IC
        {
        }

        private abstract class D
        {
        }

        private class E : D
        {
        }

        private class F
        {
        }

        private class G : F
        {
        }

        private interface IH
        {
        }

        private abstract class I : IH
        {
        }

        private interface IJ : IH
        {
        }

        private interface IK
        {
        }

        private class K : IK
        {
            public K(IL l)
            {
            }
        }

        private interface IL
        {
        }

        private class L : IL
        {
        }

        private class M
        {
            public M(IL l, IK k)
            {
            }
        }

        private interface IN
        {
        }

        private class N : IN
        {
            public N(M m, F f)
            {
                F = f;
            }

            public F F { get; }
        }

        private class O
        {
            public O(IP p)
            {
                WhatWasResolved = p;
            }

            public O(IL l)
            {
                WhatWasResolved = l;
            }

            public object WhatWasResolved { get; }
        }

        private interface IP
        {
        }

        private class P : IP
        {
        }
    }
}
