﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Services.Client;
using System.Net;
using System.IO;
using System.Xml.Linq;
using asi.asicentral.PersonifyDataASI;
using Personify.DataServices.Serialization;

namespace asi.asicentral.services.PersonifyProxy
{
    public class SvcClient
    {
        static string sUri = System.Configuration.ConfigurationManager.AppSettings["svcUri"];
        static string EnableBasicAuthentication = System.Configuration.ConfigurationManager.AppSettings["EnableBasicAuthentication"];
        static string UserName = System.Configuration.ConfigurationManager.AppSettings["UserName"];
        static string Password = System.Configuration.ConfigurationManager.AppSettings["Password"];
        static string SourceFormatValue = System.Configuration.ConfigurationManager.AppSettings["CommunicationFormat"];

        static Uri svcUri = new Uri(sUri);

        #region Helpers

        private static PersonifyEntitiesASI ctxt;
        public static PersonifyEntitiesASI Ctxt
        {
            get
            {
                if (ctxt == null)
                {
                    ctxt = new PersonifyEntitiesASI(svcUri);

                    //enable authentication if necessary
                    if (Convert.ToBoolean(EnableBasicAuthentication) == true)
                    {
                        var serviceCreds = new NetworkCredential(UserName, Password);
                        var cache = new CredentialCache();
                        cache.Add(svcUri, "Basic", serviceCreds);
                        ctxt.Credentials = cache;
                    }

                    ctxt.IgnoreResourceNotFoundException = true;
                }
                return ctxt;
            }
        }

        private static PersonifyEntitiesASI ctxtAnonymous;
        public static PersonifyEntitiesASI CtxtAnonymous
        {
            get
            {
                if (ctxtAnonymous == null)
                {
                    ctxtAnonymous = new PersonifyEntitiesASI(svcUri);
                    ctxtAnonymous.IgnoreResourceNotFoundException = true;
                }
                return ctxtAnonymous;
            }
        }

        public static SourceFormatEnum SourceFormatting
        {
            get
            {
                try
                {
                    if (string.IsNullOrEmpty(SourceFormatValue))
                        SourceFormatValue = "XML";

                    return (SourceFormatEnum)Enum.Parse(typeof(SourceFormatEnum), SourceFormatValue, true);
                }
                catch (Exception ex)
                {
                    return SourceFormatEnum.XML;
                }
            }
        }

        public static string ContentType
        {
            get
            {
                if (SourceFormatting == SourceFormatEnum.JSON)
                    return "application/json;charset=utf-8";
                else
                    return "application/xml;charset=utf-8";
            }
        }

        public static ReturnType Post<ReturnType>(string SvcOperName, object o)
        {
            return DoPost<ReturnType>(SvcOperName, o, true);
        }

        public static ReturnType PostAnonymous<ReturnType>(string SvcOperName, object o)
        {
            return DoPost<ReturnType>(SvcOperName, o, false);
        }

        private static ReturnType DoPost<ReturnType>(string SvcOperName, object o, bool enableAuthentication)
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(sUri.TrimEnd('/') + "/" + SvcOperName);
            if (enableAuthentication)
            {
                NetworkCredential serviceCreds = new NetworkCredential(UserName, Password);
                CredentialCache cache = new CredentialCache();
                Uri uri = new Uri(sUri);
                cache.Add(uri, "Basic", serviceCreds);
                req.Credentials = cache;
            }

            req.Method = "POST";
            req.ContentType = ContentType;
            req.Timeout = 1000 * 60 * 15; // 15 minutes

            byte[] arr = o.ToSerializedByteArrayUTF8();
            req.ContentLength = arr.Length;
            Stream reqStrm = req.GetRequestStream();
            reqStrm.Write(arr, 0, arr.Length);
            reqStrm.Close();
            try
            {
                HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
                return GetResponseValue<ReturnType>(resp);
            }
            catch (WebException wex)
            {
                throw DataServiceExceptionUtil.ParseException(wex);
            }
        }

        private static ReturnType GetResponseValue<ReturnType>(HttpWebResponse response)
        {

            ReturnType oEntity;
            string objText;
            objText = GetResponseString(response);
            response.Close();
            if (!string.IsNullOrWhiteSpace(objText))
                oEntity = objText.ToBusinessEntity<ReturnType>(SourceFormatting);
            else
                oEntity = default(ReturnType);
            return oEntity;
        }

