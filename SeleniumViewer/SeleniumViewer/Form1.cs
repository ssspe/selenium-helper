using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using HtmlAgilityPack;
using HtmlDocument = System.Windows.Forms.HtmlDocument;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        HtmlElement previousDoc = null;
        HtmlDocument thisdoc;
        private string previousStyle = "";
        public Form1()
        {
            InitializeComponent();
            button1.Text = "Navigate";
            webBrowser1.AllowNavigation = false;
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            webBrowser1.AllowNavigation = true;
            string val = textBox1.Text;
            webBrowser1.Navigate( val );
        }

        void thisDoc_MouseDown( object sender, HtmlElementEventArgs e)
        {
            switch ( e.ShiftKeyPressed )
            {
                case true:
                    SelectDoc(sender, e);
                    break;
                case false:
                    
                    break;
            }
        }

        private void SelectDoc(object sender, HtmlElementEventArgs e)
        {
            comboBox1.Items.Clear();
            comboBox2.Items.Clear();

            webBrowser1.AllowNavigation = false;

            List<string> outputList = new List<string>();

            var element = webBrowser1.Document.GetElementFromPoint(e.ClientMousePosition);

            string style = element.Style;
            element.Style = style + "; background-color: #ffc;";

            element = webBrowser1.Document.GetElementFromPoint(e.ClientMousePosition);
            string val = element.OuterHtml;

            var fullDocument = webBrowser1.Document.Body.OuterHtml;

            var output = processHtml(val, outputList);
            var styleOutput = checkHtml(fullDocument, output, element);

            comboBox1.Text = output.First();
            comboBox1.Items.AddRange(output.Skip(1).ToArray());

            if (styleOutput.Count > 0)
            {
                comboBox2.Text = styleOutput.First();
                comboBox2.Items.AddRange(styleOutput.Skip(1).ToArray());
            }

            if (previousDoc != null)
            {
                previousDoc.Style = previousStyle;
            }
            previousDoc = element;
            previousStyle = style;
        }

        private void webBrowser1_Navigated( object sender, WebBrowserNavigatedEventArgs e )
        {

            
            thisdoc = webBrowser1.Document;
            thisdoc.MouseOver += thisDoc_MouseDown;
            thisdoc.MouseDown += SelectDoc;
        }

        private List<string> processHtml(string html, List<string> outputList)
        {
            HtmlAgilityPack.HtmlDocument htmlDocument = new HtmlAgilityPack.HtmlDocument();
            htmlDocument.LoadHtml(html);

            var dom = htmlDocument.DocumentNode.Descendants().First().Name; 
            var element = htmlDocument.DocumentNode.Descendants().First().Attributes.ToList();
            var elementName = "";
            var value = "";

            foreach ( var elem in element )
            {
                elementName = elem.Name;
                value = elem.Value;
                string output = $"//{dom}[@{elementName}='{value}']";
                outputList.Add(output);
            }

            string text = htmlDocument.DocumentNode.Descendants().First().InnerText;
            if ( !String.IsNullOrEmpty(text) )
            {
                string domOutput = $"//{dom}[text()='{text}']";
                outputList.Add( domOutput );
            }
            return outputList;
        }

        private List<string> checkHtml(string html, List<string> outputList, HtmlElement element)
        {
            List<string> actualOutput = new List<string>();
            foreach ( var output in outputList)
            {
                HtmlAgilityPack.HtmlDocument htmlDocument = new HtmlAgilityPack.HtmlDocument();
                htmlDocument.LoadHtml(html);

                HtmlNodeCollection nodes = null;
                try
                {
                    nodes = htmlDocument.DocumentNode.SelectNodes( output );
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
                            if ( style.Contains( "rgb(255, 255, 204)" ) && !output.Contains("rgb(255, 255, 204)"))
                            {
                                actualOutput.Add("find_elements_by_xpath(\"" + output + "\")[" + count + "]");
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
                button1_Click(this, new EventArgs());
            }
        }
    }
}
