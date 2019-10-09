﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Hermes.Extensions
{
    public static class AssemblyExtensions
    {
        public static IEnumerable<Type> GetMappingTypes(this Assembly assembly, Type mappingInterface)
        {
            return assembly
                .GetTypes()
                .Where(x =>
                    !x.GetTypeInfo().IsAbstract &&
                    x.GetInterfaces().Any(y => y.GetTypeInfo().IsGenericType &&
                                               y.GetGenericTypeDefinition() == mappingInterface));
        }
    }
}