﻿/*
 * Copyright 2016 Amazon.com, Inc. or its affiliates. All Rights Reserved.
 * 
 * Licensed under the Apache License, Version 2.0 (the "License").
 * You may not use this file except in compliance with the License.
 * A copy of the License is located at
 * 
 *  http://aws.amazon.com/apache2.0
 * 
 * or in the "license" file accompanying this file. This file is distributed
 * on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either
 * express or implied. See the License for the specific language governing
 * permissions and limitations under the License.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;

using Amazon.Runtime;

using Amazon.Extensions.NETCore.DependencyInjection;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// This class adds extension methods to IServiceCollection making it easier to add Amazon service clients
    /// to the NET Core dependency injection framework.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the AWSOptions object to the dependency injection framework providing information
        /// that will be used to construct Amazon service clients.
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static IServiceCollection AddAWSOptions(this IServiceCollection collection, AWSOptions options)
        {
            collection.Add(new ServiceDescriptor(typeof(AWSOptions), options));
            return collection;
        }

        /// <summary>
        /// Adds the Amazon service client to the dependency injection framework. The Amazon service client is not
        /// created until it is requested. If the ServiceLifetime property is set to Singleon, the default, then the same
        /// instance will be reused for the lifetime of the process and the object should not be disposed.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <param name="lifetime"></param>
        /// <returns></returns>
        public static IServiceCollection AddAWSService<T>(this IServiceCollection collection, ServiceLifetime lifetime = ServiceLifetime.Singleton) where T : IAmazonService
        {
            Func<IServiceProvider, object> factory =
                new ClientFactory(typeof(T)).CreateServiceFactory;

            var descriptor = new ServiceDescriptor(typeof(T), factory, lifetime);
            collection.Add(descriptor);
            return collection;
        }
    }
}
