using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;
using System.Web.Hosting;

namespace asi.asicentral.views
{
	public class CustomVirtualPathProvider : VirtualPathProvider
	{

		private bool IsEmbeddedResourcePath(string virtualPath)
		{
			var checkPath = VirtualPathUtility.ToAppRelative(virtualPath);
			return checkPath.StartsWith("~/asicentral/", StringComparison.InvariantCultureIgnoreCase);
		}

		public override bool FileExists(string virtualPath)
		{
			return IsEmbeddedResourcePath(virtualPath) || base.FileExists(virtualPath);
		}

		public override VirtualFile GetFile(string virtualPath)
		{
			if (IsEmbeddedResourcePath(virtualPath))
			{
				return new CustomVirtualFile(virtualPath);
			}
			else
			{
				return base.GetFile(virtualPath);
			}
		}

		public override CacheDependency GetCacheDependency( string virtualPath, IEnumerable virtualPathDependencies, DateTime utcStart)
		{
			if (IsEmbeddedResourcePath(virtualPath))
			{
				return null;
			}
			else
			{
				return base.GetCacheDependency(virtualPath, virtualPathDependencies, utcStart);
			}
		}
	}
}
