using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Server.Shared.Utils
{
    public class ServerRequest
    {
        private bool _autoRedirect;
        private readonly Uri _url;
        private readonly Dictionary<string, string> _form;
        private readonly LinkedList<(string key, string val)> _headers;


        private ServerRequest(Uri url)
        {
            _url = url;
            _autoRedirect = false;
            _form = new Dictionary<string, string>();
            _headers = new LinkedList<(string key, string val)>();
            _headers.AddFirst(("User-Agent",
                "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:61.0) Gecko/20100101 Firefox/61.0"));
        }

        private ServerRequest(string url) : this(new Uri(url))
        {
        }

        /// <summary>
        /// 工厂方法
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static ServerRequest Request(string url)
        {
            return new ServerRequest(url);
        }

        /// <summary>
        /// 工厂方法
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static ServerRequest Request(Uri url)
        {
            return new ServerRequest(url);
        }

        /// <summary>
        /// 添加Form数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public ServerRequest Form(string key, string value)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new Exception("键不可为null或空格");
            if (_form.ContainsKey(key))
                _form[key] = value;
            else
                _form.Add(key, value);
            return this;
        }

        /// <summary>
        /// 允许默认跳转
        /// </summary>
        /// <returns></returns>
        public ServerRequest AllowAutoRedirect()
        {
            _autoRedirect = true;
            return this;
        }

        /// <summary>
        /// 添加头
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public ServerRequest Header(string key, string value)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new Exception("键不可为null或空格");
            _headers.AddLast((key, value));
            return this;
        }

        /// <summary>
        /// 添加Cookie
        /// </summary>
        /// <param name="cookie"></param>
        /// <returns></returns>
        public ServerRequest Cookie(string cookie)
        {
            if (string.IsNullOrWhiteSpace(cookie))
                throw new Exception("cookie不可为null或空格");
            _headers.AddLast(("Cookie", cookie));
            return this;
        }

        /// <summary>
        /// HttpGet
        /// </summary>
        /// <returns></returns>
        public async Task<HttpResponseMessage> GetAsync()
        {
            return await CreateHttpClient().GetAsync(_url);
        }

        /// <summary>
        /// HttpPost
        /// </summary>
        /// <returns></returns>
        public async Task<HttpResponseMessage> PostAsync()
        {
            return await CreateHttpClient().PostAsync(_url, new FormUrlEncodedContent(_form));
        }

        /// <summary>
        /// Get HttpBody
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetStringAsync()
        {
            var res = await CreateHttpClient().GetAsync(_url);
            return await res.Content.ReadAsStringAsync();
        }

        /// <summary>
        /// Post HttpBody
        /// </summary>
        /// <returns></returns>
        public async Task<string> PostStringAsync()
        {
            var res = await CreateHttpClient().PostAsync(_url, new FormUrlEncodedContent(_form));
            return await res.Content.ReadAsStringAsync();
        }

        private HttpClient CreateHttpClient()
        {
            var client = new HttpClient(new HttpClientHandler
            {
                AllowAutoRedirect = _autoRedirect
            });
            foreach (var (key, val) in _headers)
            {
                client.DefaultRequestHeaders.Add(key, val);
            }

            return client;
        }
    }
}