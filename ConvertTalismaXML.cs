using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;

namespace TalismaXMLConversion
{
    class ConvertTalismaXML
    {
        private static Boolean IsNumeric(string stringToTest)
        {
            int result;
            return int.TryParse(stringToTest, out result);
        }

        //Take a dictionary and return JSON from the dictionary
        private static string DictionaryToJSON(Dictionary<String, String> dict)
        {
            List<string> data = new List<string>();

            //check to see if the data is a number or true/false, if it is then don't put quotes around it
            foreach (var item in dict)
            {
                if (IsNumeric(item.Value) || item.Value.ToLower() == "true" || item.Value.ToLower() == "false")
                {
                    data.Add(string.Format(@"""{0}"":{1}", item.Key, item.Value));
                }
                else
                {
                    data.Add(string.Format(@"""{0}"":""{1}""", item.Key, item.Value));
                }
            }

            return "{" + string.Join(",", data) + "}";
        }

        //Convert an XML file to JSON, send the path to the XML file
        public static string ToJSON(string Path)
        {
            List<string> ItemList = new List<string>();
            List<Dictionary<string, string>> data = new List<Dictionary<string, string>>();

            data = ToDictionary(Path);

            foreach (Dictionary<string, string> item in data)
            {
                ItemList.Add(DictionaryToJSON(item));
            }

            return "[" + string.Join(",", ItemList) + "]";
        }

        //Get a List of Dictionaries form XML file, send the path to the XML file
        public static List<Dictionary<string, string>> ToDictionary(string Path)
        {
            //Read in the XML file formatted like the following:
            //<?xml version="1.0" encoding="UTF-8" ?>
            //  <Object Name="Program Application">
            //      <Row>
            //          <p0>Contact - Contact ID</p0>
            //          <p1>Contact - Full Name-Title</p1>
            //          ....
            //          <pXX>Blah Blah</pXX>
            //      </Row>
            //      ......
            //      <Row>
            //          <p0>131231</p0>
            //          <p1>Mr.</p1>
            //          ....
            //          <pXX>Blah Blah</pXX>
            //      </Row>
            //  </Object>

            XmlDocument xdcDocument = new XmlDocument();
            xdcDocument.Load(Path);
            XmlElement xelRoot = xdcDocument.DocumentElement;
            XmlNodeList xnlNodes = xelRoot.SelectNodes("/Object/Row");
            Dictionary<String, String> headerItems = new Dictionary<String, String>();

            List<Dictionary<String, String>> ItemList = new List<Dictionary<String, String>>();

            //Get the list of headers out of the first Row (0)
            foreach (XmlNode childNode in xnlNodes.Item(0).ChildNodes)
            {
                if (childNode != null)
                {
                    headerItems.Add(childNode.Name.ToString(), childNode.InnerText.ToString());
                }
            }

            //using the headers from the first row, build a dictionary of all the items
            //and then loop through every Row and add the dictionary to ItemsList
            int counter = 0;
            foreach (XmlNode xndNode2 in xnlNodes)
            {
                if (counter > 0)
                {
                    Dictionary<String, String> dataItems = new Dictionary<String, String>();

                    foreach (XmlNode childNode2 in xndNode2.ChildNodes)
                    {
                        if (childNode2 != null && headerItems.ContainsKey(childNode2.Name.ToString()))
                        {
                            dataItems.Add(headerItems[childNode2.Name.ToString()].ToString(), childNode2.InnerText.ToString());
                        }
                    }
                    ItemList.Add(dataItems);
                }
                counter++;
            }
            return ItemList;
        }
    }
}
