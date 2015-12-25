using Microsoft.AspNet.Http;
using Microsoft.AspNet.Http.Features;
using System.Linq;

namespace AppMvc6.Infrastructure
{
    public class MySession
    {
        private ISession _session;
        /// <summary>
        /// セッションをインデックス付きプロパティで取得・設定可能にするクラス
        /// </summary>
        public MySession(HttpContext context)
        {
            _session = context.Session;
        }
        public string this[string key]
        {
            set { _session.SetString(key, value ?? ""); }
            get { return _session.Keys.Contains(key) ? _session.GetString(key) : null; }
        }
    }
}