        private static string GetResponseString(HttpWebResponse response)
        {
            string objText = string.Empty;
            using (var reader = new StreamReader(response.GetResponseStream()))
            {
                objText = reader.ReadToEnd();
            }
            return objText;
        }

        public static ReturnType Create<ReturnType>()
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(
                string.Format("{0}/Create?EntityName='{1}'",
                    sUri.TrimEnd('/'),
                    typeof(ReturnType).Name)
                    );
            NetworkCredential serviceCreds = new NetworkCredential(UserName, Password);
            CredentialCache cache = new CredentialCache();
            cache.Add(new Uri(sUri), "Basic", serviceCreds);

            req.Credentials = cache;
            req.Method = "GET";
            req.ContentType = ContentType;
            req.Timeout = 1000 * 60 * 15; // 15 minutes

            try
            {
                HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
                return GetResponseValue<ReturnType>(resp);
            }
            catch (WebException wex)
            {
                throw DataServiceExceptionUtil.ParseException(wex);
            }
        }

        public static ReturnType Save<ReturnType>(object entityToSave, string addModOper = null)
        {

            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(
                string.Format("{0}/Save?EntityName='{1}'",
                    sUri.TrimEnd('/'),
                    typeof(ReturnType).Name)
                    );
            NetworkCredential serviceCreds = new NetworkCredential(UserName, Password);
            CredentialCache cache = new CredentialCache();
            Uri uri = new Uri(sUri);
            cache.Add(uri, "Basic", serviceCreds);

            req.Credentials = cache;
            req.Method = "POST";
            req.ContentType = ContentType;
            req.Timeout = 1000 * 60 * 15; // 15 minutes

            if (!string.IsNullOrEmpty(addModOper))
            {
                req.Headers.Add("AddModOper", addModOper);
            }

            byte[] arr = entityToSave.ToSerializedByteArrayUTF8(SourceFormatting);
            req.ContentLength = arr.Length;
            Stream reqStrm = req.GetRequestStream();
            reqStrm.Write(arr, 0, arr.Length);
            reqStrm.Close();

            try
            {
                HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
                return GetResponseValue<ReturnType>(resp);
            }
            catch (WebException wex)
            {
                throw DataServiceExceptionUtil.ParseException(wex);
            }
        }

        public static ReturnType Delete<ReturnType>(object entityToDelete)
        {

            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(
                string.Format("{0}/Delete?EntityName='{1}'",
                    sUri.TrimEnd('/'),
                    typeof(ReturnType).Name)
                    );
            NetworkCredential serviceCreds = new NetworkCredential(UserName, Password);
            CredentialCache cache = new CredentialCache();
            Uri uri = new Uri(sUri);
            cache.Add(uri, "Basic", serviceCreds);

            req.Credentials = cache;
            req.Method = "POST";
            req.ContentType = ContentType;
            req.Timeout = 1000 * 60 * 15; // 15 minutes

            byte[] arr = entityToDelete.ToSerializedByteArrayUTF8(SourceFormatting);
            req.ContentLength = arr.Length;
            Stream reqStrm = req.GetRequestStream();
            reqStrm.Write(arr, 0, arr.Length);
            reqStrm.Close();

            try
            {
                HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
                return GetResponseValue<ReturnType>(resp);
            }
            catch (WebException wex)
            {
                throw DataServiceExceptionUtil.ParseException(wex);
            }
        }

        public static string FileUpload(byte[] fileContent, string TargetFileName)
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(sUri.TrimEnd('/') + "/FileUpload?fileName='" + TargetFileName + "'");
            NetworkCredential serviceCreds = new NetworkCredential(UserName, Password);
            CredentialCache cache = new CredentialCache();
            Uri uri = new Uri(sUri);
            cache.Add(uri, "Basic", serviceCreds);
            req.Credentials = cache;
            req.Method = "POST";
            req.Timeout = 1000 * 60 * 20; // 20 minutes
            req.SendChunked = true;

            req.ContentLength = fileContent.Length;
            Stream reqStrm = req.GetRequestStream();
            reqStrm.Write(fileContent, 0, fileContent.Length);
            reqStrm.Close();

            try
            {
                HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
                return GetResponseString(resp);
            }
            catch (WebException wex)
            {
                throw DataServiceExceptionUtil.ParseException(wex);
            }
        }

        #endregion

    }
}
