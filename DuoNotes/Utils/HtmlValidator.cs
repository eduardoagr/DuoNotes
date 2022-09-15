namespace DuoNotes.Utils
{
    public class HtmlValidator
    {

        /// <summary>
        /// Correcting void elements in the HTML. 
        /// </summary>
        /// <param name="inputString">HTML string with void HTML elements</param>
        /// <returns></returns>
        public static string IgnoreVoidElementsInHTML(string inputString)
        {
            inputString = inputString.Replace("<meta http-equiv=\"Content-Type\" content=\"application/xhtml+xml; charset=utf-8\">", "");
            inputString = inputString.Replace("<br>", "<br/>");
            inputString = inputString.Replace("\n", "");
            inputString = inputString.Replace("\r", "");
            inputString = inputString.Replace("<title></title>", "");
            inputString = inputString.Replace("﻿<?xml version=\"1.0\" encoding=\"utf-8\"?><!DOCTYPE html PUBLIC" +
                " \"-//W3C//DTD XHTML 1.1//EN\" \"http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd\">", "");
            return inputString;
        }
    }
}
