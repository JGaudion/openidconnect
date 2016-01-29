﻿using IdentityServer3.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IdentityServer3.Core.Models;
using IdentityServer3.Core.Validation;
using IdentityServer3.Core.ViewModels;
using System.IO;
using System.ComponentModel;

namespace OpenIDConnect.IdentityServer.Services
{
    public class CustomViewService : IViewService
    {
        IClientStore clientStore;
        public CustomViewService(IClientStore clientStore)
        {
            this.clientStore = clientStore;
        }

        public Task<Stream> ClientPermissions(ClientPermissionsViewModel model)
        {
            return Render(model, "Permissions");
        }

        public Task<Stream> Consent(ConsentViewModel model, ValidatedAuthorizeRequest authorizeRequest)
        {
            throw new NotImplementedException();
        }

        public Task<Stream> Error(ErrorViewModel model)
        {
            return Render(model, "Error");
        }

        public Task<Stream> LoggedOut(LoggedOutViewModel model, SignOutMessage message)
        {
            return Render(model, "LoggedOut");
        }
        /// <summary>
        /// Gets the currently running client app to put it's name in the login page
        /// </summary>
        /// <param name="model"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task<Stream> Login(LoginViewModel model, SignInMessage message)
        {
            var client = await clientStore.FindClientByIdAsync(message.ClientId);
            var name = client != null ? client.ClientName : null;
            return await Render(model, "login", name);
            
        }

        public Task<Stream> Logout(LogoutViewModel model, SignOutMessage message)
        {
            return Render(model, "Logout");
        }

        #region Rendering the custom html
        protected virtual Task<System.IO.Stream> Render(CommonViewModel model, string page, string clientName = null)
        {
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(model, Newtonsoft.Json.Formatting.None, new Newtonsoft.Json.JsonSerializerSettings() { ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver() });

            string html = LoadHtml(page);
            html = Replace(html, new
            {
                siteName = SoftwareKobo.Net.WebUtility.HtmlEncode(model.SiteName),
                model = SoftwareKobo.Net.WebUtility.HtmlEncode(json),
                clientName = clientName
            });

            return Task.FromResult(StringToStream(html));
        }
        string Replace(string value, IDictionary<string, object> values)
        {
            foreach (var key in values.Keys)
            {
                var val = values[key];
                val = val ?? String.Empty;
                if (val != null)
                {
                    value = value.Replace("{" + key + "}", val.ToString());
                }
            }
            return value;
        }

        private string LoadHtml(string name)
        {
            //Hardcoded for now
            var file = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"CustomView");
            file = Path.Combine(file, name + ".html");
            return File.ReadAllText(file);
        }

        string Replace(string value, object values)
        {
            return Replace(value, Map(values));
        }

        IDictionary<string, object> Map(object values)
        {
            var dictionary = values as IDictionary<string, object>;

            if (dictionary == null)
            {
                dictionary = new Dictionary<string, object>();
                foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(values))
                {
                    dictionary.Add(descriptor.Name, descriptor.GetValue(values));
                }
            }

            return dictionary;
        }

        Stream StringToStream(string s)
        {
            var ms = new MemoryStream();
            var sw = new StreamWriter(ms);
            sw.Write(s);
            sw.Flush();
            ms.Seek(0, SeekOrigin.Begin);
            return ms;
        }
        #endregion
    }
}
