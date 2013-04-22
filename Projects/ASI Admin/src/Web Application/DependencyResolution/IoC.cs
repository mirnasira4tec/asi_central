// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IoC.cs" company="Web Advanced">
// Copyright 2012 Web Advanced (www.webadvanced.com)
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


using asi.asicentral.database.mappings;
using asi.asicentral.services;
using asi.asicentral.interfaces;
using StructureMap;
using StructureMap.Configuration.DSL;
using Castle.DynamicProxy;
using asi.asicentral.util;
using System.Web.Mvc;
namespace asi.asicentral.web.DependencyResolution
{
    public static class IoC
    {
        public static StructureMap.IContainer Initialize()
        {
            ProxyGenerator proxyGenerator = new ProxyGenerator();

            ObjectFactory.Initialize(x =>
                {
                    x.For<IEncryptionService>()
                        .Use<EncryptionService>()
                        .EnrichWith(encryption => proxyGenerator.CreateClassProxyWithTarget(encryption, new IInterceptor[] { new LogInterceptor(encryption.GetType()) }));

                    x.For<Registry>()
                        .Use<EFRegistry>()
                        .EnrichWith(registry => proxyGenerator.CreateClassProxyWithTarget(registry, new IInterceptor[] { new LogInterceptor(registry.GetType()) }));

                    x.For<asi.asicentral.interfaces.IContainer>()
                        .Singleton()
                        .Use<asi.asicentral.services.Container>()
                        .Ctor<Registry>();

                    x.For<IObjectService>()
                        .HttpContextScoped()
                        .Use<ObjectService>()
                        .EnrichWith(objectService => proxyGenerator.CreateClassProxyWithTarget(objectService.GetType(), objectService, new object[] { null }, new IInterceptor[] { new LogInterceptor(objectService.GetType()) }))
                        .Ctor<asi.asicentral.interfaces.IContainer>();

                    x.For<IStoreService>()
                        .HttpContextScoped()
                        .Use<StoreService>()
                        .EnrichWith(storeService => proxyGenerator.CreateClassProxyWithTarget(storeService.GetType(), storeService, new object[] { null }, new IInterceptor[] { new LogInterceptor(storeService.GetType()) }))
                        .Ctor<asi.asicentral.interfaces.IContainer>();

                    x.For<IFulfilmentService>()
                        .HttpContextScoped()
                        .Use<TIMSSService>()
                        .EnrichWith(timssService => proxyGenerator.CreateClassProxyWithTarget(timssService.GetType(), timssService, new object[] { null }, new IInterceptor[] { new LogInterceptor(timssService.GetType()) }))
                        .Ctor<IObjectService>();

                    x.For<ICreditCardService>()
                        .Use<asi.asicentral.services.CreditCardService>()
                        .EnrichWith(cardService => proxyGenerator.CreateClassProxyWithTarget(cardService.GetType(), cardService, new IInterceptor[] { new LogInterceptor(cardService.GetType()) }));

                    x.SetAllProperties(instance => instance.OfType<IObjectService>());
                    x.SetAllProperties(instance => instance.OfType<IStoreService>());
                    x.SetAllProperties(instance => instance.OfType<IEncryptionService>());
                    x.SetAllProperties(instance => instance.OfType<IFulfilmentService>());
                    x.SetAllProperties(instance => instance.OfType<ICreditCardService>());

                    x.For<IController>()
                        .EnrichAllWith(controller => proxyGenerator.CreateInterfaceProxyWithTarget(controller, new IInterceptor[] { new LogInterceptor(controller.GetType()) }));
                });
            return ObjectFactory.Container;
        }
    }
}