using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POC.FrontEnd.Helper
{
    public static class MasterLayoutHelper
    {


        private const string LineeForDelayedJavaScriptFiles = "LinesForDelayedJavaScript";
        private const string LinesForDocumentReadyKey = "LinesForDocumentReady";

        public static void AddLineToDocumentReadyFunction(string line, HttpContext context)
        {
            StringBuilder sb;

            if (context.Items[LinesForDocumentReadyKey] == null)
            {
                sb = new StringBuilder();
                context.Items[LinesForDocumentReadyKey] = sb;
            }
            else
            {
                sb = (StringBuilder)context.Items[LinesForDocumentReadyKey];
            }

            sb.AppendLine(line);
        }


        public static string GetLinesForDocumentReadyFunction(HttpContext context)
        {
            if (context.Items[LinesForDocumentReadyKey] == null)
                return "";

            var sb = (StringBuilder)context.Items[LinesForDocumentReadyKey];

            return sb.ToString();
        }


        public static void AddForDelayedJavaScriptFile(string javascriptFilePath, HttpContext context)
        {
            List<string> list;

            if (context.Items[LineeForDelayedJavaScriptFiles] == null)
            {
                list = new List<string>();
                context.Items[LineeForDelayedJavaScriptFiles] = list;
            }
            else
            {
                list = (List<string>)context.Items[LineeForDelayedJavaScriptFiles];
            }

            list.Add(javascriptFilePath);
        }


        public static string GetDelayedJavaScriptFiles(HttpContext context)
        {
            if (context.Items[LineeForDelayedJavaScriptFiles] == null)
                return "";

            var sb = new StringBuilder();
            var list = (List<string>)context.Items[LineeForDelayedJavaScriptFiles];
            foreach (var item in list)
            {
                sb.AppendLine("<script src=\"" + item + "\"></script>");
            }

            return sb.ToString();
        }


    }
}
