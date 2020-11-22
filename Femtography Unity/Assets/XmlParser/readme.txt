Thanks for downloading this XML parser!

There are two ways to use it:

1. Standalone in code
   In just 1 single line of code you can parse XML files!

2. As a component
   Just drag and drop on a GameObject of choice and fill in the parameters.

In both cases you are required to create the data classes yourself, so the
parser knows what you actually want from the XML file (everything you don't
want is ignored). This is very easy, see the two demo data classes. Just be
sure to follow the 4 rules:

1. Use Fields or Properties. It doesn't matter if they are public or private,
   the Parser ignores that part and sets the values anyway.

2. Only use simple datatypes. No lists, nested classes, etc. This is a simple
   XML parser/writer, not a complete load/save system for your entire
   game state.

3. For the standalone parser you need an empty (public) constructor. This also
   means that you can't use any MonoBehaviour scripts with the standalone
   parser.

4. The XmlParserComponent can only create Component classes (including the
   MonoBehaviour classes).

As you can see, very easy to use!

Enjoy!


/***********************************************************************************/
/* Standalone XML parsing.                                                         */
/* The XmlParser does not need to be attached to a GameObject or anything,         */
/* just keep it somewhere in your Assets folder.                                   */
/*                                                                                 */
/* It works for any class type with a public empty constructor.                    */
/*                                                                                 */
/* It can be called with either 1, 2 or 3 parameters:                              */
/*                                                                                 */
/* XmlParser.Read<T>(string filename);                                             */
/* XmlParser.Read<T>(string filename, bool caseSensitive);                         */
/* XmlParser.Read<T>(string filename, bool caseSensitive, bool showDebugLogging);  */
/*                                                                                 */
/* Parameter <T> is the type you want to parse from the XML.                       */
/*                                                                                 */
/* Default caseSensitive = false and showDebugLogging = false.                     */
/*                                                                                 */
/* Case sensitivity is relevant when you have multiple fields or properties with   */
/* the same name but with different case (not a good idea anyway).                 */
/***********************************************************************************/



/**************************************************************************************/
/* It is also possible to download and process and xml file from a URL.               */
/*                                                                                    */
/* The download is asynchronous so it won't block your main thread.                   */
/*                                                                                    */
/* See demo for syntax.                                                               */
/**************************************************************************************/



/***********************************************************************************/
/* XML parsing as a Component                                                      */
/* Attach the XmlParserComponent to your GameObject of choice.                     */
/*                                                                                 */
/* It adds components to the attached GameObject for each parsed element, or it    */
/* adds new GameObjects for each parsed element (your choice).                     */
/*                                                                                 */
/* Case sensitivity is relevant when you have multiple fields or properties with   */
/* the same name but with different case (not a good idea anyway).                 */
/***********************************************************************************/



/*********************************************************************************************************/
/* The following part shows XML writing.                                                                 */
/*                                                                                                       */
/* It can be called with either 1, 2 or 3 parameters:                                                    */
/*                                                                                                       */
/* XmlParser.Write(objects, string filename, bool overwrite);                                            */
/* XmlParser.Write(objects, string filename, bool overwrite, bool onlyPublic);                           */
/* XmlParser.Write(objects, string filename, bool overwrite, bool onlyPublic, bool showDebugLogging);    */
/*                                                                                                       */
/* Default onlyPublic = true and showDebugLogging = false.                                               */
/*                                                                                                       */
/* When overwrite is true, the file will be overwritten if it already exists. When overwrite is false    */
/* and the file already exists, the data will be appended to the root xml node present. If the existing  */
/* file is not an xml document, writing will do nothing and log an error.                                */
/*                                                                                                       */
/* When onlyPublic is true, only the public properties and fields of your object will be written.        */
/* When onlyPublic is false, all other properties and fields (private, internal, protected) will be      */
/* written as well.                                                                                      */
/*********************************************************************************************************/