using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Server.Shared.Utils
{
    public class ServerHttpClient
    {
        private bool _autoRedirect;
        private readonly Uri _url;
        private readonly Dictionary<string, string> _form;
        private readonly LinkedList<(string key, string val)> _headers;
        
        public ServerHttpClient(Uri url)
        {
            _url = url;
            _autoRedirect = false;
            _form = new Dictionary<string, string>();
            _headers = new LinkedList<(string key, string val)>();
            _headers.AddFirst(("User-Agent",
                "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:61.0) Gecko/20100101 Firefox/61.0"));
        }

        /// <summary>
        /// 添加Form数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public ServerHttpClient Form(string key, string value)
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
        public ServerHttpClient AllowAutoRedirect()
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
        public ServerHttpClient Header(string key, string value)
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
        public ServerHttpClient Cookie(string cookie)
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
        public async Task<string> GetBodyAsync()
        {
            var res = await CreateHttpClient().GetAsync(_url);
            return await res.Content.ReadAsStringAsync();
        }

        /// <summary>
        /// Post HttpBody
        /// </summary>
        /// <returns></returns>
        public async Task<string> PostBodyAsync()
        {
            var res = await CreateHttpClient().PostAsync(_url, new FormUrlEncodedContent(_form));
            return await res.Content.ReadAsStringAsync();
        }

        /// <summary>
        /// 创建请求Client
        /// </summary>
        /// <returns></returns>
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

    public static class Extension
    {
        /// <summary>
        /// 添加Form数据
        /// </summary>
        /// <param name="url"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static ServerHttpClient Form(this Uri url, string key, string value)
        {
            return new ServerHttpClient(url).Form(key, value);
        }

        /// <summary>
        /// 添加Form数据
        /// </summary>
        /// <param name="url"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static ServerHttpClient Form(this string url, string key, string value)
        {
            return new ServerHttpClient(new Uri(url)).Form(key, value);
        }

        /// <summary>
        /// 允许自动跳转
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static ServerHttpClient AllowAutoRedirect(this Uri url)
        {
            return new ServerHttpClient(url).AllowAutoRedirect();
        }

        /// <summary>
        /// 允许自动跳转
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static ServerHttpClient AllowAutoRedirect(this string url)
        {
            return new ServerHttpClient(new Uri(url)).AllowAutoRedirect();
        }

        /// <summary>
        /// 加上头
        /// </summary>
        /// <param name="url"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static ServerHttpClient Header(this Uri url, string key, string value)
        {
            return new ServerHttpClient(url).Header(key, value);
        }

        /// <summary>
        /// 加上头
        /// </summary>
        /// <param name="url"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static ServerHttpClient Header(this string url, string key, string value)
        {
            return new ServerHttpClient(new Uri(url)).Header(key, value);
        }

        /// <summary>
        /// 加上Cookie
        /// </summary>
        /// <param name="url"></param>
        /// <param name="cookie"></param>
        /// <returns></returns>
        public static ServerHttpClient Cookie(this Uri url, string cookie)
        {
            return new ServerHttpClient(url).Cookie(cookie);
        }

        /// <summary>
        /// 加上Cookie
        /// </summary>
        /// <param name="url"></param>
        /// <param name="cookie"></param>
        /// <returns></returns>
        public static ServerHttpClient Cookie(this string url, string cookie)
        {
            return new ServerHttpClient(new Uri(url)).Cookie(cookie);
        }

        /// <summary>
        /// 发起Get请求
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static async Task<HttpResponseMessage> GetAsync(this Uri url)
        {
            return await new ServerHttpClient(url).GetAsync();
        }

        /// <summary>
        /// 发起Get请求
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static async Task<HttpResponseMessage> GetAsync(this string url)
        {
            return await new ServerHttpClient(new Uri(url)).GetAsync();
        }

        /// <summary>
        /// 发起Post请求
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static async Task<HttpResponseMessage> PostAsync(this Uri url)
        {
            return await new ServerHttpClient(url).PostAsync();
        }

        /// <summary>
        /// 发起Post请求
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static async Task<HttpResponseMessage> PostAsync(this string url)
        {
            return await new ServerHttpClient(new Uri(url)).PostAsync();
        }

        /// <summary>
        /// Get HttpBody
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static async Task<string> GetBodyAsync(this Uri url)
        {
            return await new ServerHttpClient(url).GetBodyAsync();
        }

        /// <summary>
        /// Post HttpBody
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static async Task<string> PostBodyAsync(this Uri url)
        {
            return await new ServerHttpClient(url).PostBodyAsync();
        }

        /// <summary>
        /// Get HttpBody
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static async Task<string> GetBodyAsync(this string url)
        {
            return await new ServerHttpClient(new Uri(url)).GetBodyAsync();
        }

        /// <summary>
        /// Post HttpBody
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static async Task<string> PostBodyAsync(this string url)
        {
            return await new ServerHttpClient(new Uri(url)).PostBodyAsync();
        }

    }
}