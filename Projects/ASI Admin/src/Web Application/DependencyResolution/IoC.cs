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
using asi.asicentral.services.interfaces;
using StructureMap;
using StructureMap.Configuration.DSL;
namespace asi.asicentral.web.DependencyResolution {
    public static class IoC {
        public static StructureMap.IContainer Initialize() {
            ObjectFactory.Initialize(x =>
                        {
                            x.For<Registry>()
                                .Use<EFRegistry>();

                            x.For<asi.asicentral.services.interfaces.IContainer>()
                                .Singleton()
                                .Use<asi.asicentral.services.Container>()
                                .Ctor<Registry>();

                            x.For<IObjectService>()
                                .HttpContextScoped()
                                .Use<ObjectService>()
                                .Ctor<asi.asicentral.services.interfaces.IContainer>();
                        });
            return ObjectFactory.Container;
        }
    }
}