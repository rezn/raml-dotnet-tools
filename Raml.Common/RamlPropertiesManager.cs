﻿using System;
using System.IO;

namespace Raml.Common
{
    public class RamlPropertiesManager
    {
        public static string BuildContent(string targetNamespace, string ramlOriginalSource, bool? useAsyncMethods, string clientRootClassName)
        {
            var source = string.Empty;
            if (ramlOriginalSource.StartsWith("http")) // only store source if is from web (i.e.: an URL)
                source = ramlOriginalSource;

            var content = string.Format("// do not edit this file{0}source: {1}{0}namespace: {2}{0}",
                Environment.NewLine, source, targetNamespace);

            if (useAsyncMethods != null)
                content += string.Format("async: {0}{1}", useAsyncMethods.Value, Environment.NewLine);

            if (!string.IsNullOrWhiteSpace(clientRootClassName))
                content += string.Format("client: {0}{1}", clientRootClassName, Environment.NewLine);

            return content;
        }

        public static RamlProperties Load(string refFilePath)
        {
            return new RamlProperties
            {
                Namespace = RamlReferenceReader.GetRamlNamespace(refFilePath),
                Source = RamlReferenceReader.GetRamlSource(refFilePath),
                ClientName = RamlReferenceReader.GetClientRootClassName(refFilePath),
                UseAsyncMethods = RamlReferenceReader.GetRamlUseAsyncMethods(refFilePath)
            };
        }

        public static void Save(RamlProperties props, string refFilePath)
        {
            var contents = BuildContent(props.Namespace, props.Source, props.ClientName, props.UseAsyncMethods);
            var fileInfo = new FileInfo(refFilePath) { IsReadOnly = false };
            File.WriteAllText(refFilePath, contents);
            fileInfo.IsReadOnly = true;
        }

        public static string BuildContent(string targetNamespace, string ramlOriginalSource, string clientName, bool? useAsyncMethods)
        {
            var content = string.Format("// do not edit this file{0}source: {1}{0}namespace: {2}{0}",
                Environment.NewLine, ramlOriginalSource, targetNamespace);

            if (!string.IsNullOrWhiteSpace(clientName))
                content += string.Format("client: {0}{1}", clientName, Environment.NewLine);
            else if (useAsyncMethods != null)
                content += string.Format("async: {0}{1}", useAsyncMethods.Value, Environment.NewLine);

            return content;
        }

    }
}