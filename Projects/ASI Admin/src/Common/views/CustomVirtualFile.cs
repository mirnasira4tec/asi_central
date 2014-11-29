using System.IO;
using System.Web;
using System.Web.Hosting;

namespace asi.asicentral.views
{
	public class CustomVirtualFile : VirtualFile {
		private string m_path;

		public CustomVirtualFile(string virtualPath) : base(virtualPath)
		{
			m_path = VirtualPathUtility.ToAppRelative(virtualPath);
		}

		public override Stream Open()
		{
			var parts = m_path.Split('/');
			var assemblyName = parts[1];
			var resourceName = parts[2];

			assemblyName = Path.Combine(HttpRuntime.BinDirectory, assemblyName);
			var assembly = System.Reflection.Assembly.LoadFile(assemblyName + ".dll");

			if (assembly != null)
			{
				return assembly.GetManifestResourceStream(resourceName);
			}
			return null;
		}
	}
}
