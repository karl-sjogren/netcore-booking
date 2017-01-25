
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Razor.Directives;
using Microsoft.AspNetCore.Razor.Chunks;
using Microsoft.AspNetCore.Razor.Compilation.TagHelpers;

namespace RazorComponents {
    public class CustomRazorHost : MvcRazorHost {
        private IHostingEnvironment _hostingEnvironment;

        public CustomRazorHost(IChunkTreeCache chunkTreeCache, ITagHelperDescriptorResolver tagHelperDescriptorResolver, IHostingEnvironment hostingEnvironment) : base(chunkTreeCache,tagHelperDescriptorResolver) {                         
            _hostingEnvironment = hostingEnvironment;
        }

        public override IReadOnlyList<Chunk> DefaultInheritedChunks {
            get { 
                var def = base.DefaultInheritedChunks.ToList();
 
                if(_hostingEnvironment.EnvironmentName == "Development")
                    def.Add(new AddTagHelperChunk  { LookupText = "*, netcore-booking" });
                else
                    def.Add(new AddTagHelperChunk  { LookupText = "*, app" });
 
                return def;
            }
        }
    }
}