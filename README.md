Talisma-Export-Conversion-JSON-DotNet
=====================================

This is a class that can be used to convert Talisma XML exports to JSON or to a List of Dictionaries in .NET

XML Output Format
The Talisma XML exported file format must appear like the following.  NOTE: It must have a header row.

    <?xml version="1.0" encoding="UTF-8" ?>
      <Object Name="Program Application">
          <Row>
            <p0>Contact - Contact ID</p0>
            <p1>Contact - Full Name - Title</p1>
            ....
            <pXX>Blah Blah</pXX>
          </Row>
            ......
          <Row>
            <p0>131231</p0>
            <p1>Mr.</p1>
            ....
            <pXX>Blah Blah</pXX>
          </Row>
      </Object>


How To Use
==========

You don't need to instantiate a class, you can just call the functions directly:

Getting JSON Data
ConvertTalismaXML.ToJSON(XMLPath)
Where XMLPath is the path to your XML File, such as c:\exports\talismaexport.xml

Getting a List of Dictionaries
List<Dictionary<string, string>> userlist = ConvertTalismaXML.ToDictionary(XMLPath);
Where XMLPath is the path to your XML file.
The dictionary key's will be the header names from the XML file.

You may want to change the namespace in the ConvertTalismaXML.cs file.


