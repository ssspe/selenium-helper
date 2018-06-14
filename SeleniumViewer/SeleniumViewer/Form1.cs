using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

using HtmlAgilityPack;
using HtmlDocument = System.Windows.Forms.HtmlDocument;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        HtmlElement currentElement;
        HtmlElement previousElement;
        HtmlDocument thisdoc;
        private string highlightColor = "rgb(255, 255, 204)";
        private string previousStyle = "";

        public Form1()
        {
            //var color = Color.FromArgb();
            InitializeComponent();
            webBrowser1.AllowNavigation = false;  
        }

        private void navigateButton_Click(object sender, EventArgs e)
        {
            webBrowser1.AllowNavigation = true;
            string val = textBox1.Text;
            webBrowser1.Navigate( val );
        }

        private void webBrowser1_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            thisdoc = webBrowser1.Document;
            thisdoc.MouseOver += thisDoc_MouseDown;
            thisdoc.MouseDown += SelectDoc;
        }

        void thisDoc_MouseDown( object sender, HtmlElementEventArgs e)
        {
            if ( e.ShiftKeyPressed )
            {
                SelectDoc(sender, e);
            }
        }

        private void SelectDoc(object sender, HtmlElementEventArgs e)
        {
            seleniumStrings.Items.Clear();

            webBrowser1.AllowNavigation = false;
            
            HtmlElement currentElement = webBrowser1.Document.GetElementFromPoint(e.ClientMousePosition);

            string elementStyle = currentElement.Style;
            currentElement.Style = elementStyle + $"; background-color: {highlightColor};";

            currentElement = webBrowser1.Document.GetElementFromPoint(e.ClientMousePosition);
            string elementHtml = currentElement.OuterHtml;

            var fullDocumentHtml = webBrowser1.Document.Body.OuterHtml;

            List<string[]> processedHtml = processHtml(elementHtml);
            List<string> htmlStrings = checkHtml(fullDocumentHtml, processedHtml, currentElement);

            if (htmlStrings.Count > 0)
            {
                seleniumStrings.Text = htmlStrings.First();
                seleniumStrings.Items.AddRange(htmlStrings.Skip(1).ToArray());
            }

            if (previousElement != null)
            {
                previousElement.Style = previousStyle;
            }
            previousElement = currentElement;
            previousStyle = elementStyle;
        }

        private List<string[]> processHtml(string html)
        {
            List<string[]> outputList = new List<string[]>();
            HtmlAgilityPack.HtmlDocument htmlDocument = new HtmlAgilityPack.HtmlDocument();
            htmlDocument.LoadHtml(html);

            var dom = htmlDocument.DocumentNode.Descendants().First().Name; 
            var element = htmlDocument.DocumentNode.Descendants().First().Attributes.ToList();

            foreach ( var elem in element )
            {
                var elementName = elem.Name;
                var value = elem.Value;
                string output = $"//{dom}[@{elementName}='{value}']";
                string[] outputString = { dom, elementName, value};
                outputList.Add(outputString);
            }

            string text = htmlDocument.DocumentNode.Descendants().First().InnerText;
            if ( !String.IsNullOrEmpty(text) )
            {
                string[] domOutput = { dom, text};
                outputList.Add( domOutput );
            }
            return outputList;
        }

        private List<string> checkHtml(string html, List<string[]> outputList, HtmlElement element)
        {
            List<string> actualOutput = new List<string>();
            foreach ( var output in outputList)
            {
                HtmlAgilityPack.HtmlDocument htmlDocument = new HtmlAgilityPack.HtmlDocument();
                htmlDocument.LoadHtml(html);

                string fullOutput;

                if ( output.Length == 3 )
                { 
                    fullOutput = $"//{output[0]}[@{output[1]}='{output[2]}']";
                }
                else
                {
                    fullOutput = $"//{output[0]}[text()='{output[1]}']";
                }

                HtmlNodeCollection nodes = null;
                try
                {
                    nodes = htmlDocument.DocumentNode.SelectNodes(fullOutput);
                }
                catch
                {
                    //Nothing
                }
                try
                {
                    int count = 0;
                    foreach ( var node in nodes )
                    {
                        try
                        {
                            var style = node.Attributes["Style"].Value;
                            if ( style.Contains( highlightColor ) && !fullOutput.Contains(highlightColor))
                            {
                                if ( output[1] == "class" )
                                {
                                    actualOutput.Add(
                                        "find_elements_by_class_name(\"" + output[2] + "\")[" +
                                        count + "]" );
                                }
                                else
                                {
                                    actualOutput.Add(
                                        "find_elements_by_xpath(\"" + fullOutput + "\")[" + count +
                                        "]" );
                                }
                            }  
                        }
                        catch
                        {
                            //Nothing
                        }
                        count++;
                    }
                }
                catch
                {
                    //Nothing
                }
            }
            return actualOutput;
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                navigateButton_Click(this, new EventArgs());
            }
        }
    }
}
