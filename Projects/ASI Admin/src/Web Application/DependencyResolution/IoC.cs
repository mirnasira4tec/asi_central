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
using System.Reflection;
using asi.asicentral.web.Interface;
using asi.asicentral.web.database;
using asi.asicentral.web.Service;
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

                    x.For<IROIService>()
                        .HttpContextScoped()
                        .Use<ROIService>()
                        .EnrichWith(roiService => proxyGenerator.CreateClassProxyWithTarget(roiService.GetType(), roiService, new object[] { null }, new IInterceptor[] { new LogInterceptor(roiService.GetType()) }));

					x.For<IBackendService>()
						.Use<PersonifyService>()
						.EnrichWith(backendService => proxyGenerator.CreateClassProxyWithTarget(backendService.GetType(), backendService, new object[] { null }, new IInterceptor[] { new LogInterceptor(backendService.GetType()) }))
						.Ctor<IStoreService>();

                    x.For<IFileSystemService>()
                        .Singleton()
                        .Use<AssemblyFileService>()
                        .EnrichWith(fileService => proxyGenerator.CreateClassProxyWithTarget(fileService.GetType(), fileService, new object[] { new Assembly[] { Assembly.GetAssembly(typeof(IoC)), Assembly.GetAssembly(typeof(PersonifyService)) } }, new IInterceptor[] { new LogInterceptor(fileService.GetType()) }))
                        .Ctor<Assembly[]>("assemblies").Is(new Assembly[] { Assembly.GetAssembly(typeof(IoC)), Assembly.GetAssembly(typeof(PersonifyService)) });

                    x.For<ITemplateService>()
                       .Singleton()
                       .Use<RazorTemplateEngine>()
                       .EnrichWith(templateService => proxyGenerator.CreateClassProxyWithTarget(templateService.GetType(), templateService, new object[] { null }, new IInterceptor[] { new LogInterceptor(templateService.GetType()) }))
                       .Ctor<IFileSystemService>();

                    x.For<IEmailService>()
                       .Use<QueueMailService>()
                       .EnrichWith(emailService => proxyGenerator.CreateClassProxyWithTarget(emailService.GetType(), emailService, new IInterceptor[] { new LogInterceptor(emailService.GetType()) }));

                    x.For<IImageConvertService>()
                       .Use<ImageConvertService>()
                       .EnrichWith(imageService => proxyGenerator.CreateClassProxyWithTarget(imageService.GetType(), imageService, new IInterceptor[] { new LogInterceptor(imageService.GetType()) }));

                    x.For<IVelocityContext>()
                        .Use<VelocityContext>();
                    
                    x.For<IVelocityService>()
                        .Use<VelocityService>();

                    x.SetAllProperties(instance => instance.OfType<IObjectService>());
                    x.SetAllProperties(instance => instance.OfType<IStoreService>());
                    x.SetAllProperties(instance => instance.OfType<IEncryptionService>());
                    x.SetAllProperties(instance => instance.OfType<IFulfilmentService>());
                    x.SetAllProperties(instance => instance.OfType<IFileSystemService>());
                    x.SetAllProperties(instance => instance.OfType<ITemplateService>());
                    x.SetAllProperties(instance => instance.OfType<IEmailService>());
                    x.SetAllProperties(instance => instance.OfType<IBackendService>());
                    x.SetAllProperties(instance => instance.OfType<IVelocityService>());

                    //x.For<IController>()
                    //    .EnrichAllWith(controller => proxyGenerator.CreateInterfaceProxyWithTarget(controller, new IInterceptor[] { new LogInterceptor(controller.GetType()) }));
                });
            return ObjectFactory.Container;
        }
    }
}