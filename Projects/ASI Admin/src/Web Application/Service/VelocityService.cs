using asi.asicentral.web.Interface;
using asi.asicentral.web.model.velocity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace asi.asicentral.web.Service
{
    public class VelocityService : IVelocityService
    {
        private readonly IVelocityContext _velocityContext;
        public VelocityService(IVelocityContext velocityContext)
        {
            _velocityContext = velocityContext;
        }

        public virtual bool MapColor(ColorMapping colorMapping)
        {
            return _velocityContext.MapColor(colorMapping);
        }
    }
}