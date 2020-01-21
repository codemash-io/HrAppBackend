using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeMash.Models;

namespace HrApp.Domain
{
    [Collection("links")]
    public class AssetLink
    {
        public AssetLink(string link)
        {
            this.link = link;
        }

        [Field("asset_link")]
        public string link { get; private set; }
    }
}
